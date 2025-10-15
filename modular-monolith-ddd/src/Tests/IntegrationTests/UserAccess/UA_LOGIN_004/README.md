# UA-LOGIN-004 — API Authorization Integration Tests

## Purpose
Validate API authorization behavior for endpoints protected by the `ApiScope` policy. Ensures correct HTTP responses for authenticated/unauthenticated requests and scope-based access control.

## Scope
- Endpoint under test: `GET /api/Protected/ping`
- Policy: `ApiScope` (requires `scope` claim containing the configured scope name)
- Technology: NUnit + `Microsoft.AspNetCore.Mvc.Testing`
- Test harness: `WebApplicationFactory` with a custom `TestAuthHandler` (header-driven)

## Test Cases
- 200 OK: Authenticated request with required scope.
- 401 Unauthorized: Unauthenticated request (no identity).
- 403 Forbidden: Authenticated request without the required scope.

## How It Works
- `ApiFactory` sets `Identity:Scope:Name` to `modular-monolith-ddd-api` for consistency during tests.
- `TestAuthHandler` reads headers to emulate identity:
  - `X-Test-Auth`: `none` for unauthenticated; any other value means authenticated.
  - `X-Test-Name`: optional display/user name.
  - `X-Test-Scopes`: space-separated scopes (e.g., `modular-monolith-ddd-api`).
- The `ApiScope` policy in the API checks the `scope` claim for the configured scope value.

## Files
- `ApiFactory.cs`: Configures test host and injects `TestAuthHandler` as the default scheme.
- `TestAuthHandler.cs`: Header-driven authentication for deterministic test identities.
- `ProtectedEndpointTests.cs`: NUnit tests for 200/401/403 scenarios.

## Running
- From solution root:
  - `dotnet test` (runs all tests)
  - Or run the `ModularMonolithDDD.Tests.IntegrationTests` project
- Ensure the API project is referenced by the IntegrationTests project.

## Headers Reference
- Unauthenticated (401):
  - No headers or `X-Test-Auth: none`
- Authenticated without scope (403):
  - `X-Test-Auth: user`
  - `X-Test-Name: tester`
- Authenticated with required scope (200):
  - `X-Test-Auth: user`
  - `X-Test-Name: tester`
  - `X-Test-Scopes: modular-monolith-ddd-api`

## Notes
- These tests verify API middleware/authorization behavior, not token issuance.
- Module-level integration tests should continue focusing on command/query and infrastructure behaviors.