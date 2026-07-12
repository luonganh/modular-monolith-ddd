# Cross-Module Integration Test Scenarios
# Test how multiple modules coordinate with each other through integration events, outbox, inbox, and eventual consistency

These scenarios verify workflows that involve more than one module and require module-to-module coordination through public contracts, integration events, outbox/inbox processing, or other integration mechanisms.

- When Module A publishes an integration event, Module B should receive and process it correctly.
- When a cross-module workflow succeeds, each involved module should persist its own state changes in the database.
- When an integration event is processed, the receiving module should map event data to its own commands or state correctly.
- When outbox processing succeeds, the integration event should be published and marked as processed.
- When inbox processing succeeds, the received message should be processed only once.
- When the same integration event is received more than once, the receiving module should handle it idempotently.
- When one module changes internal implementation details, other modules should remain unaffected as long as the public contract is unchanged.
- Queries in the receiving module should eventually reflect state changes caused by integration events.
- Cross-module workflows should use eventual assertions instead of fixed delays.