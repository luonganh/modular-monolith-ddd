---
description: Fix all ESLint errors in mmdddshop-frontend-admin (any / Function types), with Vietnamese explanations
---

On branch fix/frontend-lint, fix all ESLint errors in mmdddshop-frontend-admin.

Context:
- eslint.config.js currently downgrades two rules to "warn":
  @typescript-eslint/no-explicit-any and @typescript-eslint/no-unsafe-function-type.
  Remove those two override lines first, then fix every resulting error.

Rules:
- Replace `Function` props with explicit signatures (e.g. `() => void`, `(value: string) => void`)
  based on how each prop is actually called.
- Replace `any` with real types: Redux Toolkit `PayloadAction<T>`, API response/request types,
  axios types, etc. Create shared types in src/shared/types if needed.
  Use `unknown` (with narrowing) only where the type is genuinely unknown, e.g. caught errors.
- Do NOT use `as any`, `// eslint-disable`, or `@ts-ignore` to silence errors.
- Do not change runtime behaviour. Fix the 2 react-hooks/exhaustive-deps warnings only if
  it is safe; otherwise list them for me.
- Work file by file, commit in small logical commits using Conventional Commits
  (e.g. "fix(frontend-admin): type saga actions in authSaga").

Learning (I'm new to React and TypeScript):
- After each file or commit, explain briefly in Vietnamese, in the chat (not as code comments),
  what the type change means and why it is safer than `any` / `Function`.
- Keep code comments in English, and only where they genuinely help.

Done when (run in mmdddshop-frontend-admin):
- npm run lint  → 0 errors
- npm run build → succeeds

Finally:
- Update the ESLint note at the end of the "Frontend architecture" section in CLAUDE.md
  (the rules are no longer downgraded to warnings).
- Summarise what changed and anything you were unsure about.
- Give me a short manual test checklist: which screens and actions to click through in
  `npm run dev` to confirm nothing broke (login, pagination, dialogs, dropdowns, toggles, etc.).
