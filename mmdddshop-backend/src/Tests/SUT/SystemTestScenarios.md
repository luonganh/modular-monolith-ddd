# System Test Scenarios
# Test the application through its external boundary (HTTP/API endpoints/startup/authentication/middleware/serialization)

These scenarios verify the application as a complete system from its external boundary, such as HTTP API endpoints, application startup, middleware, authentication, authorization, serialization, dependency injection, modules, and database integration.

- The application should start successfully with the test configuration.
- Public health or home endpoints should return a successful response.
- API endpoints should return the expected HTTP status codes for valid requests.
- API endpoints should return validation errors for invalid request payloads.
- Requests without authentication should return `401 Unauthorized` for protected endpoints.
- Requests with insufficient permissions should return `403 Forbidden` for protected endpoints.
- Requests with valid authentication and permissions should be processed successfully.
- JSON request and response serialization should match the public API contract.
- Middleware should translate domain and command validation errors into the expected problem details responses.
- Critical end-to-end workflows should succeed through the real API boundary.