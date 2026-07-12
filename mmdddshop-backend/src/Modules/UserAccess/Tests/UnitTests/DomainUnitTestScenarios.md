# UserAccess Domain Unit Test Scenarios (Domain Unit Test test Scenarios)
# Test business behavior in memory, no database

- When a valid user is created, it should publish a `UserCreatedDomainEvent`.
- When a user profile is updated, it should publish a `UserProfileUpdatedDomainEvent`.
- When a valid role is added, the user should have that role.
- When a duplicate role is added, it should either be rejected or not create a duplicate, depending on the designed domain rule.
- When invalid data is provided, the appropriate business rule should be thrown.
