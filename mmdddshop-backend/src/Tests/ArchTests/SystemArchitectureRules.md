# System Architecture Rules 

These rules apply to system-level architecture tests and protect boundaries between the API and modules, and between modules themselves.

- A module must not depend on another module, except through integration events or public contracts.
- The API layer must call only public module contracts and must not call infrastructure internals directly.
