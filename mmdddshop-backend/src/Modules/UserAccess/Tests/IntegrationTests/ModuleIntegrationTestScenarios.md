# UserAccess Module Integration Test Scenarios (Module-Level Integration Tests)
# Test command/query pipeline (ExecuteCommandAsync / ExecuteQueryAsync )+ DI + database + infrastructure wiring

- When a valid admin user command is executed, the user should be persisted in the database.
- When invalid user data is provided, the command should fail through the real command pipeline.
- When a duplicate login is used, the command should fail according to the business rule.
- When a command succeeds, related domain events should be dispatched and outbox messages should be stored if applicable.
- Queries should return data written by commands through the real database/read model.