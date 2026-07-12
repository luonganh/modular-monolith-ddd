# UserAccess Layer Architecture Rules (Module-Level Architecture Tests)
# Test dependency/layer rules

- The Domain layer must not depend on the Application layer.
- The Domain layer must not depend on the Infrastructure layer.
- The Domain layer must not depend on the API layer.
- The Application layer must not depend on the Infrastructure layer.
- The Application layer must not depend on the API layer.
