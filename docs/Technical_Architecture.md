# E-commerce Lite — Technical Architecture

## **1. Tổng quan kiến trúc**

### **1.1. Kiến trúc tổng thể**
```
┌─────────────────────────────────────────────────────────────┐
│                    E-commerce Lite System                   │
├─────────────────────────────────────────────────────────────┤
│  Frontend Layer (React 19.1.1 + Vite)                      │
│  ├── Admin Portal (react-dashboard-template)               │
│  └── Customer Portal (Custom React)                        │
├─────────────────────────────────────────────────────────────┤
│  Identity & Access Management (Keycloak)                   │
│  ├── OAuth 2.0 + PKCE Authentication                      │
│  ├── JWT Bearer Token Management                          │
│  └── Role-based Authorization                             │
├─────────────────────────────────────────────────────────────┤
│  API Layer (.NET 9 + ASP.NET Core)                         │
│  ├── Modular Monolith Architecture                         │
│  ├── CQRS + MediatR Pattern                                │
│  └── OpenAPI/Swagger Documentation                         │
├─────────────────────────────────────────────────────────────┤
│  Domain Layer (DDD + Clean Architecture)                   │
│  ├── UserAccess Module                                     │
│  ├── Customer Module                                       │
│  ├── Product Module                                        │
│  ├── Order Module                                          │
│  ├── Payment Module                                        │
│  ├── Inventory Module                                      │
│  ├── Shipping Module                                       │
│  ├── Notification Module                                   │
│  └── Reporting Module                                       │
├─────────────────────────────────────────────────────────────┤
│  Infrastructure Layer                                       │
│  ├── SQL Server Database                                   │
│  ├── Liquibase Migrations                                  │
│  ├── Event Sourcing (SqlStreamStore)                      │
│  ├── Outbox Pattern                                        │
│  └── Background Jobs (Quartz)                             │
├─────────────────────────────────────────────────────────────┤
│  External Services                                          │
│  ├── VNPay, MoMo, Stripe (Payment)                         │
│  ├── SMTP/SendGrid (Email)                                 │
│  ├── Twilio/Viettel (SMS)                                  │
│  └── Shipping APIs                                         │
└─────────────────────────────────────────────────────────────┘
```

### **1.2. Technology Stack Overview**

| Layer | Technology | Version | Purpose |
|-------|------------|---------|---------|
| **Frontend** | React | 19.1.1 | UI Framework |
| **Frontend** | TypeScript | 5.0+ | Type Safety |
| **Frontend** | Vite | 5.0+ | Build Tool |
| **Frontend** | Redux Toolkit | 2.0+ | State Management |
| **Frontend** | Redux Saga | 1.3+ | Side Effects |
| **Frontend** | Tailwind CSS | 3.4+ | Styling |
| **Backend** | .NET | 9.0 | Runtime |
| **Backend** | ASP.NET Core | 9.0 | Web Framework |
| **Backend** | Entity Framework Core | 9.0 | ORM |
| **Backend** | Dapper | 2.1+ | Micro ORM |
| **Backend** | MediatR | 12.2+ | CQRS |
| **Backend** | Quartz | 3.14+ | Background Jobs |
| **Database** | SQL Server | 2019 | Primary Database |
| **Database** | Liquibase | 4.23+ | Migrations |
| **Identity** | Keycloak | Latest | OAuth 2.0 + PKCE |
| **Infrastructure** | Docker | Latest | Containerization |
| **Infrastructure** | Docker Compose | Latest | Orchestration |

---

## **2. Frontend Architecture**

### **2.1. Admin Portal (react-dashboard-template)**

#### **2.1.1. Technology Stack**
```json
{
  "dependencies": {
    "react": "^19.1.1",
    "react-dom": "^19.1.1",
    "typescript": "^5.0.0",
    "vite": "^5.0.0",
    "@reduxjs/toolkit": "^2.0.0",
    "redux-saga": "^1.3.0",
    "react-redux": "^9.0.0",
    "react-router-dom": "^6.0.0",
    "axios": "^1.6.0",
    "tailwindcss": "^3.4.0",
    "@headlessui/react": "^1.7.0",
    "react-hook-form": "^7.48.0",
    "zod": "^3.22.0",
    "keycloak-js": "^23.0.0",
    "react-query": "^3.39.0",
    "react-i18next": "^13.5.0",
    "framer-motion": "^10.16.0",
    "recharts": "^2.8.0",
    "react-table": "^7.8.0"
  }
}
```

#### **2.1.2. Project Structure**
```
modular-monolith-ddd-frontend-admin/
├── src/
│   ├── components/
│   │   ├── Elements/           # Reusable UI components
│   │   ├── Footer/             # Footer component
│   │   └── Layout/             # Layout components
│   ├── pages/
│   │   ├── Auth/               # Authentication pages
│   │   ├── Dashboard/          # Dashboard pages
│   │   ├── Products/           # Product management
│   │   ├── Orders/             # Order management
│   │   ├── Customers/          # Customer management
│   │   ├── Reports/            # Reporting pages
│   │   └── Settings/           # Settings pages
│   ├── store/
│   │   ├── api/                # RTK Query API slices
│   │   ├── reducers/           # Redux reducers
│   │   ├── sagas/              # Redux Saga watchers
│   │   └── hooks/              # Typed Redux hooks
│   ├── hooks/                  # Custom React hooks
│   ├── utils/                  # Utility functions
│   └── types/                  # TypeScript types
├── public/
│   └── locales/                # i18n translations
└── package.json
```

#### **2.1.3. Dashboard Template Features**
- **Responsive Design**: Mobile-first approach
- **Dark/Light Theme**: Theme switching
- **Internationalization**: Multi-language support
- **Sidebar Navigation**: Collapsible sidebar
- **Breadcrumb Navigation**: Page navigation
- **Data Tables**: Sortable, filterable tables
- **Charts & Graphs**: Data visualization
- **Form Components**: Reusable form elements

### **2.2. Customer Portal (Custom React)**

#### **2.2.1. Technology Stack**
```json
{
  "dependencies": {
    "react": "^19.1.1",
    "react-dom": "^19.1.1",
    "typescript": "^5.0.0",
    "vite": "^5.0.0",
    "@reduxjs/toolkit": "^2.0.0",
    "redux-saga": "^1.3.0",
    "react-redux": "^9.0.0",
    "react-router-dom": "^6.0.0",
    "axios": "^1.6.0",
    "tailwindcss": "^3.4.0",
    "@headlessui/react": "^1.7.0",
    "react-hook-form": "^7.48.0",
    "zod": "^3.22.0",
    "keycloak-js": "^23.0.0",
    "react-query": "^3.39.0",
    "framer-motion": "^10.16.0"
  }
}
```

#### **2.2.2. Project Structure**
```
modular-monolith-ddd-frontend-portal/
├── src/
│   ├── components/
│   │   ├── Product/            # Product components
│   │   ├── Cart/               # Shopping cart
│   │   ├── Checkout/           # Checkout flow
│   │   └── Layout/             # Layout components
│   ├── pages/
│   │   ├── Home/               # Home page
│   │   ├── Products/           # Product catalog
│   │   ├── ProductDetail/       # Product details
│   │   ├── Cart/               # Shopping cart
│   │   ├── Checkout/           # Checkout
│   │   ├── Orders/             # Order history
│   │   └── Profile/            # User profile
│   ├── store/
│   │   ├── api/                # RTK Query API slices
│   │   ├── reducers/           # Redux reducers
│   │   ├── sagas/              # Redux Saga watchers
│   │   └── hooks/              # Typed Redux hooks
│   ├── hooks/                  # Custom React hooks
│   ├── utils/                  # Utility functions
│   └── types/                  # TypeScript types
└── package.json
```

### **2.3. State Management (Redux Toolkit + Redux Saga)**

#### **2.3.1. Redux Store Structure**
```typescript
// Store configuration with Redux Saga
interface RootState {
  auth: AuthState;           // Authentication state
  user: UserState;           // User profile
  products: ProductState;    // Product catalog
  orders: OrderState;        // Order management
  cart: CartState;           // Shopping cart
  notifications: NotificationState; // Notifications
  ui: UIState;               // UI state (loading, modals, etc.)
}

// Redux Saga middleware
const sagaMiddleware = createSagaMiddleware();
const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      thunk: false, // Disable thunk in favor of saga
      serializableCheck: {
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }).concat(sagaMiddleware),
});
```

#### **2.3.2. Redux Saga Implementation**
```typescript
// Root Saga
function* rootSaga() {
  yield all([
    fork(authSaga),
    fork(userSaga),
    fork(productSaga),
    fork(orderSaga),
    fork(cartSaga),
    fork(notificationSaga),
    fork(uiSaga),
  ]);
}

// Auth Saga
function* authSaga() {
  yield takeEvery(LOGIN_REQUEST, loginSaga);
  yield takeEvery(LOGOUT_REQUEST, logoutSaga);
  yield takeEvery(REFRESH_TOKEN_REQUEST, refreshTokenSaga);
}

function* loginSaga(action: LoginRequestAction) {
  try {
    const { username, password } = action.payload;
    const response = yield call(api.login, { username, password });
    yield put(loginSuccess(response.data));
    yield put(push('/dashboard'));
  } catch (error) {
    yield put(loginFailure(error.message));
  }
}

// Product Saga
function* productSaga() {
  yield takeEvery(FETCH_PRODUCTS_REQUEST, fetchProductsSaga);
  yield takeEvery(CREATE_PRODUCT_REQUEST, createProductSaga);
  yield takeEvery(UPDATE_PRODUCT_REQUEST, updateProductSaga);
}

function* fetchProductsSaga(action: FetchProductsRequestAction) {
  try {
    const { page, limit, filters } = action.payload;
    const response = yield call(api.getProducts, { page, limit, filters });
    yield put(fetchProductsSuccess(response.data));
  } catch (error) {
    yield put(fetchProductsFailure(error.message));
  }
}
```

---

## **3. Backend Architecture**

### **3.1. Technology Stack**

#### **3.1.1. Core Framework**
- **.NET 9**: Runtime và framework
- **ASP.NET Core 9.0**: Web framework
- **Entity Framework Core 9.0**: Primary ORM
- **Dapper 2.1.24**: Micro ORM cho performance
- **MediatR 12.2.0**: CQRS pattern implementation
- **Autofac 7.1.0**: Dependency injection container

#### **3.1.2. Authentication & Authorization**
- **Keycloak**: Identity provider
- **OAuth 2.0 + PKCE**: Authentication flow
- **JWT Bearer Tokens**: Token-based authentication
- **Role-based Authorization**: Fine-grained permissions

#### **3.1.3. Data Access & Persistence**
- **SQL Server 2019**: Primary database
- **Liquibase 4.23.2**: Database migrations
- **SqlStreamStore 1.1.3**: Event sourcing
- **Outbox Pattern**: Eventual consistency

#### **3.1.4. Background Processing**
- **Quartz 3.14.0**: Job scheduling
- **Background Services**: Long-running tasks
- **Event Processing**: Async event handling

#### **3.1.5. Resilience & Monitoring**
- **Polly 8.2.0**: Resilience patterns
- **Serilog**: Structured logging
- **Health Checks**: System monitoring
- **Application Insights**: Performance monitoring

### **3.2. Solution Structure**

```
ModularMonolithDDD/
├── src/
│   ├── API/
│   │   └── ModularMonolithDDD.API/
│   │       ├── Controllers/
│   │       ├── Middlewares/
│   │       ├── Configuration/
│   │       └── Modules/
│   ├── BuildingBlocks/
│   │   ├── Domain/
│   │   │   ├── Entity.cs
│   │   │   ├── ValueObject.cs
│   │   │   ├── IAggregateRoot.cs
│   │   │   ├── IDomainEvent.cs
│   │   │   └── IBusinessRule.cs
│   │   ├── Application/
│   │   │   ├── IExecutionContextAccessor.cs
│   │   │   ├── IEmailSender.cs
│   │   │   ├── Events/
│   │   │   └── Queries/
│   │   ├── Infrastructure/
│   │   │   ├── EventBus/
│   │   │   ├── DomainEventsDispatching/
│   │   │   ├── Outbox/
│   │   │   └── Inbox/
│   │   └── Logging/
│   │       └── Serilogger.cs
│   ├── Modules/
│   │   ├── UserAccess/
│   │   │   ├── Domain/
│   │   │   ├── Application/
│   │   │   ├── Infrastructure/
│   │   │   └── IntegrationEvents/
│   │   ├── Customer/
│   │   ├── Product/
│   │   ├── Order/
│   │   ├── Payment/
│   │   ├── Inventory/
│   │   ├── Shipping/
│   │   ├── Notification/
│   │   └── Reporting/
│   └── Database/
│       ├── liquibase/
│       └── migrations/
├── modular-monolith-ddd-frontend-admin/
├── modular-monolith-ddd-frontend-portal/
└── docs/
```

### **3.3. Module Architecture (theo Grzybek pattern)**

```
ModuleName/
├── Domain/
│   ├── Entities/
│   │   ├── EntityName.cs
│   │   └── ValueObjects/
│   ├── Rules/
│   │   └── BusinessRuleName.cs
│   └── Events/
│       └── DomainEventName.cs
├── Application/
│   ├── Commands/
│   │   ├── CommandName/
│   │   │   ├── CommandNameCommand.cs
│   │   │   ├── CommandNameCommandHandler.cs
│   │   │   └── CommandNameCommandValidator.cs
│   ├── Queries/
│   │   ├── QueryName/
│   │   │   ├── QueryNameQuery.cs
│   │   │   ├── QueryNameQueryHandler.cs
│   │   │   └── QueryNameQueryValidator.cs
│   └── DTOs/
│       └── EntityNameDto.cs
├── Infrastructure/
│   ├── Repositories/
│   │   └── EntityNameRepository.cs
│   ├── EventHandlers/
│   │   └── DomainEventNameHandler.cs
│   └── ExternalServices/
│       └── ExternalServiceName.cs
└── IntegrationEvents/
    └── IntegrationEventName.cs
```

---

## **4. Database Architecture**

### **4.1. Database Design**

#### **4.1.1. Database per Module**
- **UserAccess**: Authentication & authorization
- **Customer**: Customer management
- **Product**: Product catalog
- **Order**: Order management
- **Payment**: Payment processing
- **Inventory**: Stock management
- **Shipping**: Shipping management
- **Notification**: Notification system
- **Reporting**: Analytics & reporting

#### **4.1.2. Shared Tables**
- **SystemConfiguration**: System settings
- **AuditLog**: Audit trail
- **EventStore**: Event sourcing

#### **4.1.3. Database Migration**
```yaml
# Liquibase Configuration
liquibase:
  image: liquibase/liquibase:4.23.2
  environment:
    LIQUIBASE_COMMAND_URL: jdbc:sqlserver://database:1433;databaseName=${SQLSERVER_DATABASE_NAME}
    LIQUIBASE_COMMAND_USERNAME: ${SQLSERVER_USER}
    LIQUIBASE_COMMAND_PASSWORD: ${SQLSERVER_PASSWORD}
  command: ["update", "--changelog-file=changelog.xml"]
  volumes:
    - ./modular-monolith-ddd/src/Database/liquibase:/liquibase
```

### **4.2. Event Sourcing**

#### **4.2.1. SqlStreamStore Configuration**
```csharp
// Event Store Configuration
services.AddSqlStreamStore(options =>
{
    options.ConnectionString = connectionString;
    options.Schema = "EventStore";
    options.CreateSchemaIfNotExists = true;
});
```

#### **4.2.2. Outbox Pattern**
```csharp
// Outbox Message
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
}
```

---

## **5. Identity & Access Management**

### **5.1. Keycloak Configuration**

#### **5.1.1. OAuth 2.0 + PKCE Flow**
```
┌─────────────────────────────────────────────────────────────┐
│                    OAuth 2.0 + PKCE Flow                    │
├─────────────────────────────────────────────────────────────┤
│  1. User Login → Keycloak Login Page                        │
│  2. Keycloak → Authorization Code + PKCE Challenge         │
│  3. Frontend → Exchange Code for Access Token              │
│  4. Frontend → Store Access Token + Refresh Token          │
│  5. API Calls → Bearer Token Authentication                │
│  6. Token Refresh → Automatic token renewal              │
└─────────────────────────────────────────────────────────────┘
```

#### **5.1.2. Keycloak Setup**
```yaml
keycloak:
  image: quay.io/keycloak/keycloak:latest
  environment:
    KEYCLOAK_ADMIN: ${KEYCLOAK_ADMIN}
    KEYCLOAK_ADMIN_PASSWORD: ${KEYCLOAK_ADMIN_PASSWORD}
    KC_DB: mssql
    KC_DB_URL: jdbc:sqlserver://database:1433;databaseName=${SQLSERVER_DATABASE_NAME}
    KC_DB_USERNAME: ${KEYCLOAK_DB_USERNAME}
    KC_DB_PASSWORD: ${KEYCLOAK_DB_PASSWORD}
  ports:
    - "${IDENTITY_HOST_PORT}:${IDENTITY_PORT}"
  command: start-dev  # Development
  # command: start     # Production
```

#### **5.1.3. Keycloak Clients**
- **admin-spa**: Admin Portal (Public Client)
- **portal-spa**: Customer Portal (Public Client)
- **api-gateway**: Backend API (Confidential Client)

### **5.2. Authentication Implementation**

#### **5.2.1. Frontend (React)**
```typescript
// Keycloak configuration
const keycloakConfig = {
  url: 'http://localhost:8082',
  realm: 'modular-monolith-ddd',
  clientId: 'admin-spa', // or 'portal-spa'
  onLoad: 'check-sso',
  pkceMethod: 'S256',
  responseMode: 'query',
  flow: 'standard'
};

// OAuth 2.0 + PKCE flow
const keycloak = new Keycloak(keycloakConfig);

// Login with PKCE
await keycloak.login({
  redirectUri: 'http://localhost:3000/callback',
  pkceMethod: 'S256'
});

// Token management
const accessToken = keycloak.token;
const refreshToken = keycloak.refreshToken;

// API calls with Bearer token
axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
```

#### **5.2.2. Backend (.NET 9)**
```csharp
// Program.cs - Keycloak OAuth 2.0 configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var keycloakAuthority = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY") 
        ?? "http://localhost:8082/realms/modular-monolith-ddd";
    var keycloakAudience = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE") 
        ?? "admin-spa";
        
    options.Authority = keycloakAuthority;
    options.Audience = keycloakAudience;
    options.RequireHttpsMetadata = false; // Development only
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});
```

---

## **6. Infrastructure & Deployment**

### **6.1. Docker Services Architecture**

```yaml
services:
  # Identity & Access Management - OAuth 2.0 + PKCE
  keycloak:
    image: quay.io/keycloak/keycloak:latest
    environment:
      KEYCLOAK_ADMIN: ${KEYCLOAK_ADMIN}
      KEYCLOAK_ADMIN_PASSWORD: ${KEYCLOAK_ADMIN_PASSWORD}
      KC_DB: mssql
      KC_DB_URL: jdbc:sqlserver://database:1433;databaseName=${SQLSERVER_DATABASE_NAME}
      KC_DB_USERNAME: ${KEYCLOAK_DB_USERNAME}
      KC_DB_PASSWORD: ${KEYCLOAK_DB_PASSWORD}
    ports:
      - "${IDENTITY_HOST_PORT}:${IDENTITY_PORT}"
    command: start-dev
    depends_on:
      database:
        condition: service_healthy

  # Database
  database:
    image: mcr.microsoft.com/mssql/server:2019-CU27-ubuntu-20.04
    environment:
      SA_PASSWORD: ${SA_PASSWORD}
      ACCEPT_EULA: "Y"
    ports:
      - "${SQLSERVER_HOST_PORT}:${SQLSERVER_PORT}"
    volumes:
      - vol_sqlserver:/var/opt/mssql
      - ./modular-monolith-ddd/src/Database:/docker-entrypoint-initdb.d
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d ModularMonolithDDD -Q \"SELECT 1\""]
      interval: 10s
      retries: 10
      timeout: 5s
      start_period: 20s

  # Database Migration
  liquibase:
    image: liquibase/liquibase:4.23.2
    environment:
      LIQUIBASE_COMMAND_URL: jdbc:sqlserver://database:1433;databaseName=${SQLSERVER_DATABASE_NAME}
      LIQUIBASE_COMMAND_USERNAME: ${SQLSERVER_USER}
      LIQUIBASE_COMMAND_PASSWORD: ${SQLSERVER_PASSWORD}
    command: ["update", "--changelog-file=changelog.xml"]
    volumes:
      - ./modular-monolith-ddd/src/Database/liquibase:/liquibase
    depends_on:
      database:
        condition: service_healthy

  # Backend API
  api:
    build:
      context: .
      dockerfile: modular-monolith-ddd/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
      - KEYCLOAK_AUTHORITY=${KEYCLOAK_AUTHORITY}
      - KEYCLOAK_AUDIENCE=${KEYCLOAK_AUDIENCE}
      - SQLSERVER_HOST=${SQLSERVER_HOST}
      - SQLSERVER_DATABASE_NAME=${SQLSERVER_DATABASE_NAME}
      - SQLSERVER_USER=${SQLSERVER_USER}
      - SQLSERVER_PASSWORD=${SQLSERVER_PASSWORD}
    ports:
      - "${API_HOST_PORT}:${API_PORT}"
    depends_on:
      database:
        condition: service_healthy
      keycloak:
        condition: service_started
      liquibase:
        condition: service_completed_successfully

  # Frontend Admin (react-dashboard-template)
  frontend-admin:
    build:
      context: ./modular-monolith-ddd-frontend-admin
      dockerfile: Dockerfile.fe
    environment:
      - VITE_API_URL=${VITE_API_URL}
      - VITE_KEYCLOAK_URL=${VITE_KEYCLOAK_URL}
      - VITE_KEYCLOAK_REALM=${VITE_KEYCLOAK_REALM}
      - VITE_KEYCLOAK_CLIENT_ID=${VITE_KEYCLOAK_CLIENT_ID}
      - VITE_REDIRECT_URI=${VITE_REDIRECT_URI}
    ports:
      - "${ADMIN_HOST_PORT}:${ADMIN_PORT}"
    volumes:
      - ./modular-monolith-ddd-frontend-admin/src:/app/src
      - /app/node_modules
    depends_on:
      - api

  # Frontend Portal (Custom React)
  frontend-portal:
    build:
      context: ./modular-monolith-ddd-frontend-portal
      dockerfile: Dockerfile.fe
    environment:
      - VITE_API_URL=${VITE_API_URL}
      - VITE_KEYCLOAK_URL=${VITE_KEYCLOAK_URL}
      - VITE_KEYCLOAK_REALM=${VITE_KEYCLOAK_REALM}
      - VITE_KEYCLOAK_CLIENT_ID=${VITE_KEYCLOAK_CLIENT_ID}
      - VITE_REDIRECT_URI=${VITE_REDIRECT_URI}
    ports:
      - "${PORTAL_HOST_PORT}:${PORTAL_PORT}"
    volumes:
      - ./modular-monolith-ddd-frontend-portal/src:/app/src
      - /app/node_modules
    depends_on:
      - api

volumes:
  vol_sqlserver:

networks:
  modular-monolith-network:
    driver: bridge
    name: modular-monolith-ddd-network
```

### **6.2. Environment Variables**

```bash
# Keycloak OAuth 2.0 + PKCE Configuration
KEYCLOAK_ADMIN=admin
KEYCLOAK_ADMIN_PASSWORD=admin123
KEYCLOAK_AUTHORITY=http://localhost:8082/realms/modular-monolith-ddd
KEYCLOAK_AUDIENCE=admin-spa
KEYCLOAK_DB_USERNAME=keycloak_user
KEYCLOAK_DB_PASSWORD=keycloak_password

# Frontend OAuth 2.0 + PKCE Configuration
VITE_KEYCLOAK_URL=http://localhost:8082
VITE_KEYCLOAK_REALM=modular-monolith-ddd
VITE_KEYCLOAK_CLIENT_ID=admin-spa  # or portal-spa
VITE_REDIRECT_URI=http://localhost:3000/callback

# Database Configuration
SQLSERVER_DATABASE_NAME=ModularMonolithDDD
SQLSERVER_USER=app_admin
SQLSERVER_PASSWORD=app_password
SA_PASSWORD=YourStrong@Passw0rd

# API Configuration
API_HOST_PORT=5000
API_PORT=80
IDENTITY_HOST_PORT=8082
IDENTITY_PORT=8080
ADMIN_HOST_PORT=3000
ADMIN_PORT=80
PORTAL_HOST_PORT=3001
PORTAL_PORT=80
```

### **6.3. Development vs Production**

#### **6.3.1. Development**
- **Docker Compose**: Local development
- **Hot Reload**: Frontend development
- **Debug Mode**: Backend debugging
- **Local Database**: SQL Server container

#### **6.3.2. Production**
- **Kubernetes**: Container orchestration
- **Azure/AWS**: Cloud deployment
- **Load Balancer**: Traffic distribution
- **Monitoring**: Application Insights

---

## **7. Security Architecture**

### **7.1. Authentication & Authorization**

#### **7.1.1. OAuth 2.0 + PKCE Benefits**
- **PKCE (Proof Key for Code Exchange)**: Bảo vệ authorization code
- **Authorization Code Flow**: Secure token exchange
- **JWT Bearer Tokens**: Stateless authentication
- **Refresh Tokens**: Automatic token renewal
- **Scope-based Authorization**: Fine-grained permissions
- **Cross-Origin Support**: CORS-friendly

#### **7.1.2. Security Measures**
- **HTTPS**: Bắt buộc cho tất cả communications
- **JWT Validation**: Token signature verification
- **Role-based Access**: Fine-grained permissions
- **Audit Logging**: Security event tracking
- **Rate Limiting**: API abuse prevention

### **7.2. Data Protection**

#### **7.2.1. Encryption**
- **Data at Rest**: AES-256 encryption
- **Data in Transit**: TLS 1.3
- **Sensitive Data**: PCI DSS compliance
- **Personal Data**: GDPR compliance

#### **7.2.2. Compliance**
- **PCI DSS**: Payment data protection
- **GDPR**: Personal data protection
- **SOC 2**: Security controls
- **ISO 27001**: Information security

---

## **8. Performance & Scalability**

### **8.1. Performance Optimization**

#### **8.1.1. Frontend**
- **Code Splitting**: Lazy loading
- **Bundle Optimization**: Tree shaking
- **Caching**: Browser caching
- **CDN**: Static asset delivery

#### **8.1.2. Backend**
- **Connection Pooling**: Database connections
- **Caching**: Redis caching
- **Async Processing**: Background jobs
- **Database Optimization**: Indexing

### **8.2. Scalability**

#### **8.2.1. Horizontal Scaling**
- **Load Balancer**: Traffic distribution
- **Database Scaling**: Read replicas
- **Event Processing**: Multiple consumers
- **Background Jobs**: Distributed processing

#### **8.2.2. Monitoring**
- **Application Insights**: Performance monitoring
- **Health Checks**: System health
- **Logging**: Structured logging
- **Alerting**: Proactive monitoring

---

## **9. Testing Strategy**

### **9.1. Testing Pyramid**

#### **9.1.1. Unit Tests**
- **Domain Logic**: Business rules
- **Application Services**: Command/Query handlers
- **Infrastructure**: Repository implementations
- **Frontend Components**: React components

#### **9.1.2. Integration Tests**
- **API Endpoints**: HTTP integration
- **Database**: Data persistence
- **External Services**: Third-party integrations
- **Event Processing**: Event handling

#### **9.1.3. End-to-End Tests**
- **User Workflows**: Complete user journeys
- **Cross-browser**: Browser compatibility
- **Performance**: Load testing
- **Security**: Penetration testing

### **9.2. Testing Tools**

#### **9.2.1. Backend Testing**
- **NUnit**: Unit testing framework
- **NSubstitute**: Mocking framework
- **FluentAssertions**: Assertion library
- **TestContainers**: Integration testing

#### **9.2.2. Frontend Testing**
- **Jest**: Testing framework
- **React Testing Library**: Component testing
- **Cypress**: E2E testing
- **Storybook**: Component documentation

---

## **10. DevOps & CI/CD**

### **10.1. Development Workflow**

#### **10.1.1. Git Workflow**
- **Feature Branches**: Feature development
- **Pull Requests**: Code review
- **Automated Testing**: CI pipeline
- **Deployment**: CD pipeline

#### **10.1.2. Code Quality**
- **ESLint**: JavaScript linting
- **Prettier**: Code formatting
- **TypeScript**: Type checking
- **SonarQube**: Code quality analysis

### **10.2. Deployment Pipeline**

#### **10.2.1. CI/CD Pipeline**
- **Build**: Docker image creation
- **Test**: Automated testing
- **Security**: Vulnerability scanning
- **Deploy**: Production deployment

#### **10.2.2. Environment Management**
- **Development**: Local development
- **Staging**: Pre-production testing
- **Production**: Live environment
- **Monitoring**: Continuous monitoring

---

## **11. Monitoring & Observability**

### **11.1. Logging**

#### **11.1.1. Structured Logging**
- **Serilog**: .NET logging
- **Log Levels**: Debug, Info, Warning, Error
- **Correlation IDs**: Request tracing
- **Log Aggregation**: Centralized logging

#### **11.1.2. Log Analysis**
- **ELK Stack**: Elasticsearch, Logstash, Kibana
- **Application Insights**: Azure monitoring
- **Splunk**: Log analysis
- **Grafana**: Visualization

### **11.2. Metrics & Monitoring**

#### **11.2.1. Application Metrics**
- **Performance**: Response times
- **Throughput**: Requests per second
- **Error Rates**: Error percentages
- **Resource Usage**: CPU, Memory, Disk

#### **11.2.2. Business Metrics**
- **User Activity**: User engagement
- **Transaction Volume**: Business metrics
- **Revenue**: Financial metrics
- **Conversion Rates**: Business KPIs

---

## **12. Conclusion**

### **12.1. Architecture Benefits**

#### **12.1.1. Modular Monolith**
- **Maintainability**: Easy to maintain
- **Scalability**: Independent scaling
- **Team Autonomy**: Independent development
- **Technology Diversity**: Different tech stacks

#### **12.1.2. Domain-Driven Design**
- **Business Alignment**: Domain-focused
- **Clean Architecture**: Separation of concerns
- **Testability**: Easy to test
- **Flexibility**: Easy to modify

### **12.2. Technology Benefits**

#### **12.2.1. Frontend**
- **React 19.1.1**: Latest features
- **TypeScript**: Type safety
- **Redux Saga**: Complex state management
- **Tailwind CSS**: Rapid styling

#### **12.2.2. Backend**
- **.NET 9**: Latest framework
- **CQRS**: Command/Query separation
- **Event Sourcing**: Audit trail
- **Keycloak**: Enterprise authentication

### **12.3. Next Steps**

1. **Implementation**: Start with core modules
2. **Testing**: Comprehensive test coverage
3. **Documentation**: API documentation
4. **Deployment**: Production deployment
5. **Monitoring**: Continuous monitoring

---

## **13. Appendices**

### **13.1. References**
- Business Analysis.md
- SRS.md
- Domain-Driven Design patterns
- .NET 9 documentation
- React 19.1.1 documentation
- Keycloak documentation
- Redux Saga documentation

### **13.2. Glossary**
- **Bounded Context**: Domain boundary trong DDD
- **Aggregate**: Domain entity cluster
- **Event Sourcing**: Event-driven data storage
- **CQRS**: Command Query Responsibility Segregation
- **OAuth 2.0**: Open Authorization 2.0
- **PKCE**: Proof Key for Code Exchange
- **JWT**: JSON Web Token

### **13.3. Change History**
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2024-01-19 | System Architect | Initial version |
