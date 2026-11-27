# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout

- `mmdddshop-backend/` — .NET 9 modular monolith (DDD, CQRS) for "Mmdddshop". Solution file is `Mmdddshop.sln` at the repo root.
- `mmdddshop-frontend-admin/` — React 18 + TypeScript + Vite admin SPA (Redux Toolkit, redux-saga, Tailwind, oidc-client-ts).
- `keycloak/` — Keycloak image and realm import (`mmdddshop` realm). Keycloak is the identity provider for both the API and the SPA.
- `mmdddshop-backend/src/Database/` — DB init container plus Liquibase migrations (`liquibase/changelog.xml`, `liquibase/migrations/NNN_*.sql`). Schema changes go in a new numbered SQL migration, not EF migrations.
- Root `.env*` files configure everything (backend, Docker Compose, and the Vite build: `vite.config.ts` sets `envDir` to the repo root and exposes `VITE_*` and `DASHBOARD_*` vars).

## Commands

Backend (run from the repo root):

```bash
dotnet restore Mmdddshop.sln
dotnet build Mmdddshop.sln
dotnet test Mmdddshop.sln --filter "TestCategory!=Integration"     # what CI runs (unit + architecture tests)
dotnet test Mmdddshop.sln                                          # also integration tests, needs SQL Server reachable via .env
dotnet test <path/to/Tests.csproj> --filter "FullyQualifiedName~AddAdminUserTests"   # single test or class
dotnet run --project mmdddshop-backend/src/API/Mmdddshop.API
```

Tests use NUnit, FluentAssertions, NSubstitute and NetArchTest. Only the system-level `[Category("Integration")]` tests are excluded by the CI filter.

Frontend (from `mmdddshop-frontend-admin/`; Node 24):

```bash
npm ci
npm run dev      # Vite dev server on port 3010
npm run lint     # eslint .
npm run build    # tsc -b && vite build (type errors fail the build)
```

Docker (from the repo root):

```bash
docker compose up -d                              # infra only: SQL Server, db-init, Liquibase, Keycloak
docker compose --profile local-docker up -d --build   # also backend-api and frontend-admin in containers
docker compose -f docker-compose.yml -f docker-compose.prod.yml config --quiet   # validate the prod compose
```

`docker-compose.yml` is the shared base, `docker-compose.override.yml` (auto-applied locally) adds ports and puts `backend-api`/`frontend-admin` behind the `local-docker` profile, and `docker-compose.prod.yml` is for staging/production. Startup order: `db-sqlserver` → `db-init` → `liquibase` → `backend-api` (healthcheck `GET /api/home`) → `frontend-admin`.

## CI/CD

- `.github/workflows/ci.yml` runs on PRs to `develop`, `main` and `release/**`, and on pushes to `develop`. It builds and tests the backend, lints and builds the frontend, and validates and builds the compose files.
- `.github/workflows/deploy.yml`: pushes to `release/**` deploy to staging and pushes to `main` deploy to production. It builds images tagged with the commit SHA, pushes them to Docker Hub, then SSHes to the VPS and runs compose. Running it manually with `image_tag` set does a rollback and skips CI.

## Git workflow (GitFlow)

- Branch from `develop`: `feature/*`, `fix/*`, `refactor/*`, `chore/*`, `docs/*`, `ci/*`. Open the PR into `develop`.
- `release/*` (from `develop`) deploys to staging; `release/*` and `hotfix/*` are the only branches merged into `main`.
- Never push directly to `develop`, `release/*` or `main`. Never create tags or trigger deployments — releases are done by a human.
- Use Conventional Commits (`fix(frontend-admin): ...`, `refactor(user-access): ...`).

## Backend architecture

### Project references are implicit

`Directory.Build.targets` at the repo root adds most `ProjectReference`s **by naming convention** (project name suffix and folder). Examples: the API references every `Modules/**/*.Infrastructure.csproj`; a module's `*.Application` references its `*.Domain` and `*.IntegrationEvents`; `*.Infrastructure` references `*.Application`; test projects reference the BuildingBlocks integration-test SeedWork. When you add a module or project, follow the existing naming (`Mmdddshop.Modules.<Name>.{Domain,Application,Infrastructure,IntegrationEvents}`) and folder layout, or references won't be wired. Package versions are managed centrally in `Directory.Packages.props`.

### Module structure (example: `Modules/UserAccess`)

- **Domain**: aggregates, value objects, domain events, and business rules (`BusinessRuleValidationException`). Base types come from `BuildingBlocks.Domain`.
- **Application**: one folder per use case (`Users/CreateUser/{Command,Handler}`). `Contracts/` holds the module's **public API**: `IUserAccessModule`, `CommandBase`/`QueryBase`, `ICommand`/`IQuery`. Reads use Dapper via `ISqlConnectionFactory`; writes use EF Core through repositories.
- **Infrastructure**: the module's own Autofac composition root (`UserAccessCompositionRoot`) and `UserAccessStartup.Initialize(...)`. That startup builds a **separate, module-private container**, starts Quartz jobs (outbox, inbox and internal-command processing), and subscribes to the event bus. `UserAccessModule` is the facade that implements `IUserAccessModule`:
  - Commands go through `CommandsExecutor`, then a MediatR decorator chain (`Processing/`: logging → validation via FluentValidation → unit of work).
  - Queries resolve MediatR directly in a new lifetime scope.
- **IntegrationEvents**: the public events other modules may consume.

### API host (`src/API/Mmdddshop.API`)

- `Program.cs` uses Autofac. It registers each module facade through an Autofac module (`Modules/UserAccess/UserAccessAutofacModule.cs`) and calls each module's `*Startup.Initialize` in `InitializeModules`. A new module must be wired in both places.
- Controllers talk to modules **only** through `I<Module>Module.ExecuteCommandAsync/ExecuteQueryAsync`. Architecture tests enforce this.
- Auth is Keycloak JWT bearer, configured with `KEYCLOAK_AUTHORITY`, `KEYCLOAK_AUDIENCE` and `KEYCLOAK_METADATA_ADDRESS`. `ProvisioningMiddleware` provisions the local user from token claims after authentication. `CorrelationMiddleware`, `ExceptionHandlerMiddleware` and Hellang ProblemDetails map `InvalidCommandException` and `BusinessRuleValidationException` to 400.
- Configuration: `EnvironmentHelper` (BuildingBlocks.Infrastructure) loads the repo-root `.env` (in Development) or `.env.{environment}` then `.env` (other environments), or `/app` inside Docker. It builds the connection string from the `SQLSERVER_*` env vars.

### Architecture rules (enforced by NetArchTest)

Rules are documented next to the tests; read them before changing boundaries:
- `src/Tests/ArchTests/SystemArchitectureRules.md`: modules must not depend on each other except through integration events or public contracts, and the API calls only public module contracts.
- `src/Modules/UserAccess/Tests/ArchTests/LayerArchitectureRules.md`: layer rules inside a module.

Each test project also has a `*TestScenarios.md` that lists its intended scenarios.

## Frontend architecture

- Feature code lives in `src/modules/<feature>/` (`pages/`, `services/`, `store/`, `index.ts`). Shared Redux store, sagas, API layer and hooks are in `src/shared/store/`.
- Use the path aliases from `tsconfig.app.json` (`components/*`, `modules/*`, `shared/*`, `store/*`, `utils/*`, `configuration/*`, and others) rather than long relative imports.
- Auth is OIDC (authorization code) against Keycloak via `oidc-client-ts`, using client `admin-spa` with the redirect at `/callback`.
- ESLint enforces `@typescript-eslint/no-explicit-any` and `@typescript-eslint/no-unsafe-function-type` as errors (CI fails on them). Type callback props with explicit signatures (e.g. `(page: number) => void`, `() => React.ReactNode`) and use `unknown` plus narrowing for genuinely unknown values such as caught errors; `shared/utils/errors.ts` has `getErrorMessage` for the latter.

## Rules

- Never silence errors with `as any`, `// eslint-disable` or `@ts-ignore`; fix the types instead.
- Liquibase: never edit a changeset that has already been applied; new migration files must not contain `USE [db]` (the connection already selects the database).
- Never put real secrets in `.env.example` or any other committed file. Staging/production values live in GitHub Environments.
- If Docker, compose, `.env*`, nginx or `.sh` files change, verify with `docker compose --profile local-docker up -d --build` before opening a PR. Shell scripts must use LF line endings and no trailing whitespace after `\`.
- Don't change `docker-compose.prod.yml` or `.github/workflows/deploy.yml` without saying so explicitly in the summary.
