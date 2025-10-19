# E-commerce Lite — Sprint Planning (MVP for Job Hunting)

## **1. Project Overview**
- **Duration**: 8 weeks (2 tháng)
- **Goal**: MVP để showcase trong CV và tìm việc
- **Team**: Solo developer
- **Target**: Complete e-commerce system với modern tech stack

## **2. Technology Stack Showcase**
- **Backend**: .NET 9, ASP.NET Core, Entity Framework Core
- **Frontend**: React 19, TypeScript, Redux Toolkit
- **Architecture**: Modular Monolith, DDD, CQRS
- **Database**: SQL Server, Liquibase
- **Authentication**: Keycloak (OAuth 2.0)
- **Infrastructure**: Docker, Docker Compose
- **Testing**: Unit tests, Integration tests

## **3. Sprint 1: Foundation (2 tuần)**
### **Sprint Goal**: Setup authentication và admin layout

| Story ID | Story | Story Points | Tasks | Demo | **React Knowledge Required** | **Backend Knowledge Required** |
|----------|-------|--------------|-------|------|------------------------------|--------------------------------|
| F1-001 | Setup project structure | 3 | - Create solution<br/>- Setup Docker Compose<br/>- Configure Keycloak | Docker containers running | **None** (Backend only) | **Docker**: Containerization, Docker Compose<br/>**Keycloak**: OAuth 2.0 setup, Realm configuration<br/>**SQL Server**: Database setup, Connection strings |
| F1-002 | UserAccess module | 5 | - Domain entities<br/>- Application services<br/>- Infrastructure | Backend APIs working | **None** (Backend only) | **Controller**: API endpoints, HTTP methods<br/>**Model**: User entity, properties<br/>**Service**: Business logic, validation<br/>**DbContext**: Database operations, migrations |
| F1-003 | Admin layout | 5 | - Install react-dashboard-template<br/>- Setup routing<br/>- Configure Redux | Admin layout hiển thị | **React Router**: Route, Link, Outlet<br/>**Component**: JSX, Props, Children<br/>**Layout**: Header, Sidebar, Main content | **None** (Frontend only) |
| F1-003a | Header component | 2 | - Logo, user profile, notifications<br/>- Theme toggle, logout button | Header hiển thị đầy đủ | **useState**: Menu state, user info<br/>**Props**: User data, navigation props<br/>**Event Handling**: Logout, menu toggle | **None** (Frontend only) |
| F1-003b | Sidebar component | 2 | - Navigation menu, collapsible<br/>- Active menu highlighting | Sidebar navigation working | **useState**: Collapsed state<br/>**React Router**: Navigation links<br/>**Conditional Rendering**: Active menu items | **None** (Frontend only) |
| F1-003c | Main layout | 1 | - Layout composition<br/>- Responsive design | Complete layout structure | **Component Composition**: Header + Sidebar + Content<br/>**Props**: Children, layout props<br/>**CSS**: Flexbox, responsive | **None** (Frontend only) |
| F1-004 | Authentication | 5 | - Keycloak integration<br/>- Login/logout pages<br/>- Auth guards | Admin có thể đăng nhập | **useState**: Form state, loading state<br/>**useEffect**: Token check, redirect<br/>**Event Handling**: onSubmit, onClick<br/>**Conditional Rendering**: Show/hide based on auth | **JWT**: Token generation, validation<br/>**Middleware**: Authentication middleware<br/>**Keycloak**: OAuth 2.0 flow, token exchange |
| F1-005 | Dashboard skeleton | 2 | - Dashboard page<br/>- Navigation menu<br/>- Responsive layout | Complete admin experience | **useState**: Menu state, theme state<br/>**useEffect**: Load initial data<br/>**Props**: Passing data between components | **None** (Frontend only) |

**Total**: 20 story points

### **Sprint 1: Layout UI Features Breakdown**

#### **F1-003: Admin Layout (Main Feature)**
| Sub-Feature | Story Points | Tasks | React Knowledge | Demo |
|-------------|--------------|-------|-----------------|------|
| **F1-003a: Header Component** | 2 | - Logo, user profile<br/>- Notifications, theme toggle<br/>- Logout functionality | **useState**: Menu state, user info<br/>**Props**: User data, navigation<br/>**Event Handling**: Logout, menu toggle | Header hiển thị đầy đủ chức năng |
| **F1-003b: Sidebar Component** | 2 | - Navigation menu<br/>- Collapsible functionality<br/>- Active menu highlighting | **useState**: Collapsed state<br/>**React Router**: Navigation links<br/>**Conditional Rendering**: Active menu items | Sidebar navigation working |
| **F1-003c: Main Layout** | 1 | - Layout composition<br/>- Responsive design<br/>- Component structure | **Component Composition**: Header + Sidebar + Content<br/>**Props**: Children, layout props<br/>**CSS**: Flexbox, responsive | Complete layout structure |

#### **F1-005: Dashboard Skeleton (Layout Enhancement)**
| Sub-Feature | Story Points | Tasks | React Knowledge | Demo |
|-------------|--------------|-------|-----------------|------|
| **F1-005a: Dashboard Page** | 1 | - Dashboard content area<br/>- Placeholder widgets<br/>- Basic metrics display | **useState**: Dashboard data<br/>**useEffect**: Load initial data<br/>**Component**: Dashboard widgets | Dashboard page hiển thị |
| **F1-005b: Navigation Integration** | 1 | - Breadcrumb navigation<br/>- Page routing<br/>- Menu integration | **React Router**: useLocation, useParams<br/>**useState**: Breadcrumb state<br/>**Dynamic Routing**: Nested routes | Navigation system working |

### **React Learning Path for Sprint 1:**
1. **Week 1**: JSX, Component, Props, useState, Event Handling
2. **Week 2**: useEffect, React Router, Conditional Rendering

## **4. Sprint 2: Product Management (2 tuần)**
### **Sprint Goal**: Admin có thể quản lý sản phẩm

| Story ID | Story | Story Points | Tasks | Demo | **React Knowledge Required** | **Backend Knowledge Required** |
|----------|-------|--------------|-------|------|------------------------------|--------------------------------|
| F2-001 | Product domain model | 5 | - Product entities<br/>- Business rules<br/>- Domain events | Domain logic working | **None** (Backend only) | **Model**: Product entity, properties<br/>**Validation**: Data annotations, business rules<br/>**DbContext**: Entity relationships, migrations<br/>**Repository**: Data access pattern |
| F2-002 | Product APIs | 3 | - CRUD endpoints<br/>- Validation<br/>- Error handling | API endpoints working | **None** (Backend only) | **Controller**: CRUD endpoints, HTTP methods<br/>**Service**: Business logic, validation<br/>**DTO**: Data transfer objects<br/>**Error Handling**: Exception handling, status codes |
| F2-003 | Product list page | 5 | - Data table<br/>- Pagination<br/>- Search/filter | Admin xem danh sách sản phẩm | **useState**: List data, loading, error states<br/>**useEffect**: Fetch data on mount<br/>**Event Handling**: Search, filter, pagination<br/>**List Rendering**: .map(), key prop<br/>**Conditional Rendering**: Loading, empty states | **API Design**: RESTful endpoints, query parameters<br/>**Pagination**: Skip, take, total count<br/>**Filtering**: Search, category filter<br/>**Response Format**: JSON structure, status codes |
| F2-004 | Product form | 5 | - Add/edit forms<br/>- Image upload<br/>- Validation | Admin có thể thêm/sửa sản phẩm | **useState**: Form state, validation errors<br/>**useEffect**: Load product data for edit<br/>**Event Handling**: Form submission, input changes<br/>**Form Handling**: Controlled components<br/>**File Upload**: File input, preview | **File Upload**: IFormFile, file validation<br/>**Image Processing**: Resize, format conversion<br/>**Validation**: Model validation, custom validators<br/>**Database**: Update operations, transactions |
| F2-005 | Category management | 2 | - Category CRUD<br/>- Tree structure | Admin quản lý danh mục | **useState**: Category list, form state<br/>**useEffect**: Fetch categories<br/>**Event Handling**: CRUD operations<br/>**Component Reuse**: Reuse Product logic | **Controller**: Category CRUD endpoints<br/>**Model**: Category entity, relationships<br/>**Service**: Category business logic<br/>**Tree Structure**: Parent-child relationships |

**Total**: 20 story points

### **React Learning Path for Sprint 2:**
1. **Week 1**: useEffect (fetch API), List Rendering, Form Handling
2. **Week 2**: Controlled Components, File Upload, Error Handling

## **5. Sprint 3: Customer Portal (2 tuần)**
### **Sprint Goal**: Customer có thể duyệt sản phẩm và mua hàng

| Story ID | Story | Story Points | Tasks | Demo | **React Knowledge Required** | **Backend Knowledge Required** |
|----------|-------|--------------|-------|------|------------------------------|--------------------------------|
| F3-001 | Customer portal layout | 3 | - E-commerce layout<br/>- Header/footer<br/>- Navigation | Customer portal hiển thị | **React Router**: Route, Link, useNavigate<br/>**Component**: Layout components<br/>**Props**: Passing data between components | **None** (Frontend only) |
| F3-002 | Product catalog API | 2 | - Public product APIs<br/>- Search/filter<br/>- Pagination | Public APIs working | **None** (Backend only) | **Controller**: Public product endpoints<br/>**API Design**: RESTful API, query parameters<br/>**Pagination**: Skip, take, total count<br/>**Filtering**: Search, category, price range<br/>**Response Format**: JSON structure, status codes |
| F3-003 | Product listing page | 5 | - Grid layout<br/>- Product cards<br/>- Filters | Customer duyệt sản phẩm | **useState**: Product list, filters, pagination<br/>**useEffect**: Fetch products, handle filters<br/>**Event Handling**: Search, filter, pagination<br/>**List Rendering**: Grid layout, product cards<br/>**Component Composition**: ProductCard component | **API Integration**: Fetch data from backend<br/>**Query Parameters**: Search, filter, pagination<br/>**Error Handling**: API error states<br/>**Loading States**: API loading indicators |
| F3-004 | Product detail page | 5 | - Product details<br/>- Image gallery<br/>- Add to cart | Customer xem chi tiết sản phẩm | **useState**: Product data, selected image, quantity<br/>**useEffect**: Fetch product details<br/>**Event Handling**: Image selection, quantity change<br/>**Conditional Rendering**: Show/hide based on data<br/>**Props**: Receiving product ID from router | **API Integration**: Fetch single product<br/>**Route Parameters**: Product ID from URL<br/>**Error Handling**: 404, API errors<br/>**Data Validation**: Product existence check |
| F3-005 | Shopping cart | 5 | - Cart functionality<br/>- Add/remove items<br/>- Cart persistence | Customer có thể thêm vào giỏ | **useState**: Cart items, total, quantities<br/>**useEffect**: Load/save cart to localStorage<br/>**Event Handling**: Add/remove/update quantities<br/>**Local Storage**: Persist cart data<br/>**Component State**: Cart state management | **None** (Frontend only - Local Storage) |

**Total**: 20 story points

### **React Learning Path for Sprint 3:**
1. **Week 1**: Component Composition, Props drilling, Local Storage
2. **Week 2**: State Management patterns, Event handling patterns

## **6. Sprint 4: Order Processing (2 tuần)**
### **Sprint Goal**: Complete e-commerce flow

| Story ID | Story | Story Points | Tasks | Demo | **React Knowledge Required** | **Backend Knowledge Required** |
|----------|-------|--------------|-------|------|------------------------------|--------------------------------|
| F4-001 | Order domain model | 5 | - Order entities<br/>- Business rules<br/>- Order events | Order logic working | **None** (Backend only) | **Model**: Order entity, OrderItem entity<br/>**Relationships**: Order-OrderItem, Order-Customer<br/>**Validation**: Business rules, data validation<br/>**DbContext**: Entity relationships, migrations |
| F4-002 | Order APIs | 3 | - Create order<br/>- Order status<br/>- Order history | Order APIs working | **None** (Backend only) | **Controller**: Order CRUD endpoints<br/>**Service**: Order business logic, validation<br/>**DTO**: OrderDto, OrderItemDto<br/>**Error Handling**: Exception handling, status codes<br/>**Transactions**: Database transactions |
| F4-003 | Checkout flow | 8 | - Multi-step form<br/>- Shipping info<br/>- Order confirmation | Customer có thể đặt hàng | **useState**: Form data, current step, validation<br/>**useEffect**: Load cart data, validate form<br/>**Event Handling**: Step navigation, form submission<br/>**Multi-step Forms**: Step management<br/>**Form Validation**: Client-side validation<br/>**Context API**: Share cart data across components | **API Integration**: Submit order to backend<br/>**Data Validation**: Order validation, stock check<br/>**Error Handling**: API errors, validation errors<br/>**Response Handling**: Success/error responses |
| F4-004 | Order management | 4 | - Order list<br/>- Order details<br/>- Status updates | Admin quản lý đơn hàng | **useState**: Order list, filters, selected order<br/>**useEffect**: Fetch orders, handle filters<br/>**Event Handling**: Filter, search, status updates<br/>**List Rendering**: Order table, pagination<br/>**Modal Components**: Order details modal | **Controller**: Order management endpoints<br/>**Service**: Order status updates, filtering<br/>**API Design**: Admin order endpoints<br/>**Error Handling**: Order not found, validation errors |

**Total**: 20 story points

### **React Learning Path for Sprint 4:**
1. **Week 1**: Context API, Multi-step forms, Form validation
2. **Week 2**: Advanced state management, Modal patterns, Complex event handling

## **7. React Learning Summary**

### **7.1. React Core Concepts (Sprint 1-2)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **JSX** | 1 | Component structure | `<div>Hello {name}</div>` |
| **Component** | 1 | Reusable UI pieces | `function ProductCard({ product })` |
| **Props** | 1 | Data passing | `<ProductCard product={product} />` |
| **useState** | 1-4 | Local state management | `const [count, setCount] = useState(0)` |
| **useEffect** | 2-4 | Side effects, API calls | `useEffect(() => fetchData(), [])` |
| **Event Handling** | 1-4 | User interactions | `onClick={handleClick}` |
| **Conditional Rendering** | 1-4 | Show/hide based on state | `{isLoading && <Spinner />}` |
| **List Rendering** | 2-4 | Display arrays | `{items.map(item => <Item key={item.id} />)}` |

### **7.2. React Router (Sprint 1, 3)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **Route** | 1, 3 | Page navigation | `<Route path="/products" element={<Products />} />` |
| **Link** | 1, 3 | Navigation links | `<Link to="/products">Products</Link>` |
| **useNavigate** | 3 | Programmatic navigation | `const navigate = useNavigate()` |
| **Outlet** | 1 | Nested routes | `<Outlet />` |

### **7.3. Form Handling (Sprint 2, 4)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **Controlled Components** | 2, 4 | Form input control | `<input value={name} onChange={setName} />` |
| **Form Validation** | 2, 4 | Client-side validation | `if (!name) setError('Name required')` |
| **File Upload** | 2 | Image upload | `<input type="file" onChange={handleFile} />` |
| **Multi-step Forms** | 4 | Checkout flow | `const [step, setStep] = useState(1)` |

### **7.4. State Management (Sprint 3-4)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **Local Storage** | 3 | Persist cart data | `localStorage.setItem('cart', JSON.stringify(cart))` |
| **Context API** | 4 | Share state globally | `const CartContext = createContext()` |
| **Component Composition** | 3 | Reusable components | `<ProductCard><ProductImage /><ProductInfo /></ProductCard>` |

### **7.5. Advanced Patterns (Sprint 4)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **Modal Components** | 4 | Order details modal | `<Modal isOpen={showModal} onClose={closeModal} />` |
| **Error Handling** | 2-4 | API error states | `{error && <ErrorMessage error={error} />}` |
| **Loading States** | 2-4 | Loading indicators | `{isLoading && <Spinner />}` |

## **8. Backend Learning Summary**

### **8.1. Core Backend Concepts (Sprint 1-2)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **Controller** | 1-4 | API endpoints | `[HttpGet] public async Task<IActionResult> GetProducts()` |
| **Model** | 1-4 | Entity classes | `public class Product { public int Id { get; set; } }` |
| **Service** | 1-4 | Business logic | `public class ProductService { public async Task<Product> CreateAsync(Product product) }` |
| **DbContext** | 1-4 | Database operations | `public DbSet<Product> Products { get; set; }` |
| **DTO** | 2-4 | Data transfer | `public class ProductDto { public string Name { get; set; } }` |
| **Validation** | 2-4 | Data validation | `[Required] public string Name { get; set; }` |
| **Error Handling** | 2-4 | Exception handling | `try { } catch { return BadRequest(); }` |

### **8.2. API Design (Sprint 2-4)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **RESTful API** | 2-4 | HTTP methods | GET, POST, PUT, DELETE |
| **Query Parameters** | 2-4 | Filtering, pagination | `?page=1&size=10&search=keyword` |
| **Response Format** | 2-4 | JSON structure | `{ "data": [], "total": 100, "page": 1 }` |
| **Status Codes** | 2-4 | HTTP status | 200 OK, 400 BadRequest, 404 NotFound |
| **Pagination** | 2-4 | Skip, take, total | `Skip((page - 1) * size).Take(size)` |

### **8.3. File Handling (Sprint 2)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **File Upload** | 2 | IFormFile | `public async Task<IActionResult> UploadImage(IFormFile file)` |
| **Image Processing** | 2 | Resize, format | `Image.Resize(width, height)` |
| **File Validation** | 2 | File type, size | `if (file.Length > maxSize) return BadRequest()` |
| **File Storage** | 2 | Save to disk | `File.WriteAllBytes(path, bytes)` |

### **8.4. Database Operations (Sprint 1-4)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **Migrations** | 1-4 | Database schema | `Add-Migration InitialCreate` |
| **Relationships** | 2-4 | Entity relationships | `public virtual ICollection<OrderItem> OrderItems { get; set; }` |
| **Transactions** | 4 | Database transactions | `using var transaction = await _context.Database.BeginTransactionAsync()` |
| **Repository Pattern** | 2-4 | Data access | `public interface IProductRepository { Task<Product> GetByIdAsync(int id); }` |

### **8.5. Authentication & Security (Sprint 1, 4)**
| Concept | Sprint | Usage | Example |
|---------|--------|-------|---------|
| **JWT** | 1, 4 | Token generation | `var token = _jwtService.GenerateToken(user)` |
| **Middleware** | 1, 4 | Authentication | `app.UseAuthentication()` |
| **Authorization** | 1, 4 | Role-based access | `[Authorize(Roles = "Admin")]` |
| **Keycloak** | 1 | OAuth 2.0 | OAuth 2.0 flow, token exchange |

## **9. Learning Strategy (Cách học hiệu quả với Cursor AI)**

### **9.1. Phương pháp học "Developer thật trong thời đại AI"**
```
Bước 1: Tôi generate code → Bạn đọc hiểu từng dòng
Bước 2: Bạn hỏi tôi "Tại sao cần dòng này? Dependency injection là gì?"
Bước 3: Bạn code lại bằng tay (không copy)
Bước 4: Tôi review code của bạn và chỉ ra điểm cần cải thiện
```

### **9.2. Backend Learning Path**
| Sprint | Backend Focus | Cách học với tôi |
|--------|---------------|------------------|
| **Sprint 1** | **Controller + Model + Service** | Tôi generate CategoryController → Bạn học Controller pattern |
| **Sprint 2** | **CRUD + Validation + File Upload** | Tôi generate ProductController → Bạn học CRUD pattern |
| **Sprint 3** | **API Design + Pagination** | Tôi generate ProductCatalog API → Bạn học API design |
| **Sprint 4** | **Complex Business Logic** | Tôi generate OrderController → Bạn học Business logic |

### **9.3. React Learning Path**
| Sprint | React Focus | Cách học với tôi |
|--------|-------------|------------------|
| **Sprint 1** | **useState + useEffect + Router** | Tôi generate CategoryList → Bạn học React core |
| **Sprint 2** | **Form Handling + File Upload** | Tôi generate ProductForm → Bạn học Form patterns |
| **Sprint 3** | **Component Composition + Local Storage** | Tôi generate ProductCard → Bạn học Component patterns |
| **Sprint 4** | **Context API + Multi-step Forms** | Tôi generate CheckoutForm → Bạn học Advanced patterns |

### **9.4. Cách học từng kiến thức cụ thể**

#### **Backend: Controller Pattern**
```
Bạn hỏi: "Tạo CategoryController với CRUD operations"
Tôi generate: [Code + giải thích từng method]
Bạn học: [HttpGet], [HttpPost], async/await, IActionResult
Bạn code lại: Tự viết ProductController theo pattern
Tôi review: Chỉ ra điểm cần cải thiện
```

#### **React: useState + useEffect**
```
Bạn hỏi: "Tạo CategoryList component với fetch API"
Tôi generate: [Code + giải thích từng hook]
Bạn học: useState, useEffect, dependency array
Bạn code lại: Tự viết ProductList theo pattern
Tôi review: Chỉ ra điểm cần cải thiện
```

### **9.5. Checklist học hiệu quả**
- [ ] **Đọc hiểu code tôi generate** (không copy ngay)
- [ ] **Hỏi tôi câu hỏi** khi không hiểu
- [ ] **Code lại bằng tay** (không copy)
- [ ] **Test code** của bạn
- [ ] **Tôi review** và chỉ ra điểm cần cải thiện
- [ ] **Refactor** code theo gợi ý của tôi

## **10. Grzybek Design Patterns Analysis**

### **10.1. Design Patterns trong Grzybek Architecture**

| Pattern Category | Design Pattern | Mô tả | Áp dụng trong Grzybek |
|------------------|----------------|-------|----------------------|
| **Architectural** | **Clean Architecture** | Tách biệt Domain, Application, Infrastructure | Module structure (Domain/Application/Infrastructure) |
| **Architectural** | **Modular Monolith** | Modules độc lập trong cùng solution | 9 modules (UserAccess, Product, Order...) |
| **Architectural** | **Domain-Driven Design (DDD)** | Domain-centric design | Domain entities, aggregates, business rules |
| **Architectural** | **CQRS** | Tách Command và Query | Commands/Queries folders |
| **Architectural** | **Event-Driven Architecture** | Communication qua events | Domain events, integration events |
| **Creational** | **Factory Pattern** | Tạo objects phức tạp | Entity factories, service factories |
| **Creational** | **Builder Pattern** | Xây dựng objects từng bước | Aggregate builders |
| **Creational** | **Dependency Injection** | Invert dependencies | Constructor injection, service registration |
| **Structural** | **Repository Pattern** | Abstract data access | IProductRepository, IOrderRepository |
| **Structural** | **Unit of Work** | Transaction management | DbContext, transaction scope |
| **Structural** | **Specification Pattern** | Business rules encapsulation | BusinessRule classes |
| **Structural** | **Adapter Pattern** | Interface adaptation | External service adapters |
| **Structural** | **Facade Pattern** | Simplify complex subsystems | Application services |
| **Behavioral** | **Command Pattern** | Encapsulate requests | Command/CommandHandler |
| **Behavioral** | **Query Pattern** | Encapsulate queries | Query/QueryHandler |
| **Behavioral** | **Observer Pattern** | Event notification | Domain events, event handlers |
| **Behavioral** | **Mediator Pattern** | Loose coupling | MediatR library |
| **Behavioral** | **Strategy Pattern** | Algorithm selection | Payment strategies, validation strategies |
| **Behavioral** | **Template Method** | Algorithm skeleton | Base command/query handlers |
| **Behavioral** | **Chain of Responsibility** | Request processing chain | Validation pipeline |
| **Integration** | **Outbox Pattern** | Eventual consistency | Outbox table, event publishing |
| **Integration** | **Inbox Pattern** | Event processing | Inbox table, event handling |
| **Integration** | **Saga Pattern** | Distributed transactions | Order processing saga |
| **Integration** | **Event Sourcing** | Event-based storage | SqlStreamStore |
| **Integration** | **Background Jobs** | Async processing | Quartz scheduler |

### **10.2. Design Patterns theo Feature**

#### **Sprint 1: Foundation**
| Feature | Design Patterns | Mức độ phức tạp | Học được gì |
|---------|----------------|-----------------|-------------|
| **Setup project structure** | **Dependency Injection**, **Modular Monolith** | ⭐⭐ | DI container, module separation |
| **UserAccess module** | **Repository Pattern**, **CQRS**, **Domain Events** | ⭐⭐⭐⭐ | Data access, command/query separation |
| **Authentication** | **Strategy Pattern**, **Adapter Pattern** | ⭐⭐⭐ | OAuth strategies, external service integration |

#### **Sprint 2: Product Management**
| Feature | Design Patterns | Mức độ phức tạp | Học được gì |
|---------|----------------|-----------------|-------------|
| **Product domain model** | **Domain-Driven Design**, **Aggregate Pattern** | ⭐⭐⭐⭐ | Domain modeling, business rules |
| **Product APIs** | **CQRS**, **Command Pattern**, **Query Pattern** | ⭐⭐⭐ | Command/Query separation, request handling |
| **Product list page** | **Repository Pattern**, **Specification Pattern** | ⭐⭐⭐ | Data access, business rules |
| **Product form** | **Command Pattern**, **Validation Strategy** | ⭐⭐⭐ | Form handling, validation |
| **Category management** | **Tree Structure Pattern**, **Composite Pattern** | ⭐⭐⭐ | Hierarchical data, tree operations |

#### **Sprint 3: Customer Portal**
| Feature | Design Patterns | Mức độ phức tạp | Học được gì |
|---------|----------------|-----------------|-------------|
| **Customer portal layout** | **Facade Pattern**, **Template Method** | ⭐⭐ | UI composition, layout patterns |
| **Product catalog API** | **Repository Pattern**, **Query Pattern** | ⭐⭐⭐ | Data access, query optimization |
| **Product listing page** | **Observer Pattern**, **State Pattern** | ⭐⭐⭐ | UI state management, reactive updates |
| **Product detail page** | **Command Pattern**, **Strategy Pattern** | ⭐⭐⭐ | User actions, business logic |
| **Shopping cart** | **State Pattern**, **Observer Pattern** | ⭐⭐⭐ | Cart state management, UI updates |

#### **Sprint 4: Order Processing**
| Feature | Design Patterns | Mức độ phức tạp | Học được gì |
|---------|----------------|-----------------|-------------|
| **Order domain model** | **Aggregate Pattern**, **Domain Events** | ⭐⭐⭐⭐ | Complex domain modeling, event handling |
| **Order APIs** | **CQRS**, **Saga Pattern** | ⭐⭐⭐⭐ | Command/Query, distributed transactions |
| **Checkout flow** | **State Machine Pattern**, **Chain of Responsibility** | ⭐⭐⭐⭐ | Multi-step process, validation chain |
| **Order management** | **Command Pattern**, **Observer Pattern** | ⭐⭐⭐ | Admin operations, event notifications |

### **10.3. Learning Path theo Design Patterns**

#### **Beginner Level (Sprint 1-2)**
| Pattern | Khi nào học | Cách học với tôi |
|---------|-------------|------------------|
| **Dependency Injection** | Setup project | Tôi generate DI container → Bạn hiểu service registration |
| **Repository Pattern** | Product CRUD | Tôi generate IProductRepository → Bạn học data access |
| **Command Pattern** | Product APIs | Tôi generate CreateProductCommand → Bạn học request handling |
| **Query Pattern** | Product listing | Tôi generate GetProductsQuery → Bạn học data retrieval |

#### **Intermediate Level (Sprint 3)**
| Pattern | Khi nào học | Cách học với tôi |
|---------|-------------|------------------|
| **Observer Pattern** | Shopping cart | Tôi generate cart state management → Bạn học reactive updates |
| **State Pattern** | UI state | Tôi generate loading/error states → Bạn học state management |
| **Strategy Pattern** | Payment methods | Tôi generate payment strategies → Bạn học algorithm selection |
| **Facade Pattern** | Complex operations | Tôi generate service facades → Bạn học interface simplification |

#### **Advanced Level (Sprint 4)**
| Pattern | Khi nào học | Cách học với tôi |
|---------|-------------|------------------|
| **Saga Pattern** | Order processing | Tôi generate order saga → Bạn học distributed transactions |
| **Event Sourcing** | Domain events | Tôi generate event store → Bạn học event-based storage |
| **Outbox Pattern** | Event publishing | Tôi generate outbox implementation → Bạn học eventual consistency |
| **CQRS** | Complex queries | Tôi generate read/write separation → Bạn học performance optimization |

### **10.4. Pattern Complexity Matrix**

| Pattern | Beginner | Intermediate | Advanced | MVP Priority |
|---------|----------|-------------|----------|--------------|
| **Dependency Injection** | ✅ | ✅ | ✅ | **High** |
| **Repository Pattern** | ✅ | ✅ | ✅ | **High** |
| **Command Pattern** | ✅ | ✅ | ✅ | **High** |
| **Query Pattern** | ✅ | ✅ | ✅ | **High** |
| **Observer Pattern** | ❌ | ✅ | ✅ | **Medium** |
| **State Pattern** | ❌ | ✅ | ✅ | **Medium** |
| **Strategy Pattern** | ❌ | ✅ | ✅ | **Medium** |
| **Saga Pattern** | ❌ | ❌ | ✅ | **Low** |
| **Event Sourcing** | ❌ | ❌ | ✅ | **Low** |
| **CQRS** | ❌ | ❌ | ✅ | **Low** |

## **11. Team Capacity & Velocity**
- **Solo Developer**: 20 story points/sprint
- **Sprint Duration**: 2 tuần
- **Total Project**: 8 tuần (4 sprints)

## **8. Risk Assessment**
- **Low Risk**: Sprint 1, 2, 3 - Có thể hoàn thành đúng hạn
- **Medium Risk**: Sprint 4 - Cần focus vào core features
- **Mitigation**: Bỏ qua advanced features nếu thiếu thời gian

## **9. Dependencies**
- Sprint 2 depends on Sprint 1 (authentication)
- Sprint 3 depends on Sprint 2 (product management)
- Sprint 4 depends on Sprint 3 (shopping cart)

## **10. Demo Scenarios**
### **Sprint 1 Demo**: Admin đăng nhập và thấy dashboard
### **Sprint 2 Demo**: Admin quản lý sản phẩm (CRUD)
### **Sprint 3 Demo**: Customer duyệt sản phẩm và thêm vào giỏ
### **Sprint 4 Demo**: Complete e-commerce flow (browse → cart → checkout → order)

## **11. Success Criteria**
- ✅ Admin có thể đăng nhập và quản lý sản phẩm
- ✅ Customer có thể duyệt sản phẩm và đặt hàng
- ✅ Complete e-commerce flow hoạt động
- ✅ Code quality tốt với tests
- ✅ Documentation đầy đủ
- ✅ Docker setup hoàn chỉnh

## **12. Nice to Have (Nếu có thời gian)**
- Payment integration (VNPay mock)
- Basic inventory management
- Simple reporting
- Email notifications
- Mobile responsive optimization

## **13. CV Showcase Points**
- **Modern Tech Stack**: .NET 9, React 19, TypeScript
- **Architecture Patterns**: DDD, CQRS, Clean Architecture
- **Full-Stack Development**: Backend + Frontend
- **DevOps**: Docker, CI/CD
- **Testing**: Unit tests, Integration tests
- **Documentation**: Technical documentation
- **Git Workflow**: Feature branches, PR reviews
