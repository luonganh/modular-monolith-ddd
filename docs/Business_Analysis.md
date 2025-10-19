# E-commerce Lite — Business Analysis & DDD Mapping

## Mục tiêu
- Xác định domain logic thực sự, phân tích quy trình nghiệp vụ, và đảm bảo thiết kế hệ thống đúng theo DDD.
- Kết quả: Tài liệu BA/DDD tổng hợp gồm Actor/Use Cases, quy trình BC, Domain Model, Aggregates, Business Rules, Events & Workflows giữa các BC.

---

## Bước 1: Xác định các Actor & Use Cases (Ai làm gì)
### 1.1. Actors (Người/Ứng dụng tương tác)
| Tên | Mô tả | Chức năng chính |
|---|---|---|
| Administrator | Quản trị viên cấp cao | Quản lý toàn bộ hệ thống, phân quyền và cấu hình |
| Manager | Quản lý | Quản lý sản phẩm, đơn hàng, tồn kho, thanh toán, báo cáo |
| Staff | Nhân viên | Hỗ trợ vận chuyển, cập nhật trạng thái đơn, xem tồn kho, hỗ trợ khách, thanh toán, báo cáo |
| Customer | Người mua hàng | Đăng ký, đăng nhập, duyệt và xem sản phẩm, thêm sản phẩm vào giỏ hàng, đặt hàng và thanh toán linh hoạt theo phương thức đã chọn, theo dõi và hủy đơn hàng |
| Payment Gateway | Hệ thống thanh toán | Xử lý giao dịch, hoàn tiền |
| Background Jobs | Hệ thống tự động | Hủy đơn quá hạn, gửi thông báo, xử lý nghiệp vụ định kỳ |
### 1.2. Use Cases
| STT | Tên Use Case | Actor | Mô tả |
|---|---|---|---|
| **UC-** | Đăng nhập tài khoản quản trị | Administrator, Manager, Staff | Xác thực tài khoản quản trị |
| **UC-** | Đăng xuất tài khoản quản trị | Administrator, Manager, Staff | Đăng xuất tài khoản quản trị |
| **UC-** | Xem/Chỉnh sửa thông tin tài khoản quản trị | Administrator, Manager, Staff | Xem/Chỉnh sửa thông tin tài khoản quản trị |
| **UC-** | Đổi mật khẩu tài khoản quản trị | Administrator, Manager, Staff | Tài khoản quản trị tự đổi mật khẩu |
| **UC-** | Reset mật khẩu tài khoản cấp dưới | Administrator, Manager | Admin có thể reset mật khẩu Manager, Staff; Manager chỉ có thể reset mật khẩu Staff |

| **UC-** | Đăng ký tài khoản | Customer | Tạo tài khoản mới |
| **UC-** | Đăng nhập Customer | Customer | Xác thực người dùng (Web Portal) |
| **UC-** | Duyệt danh sách sản phẩm | Customer | Xem danh sách sản phẩm |
| **UC-** | Xem chi tiết sản phẩm | Customer | Xem thông tin chi tiết sản phẩm |
| **UC-** | Thêm sản phẩm vào giỏ hàng | Customer | Thêm sản phẩm vào giỏ |
| **UC-** | Đặt hàng COD | Customer | Tạo đơn hàng chưa thanh toán |
| **UC-** | Đặt hàng Online | Customer | Tạo đơn hàng và thanh toán online |
| **UC-** | Theo dõi đơn hàng | Customer | Xem trạng thái đơn hàng |
| **UC-** | Hủy đơn hàng | Customer | Hủy đơn hàng chưa xử lý |
| **UC-** | Tìm kiếm sản phẩm | Customer | Tìm kiếm sản phẩm theo từ khóa |
| **UC-** | Lọc sản phẩm | Customer | Lọc sản phẩm theo giá, danh mục... |
| **UC-** | Xem / Chỉnh sửa thông tin cá nhân | Customer | Thường có trong portal |
| **UC-** | Đổi mật khẩu | Customer | Khách hàng tự đổi mật khẩu |
| **UC-** | Quên mật khẩu | Customer | Reset mật khẩu qua email/SMS |
| **UC-** | Nạp tiền vào ví | Customer | Nạp tiền bằng thẻ ngân hàng / cổng thanh toán vào ví cá nhân |
| **UC-** | Thanh toán bằng ví | Customer | Dùng số dư ở ví điện tử để thanh toán đơn hàng |

| **UC-** | Quản lý người dùng | Administrator | Thêm, sửa, khóa hoặc xóa tài khoản người dùng trong hệ thống |
| **UC-** | Phân quyền người dùng | Administrator | Gán quyền và vai trò cho người dùng nội bộ |
| **UC-** | Cấu hình hệ thống | Administrator | Thiết lập các tham số cấu hình hệ thống |
| **UC-** | Quản lý cấu hình thanh toán | Administrator | Thiết lập VNPay, MoMo, Stripe... |
| **UC-** | Quản lý cấu hình email | Administrator | Thiết lập SMTP, template email |
| **UC-** | Quản lý cấu hình vận chuyển | Administrator | Thiết lập phí ship, khu vực giao hàng |

| **UC-** | Quản lý danh mục sản phẩm | Manager | CRUD danh mục sản phẩm |
| **UC-** | Quản lý sản phẩm | Manager | CRUD sản phẩm |
| **UC-** | Quản lý đơn hàng | Manager | Xác nhận/hủy đơn hàng |
| **UC-** | Quản lý tồn kho | Manager | Cập nhật số lượng tồn kho |
| **UC-** | Quản lý thanh toán | Manager | Theo dõi, xác nhận, hoặc yêu cầu hoàn tiền |
| **UC-** | Tạo và xem báo cáo | Manager | Tạo và xem báo cáo |
| **UC-** | Xử lý hoàn tiền | Manager | Thực hiện hoàn tiền qua cổng thanh toán hoặc ghi nhận thủ công (COD) |
| **UC-** | Quản lý khách hàng | Manager | Xem thông tin khách hàng, lịch sử mua hàng |
| **UC-** | Quản lý nhân viên | Manager | CRUD nhân viên (Staff) |
| **UC-** | Theo dõi & xử lý giao dịch ví | Manager | Theo dõi giao dịch ví, xử lý khiếu nại |

| **UC-** | Hỗ trợ vận chuyển | Staff | Cập nhật trạng thái giao hàng |
| **UC-** | Cập nhật trạng thái đơn | Staff | Thay đổi trạng thái đơn hàng |
| **UC-** | Xem tồn kho | Staff | Kiểm tra số lượng tồn kho |
| **UC-** | Chỉnh sửa tồn kho | Staff | Chỉnh sửa số lượng tồn kho |
| **UC-** | Hỗ trợ khách hàng | Staff | Trả lời câu hỏi và hỗ trợ khách |
| **UC-** | Xem thanh toán | Staff | Kiểm tra trạng thái thanh toán, chỉ được xem, ko sửa hoặc hoàn tiền |
| **UC-** | Xem báo cáo | Staff | Xem báo cáo hiệu suất |
| **UC-** | Xử lý khiếu nại | Staff | Giải quyết khiếu nại của khách |

| **UC-** | Xử lý giao dịch | Payment Gateway | Xử lý thanh toán |
| **UC-** | Hoàn tiền giao dịch | Payment Gateway | Xử lý hoàn tiền |
| **UC-** | Gửi thông báo kết quả | Payment Gateway | Gửi webhook về kết quả thanh toán |

| **UC-** | Hủy đơn quá hạn | Background Jobs | Tự động hủy đơn chưa thanh toán |
| **UC-** | Gửi thông báo cho Customer | Background Jobs | Gửi Email/SMS thông báo đơn hàng, thanh toán giao hàng |
| **UC-** | Xử lý nghiệp vụ định kỳ | Background Jobs | Thực hiện các tác vụ định kỳ (clear cache, sync payment, v.v...) |
| **UC-** | Kiểm tra sức khỏe hệ thống | Background Jobs | Health check và monitoring |
| **UC-** | Dọn dẹp dữ liệu cũ | Background Jobs | Xóa logs, cache cũ |

### **1.3. User Stories & Acceptance Criteria**

#### **1.3.1. Admin User Stories**
| User Story | Acceptance Criteria |
|---|---|
| **AS-001**: Là Admin, tôi muốn đăng nhập vào hệ thống để quản lý toàn bộ hệ thống | - Có thể đăng nhập với username/email và password<br/>- Sau khi đăng nhập, hiển thị dashboard với tổng quan hệ thống<br/>- Có thể truy cập tất cả modules (Products, Orders, Customers, etc.) |
| **AS-002**: Là Admin, tôi muốn quản lý sản phẩm để cập nhật catalog | - Có thể thêm/sửa/xóa sản phẩm<br/>- Có thể upload hình ảnh sản phẩm<br/>- Có thể quản lý danh mục sản phẩm<br/>- Có thể set giá và mô tả sản phẩm |
| **AS-003**: Là Admin, tôi muốn xem báo cáo để theo dõi hiệu suất kinh doanh | - Có thể xem báo cáo doanh thu theo ngày/tháng/năm<br/>- Có thể xem báo cáo sản phẩm bán chạy<br/>- Có thể export báo cáo ra Excel/PDF<br/>- Có thể tạo dashboard tùy chỉnh |

#### **1.3.2. Manager User Stories**
| User Story | Acceptance Criteria |
|---|---|
| **MS-001**: Là Manager, tôi muốn quản lý đơn hàng để xử lý orders | - Có thể xem danh sách đơn hàng với filter và search<br/>- Có thể xem chi tiết đơn hàng<br/>- Có thể cập nhật trạng thái đơn hàng<br/>- Có thể xử lý hoàn tiền |
| **MS-002**: Là Manager, tôi muốn quản lý tồn kho để kiểm soát inventory | - Có thể xem tổng quan tồn kho<br/>- Có thể cập nhật số lượng tồn kho<br/>- Có thể nhận cảnh báo khi hết hàng<br/>- Có thể xem lịch sử thay đổi tồn kho |

#### **1.3.3. Staff User Stories**
| User Story | Acceptance Criteria |
|---|---|
| **ST-001**: Là Staff, tôi muốn cập nhật trạng thái giao hàng để thông báo cho khách | - Có thể xem danh sách đơn hàng cần giao<br/>- Có thể cập nhật trạng thái giao hàng<br/>- Có thể nhập tracking number<br/>- Có thể gửi thông báo cho khách hàng |
| **ST-002**: Là Staff, tôi muốn hỗ trợ khách hàng để giải quyết khiếu nại | - Có thể xem thông tin khách hàng<br/>- Có thể xem lịch sử đơn hàng của khách<br/>- Có thể gửi tin nhắn hỗ trợ<br/>- Có thể cập nhật trạng thái khiếu nại |

#### **1.3.4. Customer User Stories**
| User Story | Acceptance Criteria |
|---|---|
| **CS-001**: Là Customer, tôi muốn duyệt sản phẩm để tìm hàng cần mua | - Có thể xem danh sách sản phẩm với hình ảnh và giá<br/>- Có thể tìm kiếm sản phẩm theo tên<br/>- Có thể lọc sản phẩm theo danh mục và giá<br/>- Có thể xem chi tiết sản phẩm |
| **CS-002**: Là Customer, tôi muốn đặt hàng để mua sản phẩm | - Có thể thêm sản phẩm vào giỏ hàng<br/>- Có thể xem giỏ hàng và chỉnh sửa số lượng<br/>- Có thể chọn phương thức thanh toán<br/>- Có thể nhập thông tin giao hàng |
| **CS-003**: Là Customer, tôi muốn theo dõi đơn hàng để biết trạng thái | - Có thể xem danh sách đơn hàng của mình<br/>- Có thể xem chi tiết đơn hàng<br/>- Có thể theo dõi trạng thái giao hàng<br/>- Có thể nhận thông báo cập nhật |

### **1.4. User Journey Maps**

#### **1.4.1. Admin User Journey**
```
Login → Dashboard → Product Management → Order Management → Reports
  ↓         ↓              ↓                    ↓              ↓
Auth    Overview      Add/Edit Products    View Orders    Generate Reports
  ↓         ↓              ↓                    ↓              ↓
Success  Metrics      Publish Products     Update Status   Export Data
```

#### **1.4.2. Customer User Journey**
```
Browse → Search → Product Detail → Add to Cart → Checkout → Payment → Confirmation
  ↓        ↓           ↓              ↓            ↓          ↓          ↓
Catalog  Filter     View Details    Review Cart   Shipping   Payment   Order Success
  ↓        ↓           ↓              ↓            ↓          ↓          ↓
Products  Results    Add to Cart    Proceed      Payment    Success   Track Order
```

### **1.5. Bounded Contexts sơ bộ

## Bước 2: Phân tích quy trình nghiệp vụ của từng Bounded Context (Chi tiết từng context, bao gồm entities)
### 2.1. CustomerContext
- Actors: Customer
- Use Cases: Đăng ký, đăng nhập
- **Domain Entities**: Customer, User, Role

### 2.2. ProductContext  
- Actors: Customer, Admin
- Use Cases: Duyệt sản phẩm, quản lý sản phẩm
- **Domain Entities**: Product, Category, SKU

### 2.3. OrderContext
- Actors: Customer, Admin
- Use Cases: Đặt hàng, theo dõi đơn, hủy đơn
- **Domain Entities**: Order, OrderItem, ShoppingCart

## Bước 3: Xác định Domain Model, Aggregates, Business Rules (Thiết kế aggregates và business rules)

### **Mục tiêu:**
- Xác định Domain Model cho từng Bounded Context.
- Xác định Aggregates & Aggregate Roots để đảm bảo tính toàn vẹn dữ liệu.
- Ràng buộc các Business Rules trong phạm vi Domain Model.

### **1. Tổng quan về cách xác định Domain Model & Aggregates**

**Domain Model:** Là tập hợp các thực thể (Entities), Value Objects, Aggregates thể hiện nghiệp vụ của hệ thống.

**Aggregate:** Là một nhóm thực thể có mối quan hệ chặt chẽ với nhau, được quản lý như một đơn vị duy nhất.

**Aggregate Root:** Là thực thể chính trong một Aggregate, chịu trách nhiệm duy trì tính toàn vẹn của Aggregate.

**Nguyên tắc thiết kế Aggregates:**
- Một Aggregate phải đảm bảo Consistency trong phạm vi của nó.
- Mọi thay đổi bên trong Aggregate chỉ được thực hiện thông qua Aggregate Root.
- Aggregate không nên quá lớn để tránh chặn hiệu suất & tính mở rộng.
- Dữ liệu được truy xuất bằng Read Models (CQRS) thay vì truy vấn trực tiếp trên Aggregate.

### **2. Xác định Domain Model & Aggregates cho từng Bounded Context**

#### **2.1. UserAccessContext**

**Domain Model:**
- **User (Aggregate Root)**
  - UserId (GUID)
  - Username
  - Email
  - PasswordHash
  - FirstName, LastName
  - Role (Admin, Manager, Staff)
  - IsActive (Xác định tài khoản có bị khóa hay không)
  - DateCreated, LastLoginAt
  - **Methods:**
    - Register(username, email, password, firstName, lastName, role)
    - Login(username, password)
    - UpdateProfile(firstName, lastName)
    - ChangePassword(oldPassword, newPassword)
    - DeactivateAccount()
    - AssignRole(role)

- **UserSession (Entity)**
  - SessionId (GUID)
  - UserId
  - Token
  - ExpiresAt
  - IsActive

- **SystemConfiguration (Entity)**
  - ConfigKey
  - ConfigValue
  - Category
  - IsEncrypted

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| User | UserSession, SystemConfiguration | **BR-01:** Username phải duy nhất trong hệ thống.<br/>**BR-02:** Email phải duy nhất trong hệ thống.<br/>**BR-03:** Mật khẩu phải đáp ứng policy (tối thiểu 8 ký tự, có chữ hoa, số, ký tự đặc biệt).<br/>**BR-04:** Tài khoản phải kích hoạt mới có thể đăng nhập.<br/>**BR-05:** Admin có thể khóa/bỏ khóa tài khoản Manager và Staff.<br/>**BR-06:** Manager chỉ có thể quản lý Staff, không thể quản lý Admin.<br/>**BR-07:** Sau 5 lần đăng nhập sai, tài khoản bị khóa 30 phút.<br/>**BR-08:** Session tự động expire sau 8 giờ không hoạt động. |

#### **2.2. CustomerContext**

**Domain Model:**
- **Customer (Aggregate Root)**
  - CustomerId (GUID)
  - Email
  - PasswordHash
  - FirstName, LastName
  - PhoneNumber
  - IsActive (Xác định tài khoản có bị khóa hay không)
  - IsEmailVerified
  - DateCreated, LastLoginAt
  - **Methods:**
    - Register(email, password, firstName, lastName, phoneNumber)
    - Login(email, password)
    - UpdateProfile(firstName, lastName, phoneNumber)
    - ChangePassword(oldPassword, newPassword)
    - VerifyEmail(verificationCode)
    - DeactivateAccount()

- **CustomerAddress (Entity)**
  - AddressId (GUID)
  - CustomerId
  - Street, City, State, PostalCode, Country
  - IsDefault
  - IsActive

- **CustomerPreference (Entity)**
  - PreferenceId (GUID)
  - CustomerId
  - Key, Value
  - Category

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Customer | CustomerAddress, CustomerPreference | **BR-01:** Email phải duy nhất trong hệ thống.<br/>**BR-02:** Số điện thoại phải đúng định dạng Việt Nam.<br/>**BR-03:** Tài khoản phải verify email trước khi có thể đặt hàng.<br/>**BR-04:** Customer có thể có nhiều địa chỉ, chỉ 1 địa chỉ mặc định.<br/>**BR-05:** Customer có thể xóa dữ liệu cá nhân theo quy định GDPR.<br/>**BR-06:** Sau 3 lần đăng nhập sai, tài khoản bị khóa 15 phút.<br/>**BR-07:** Mật khẩu phải đáp ứng policy tương tự UserAccess. |

#### **2.3. ProductContext**

**Domain Model:**
- **Product (Aggregate Root)**
  - ProductId (GUID)
  - Name
  - Description
  - SKU (Mã sản phẩm)
  - Price (Giá bán)
  - CategoryId
  - Status (Active, Inactive, Archived)
  - CreatedAt, PublishedAt
  - **Methods:**
    - CreateProduct(name, description, price, categoryId, sku)
    - UpdateProduct(name, description, price)
    - PublishProduct()
    - ArchiveProduct()
    - AddProductImage(imageUrl, isMain)

- **Category (Entity)**
  - CategoryId (GUID)
  - Name
  - Description
  - ParentId (Danh mục cha)
  - IsActive
  - SortOrder

- **ProductImage (Entity)**
  - ImageId (GUID)
  - ProductId
  - ImageUrl
  - IsMain (Hình ảnh chính)
  - SortOrder

- **ProductAttribute (Entity)**
  - AttributeId (GUID)
  - ProductId
  - Name, Value
  - Type (Text, Number, Color, Size)

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Product | Category, ProductImage, ProductAttribute | **BR-01:** SKU phải duy nhất trong hệ thống.<br/>**BR-02:** Tên sản phẩm không được trống và tối đa 200 ký tự.<br/>**BR-03:** Giá sản phẩm phải lớn hơn 0.<br/>**BR-04:** Sản phẩm phải có ít nhất 1 hình ảnh mới được publish.<br/>**BR-05:** Không thể xóa sản phẩm đã có đơn hàng.<br/>**BR-06:** Danh mục con không thể là parent của danh mục cha.<br/>**BR-07:** Chỉ sản phẩm Active mới hiển thị cho Customer. |

#### **2.4. OrderContext**

**Domain Model:**
- **Order (Aggregate Root)**
  - OrderId (GUID)
  - OrderNumber (Mã đơn hàng)
  - CustomerId
  - TotalAmount (Tổng tiền)
  - ShippingCost (Phí vận chuyển)
  - TaxAmount (Thuế)
  - OrderStatus (Pending, Confirmed, Processing, Shipped, Delivered, Cancelled)
  - PaymentStatus (Pending, Paid, Failed, Refunded)
  - CreatedAt, CompletedAt
  - **Methods:**
    - CreateOrder(customerId, orderItems, shippingAddress)
    - ConfirmOrder()
    - CancelOrder()
    - UpdateOrderStatus(status)
    - AddOrderNote(note)

- **OrderItem (Entity)**
  - OrderItemId (GUID)
  - OrderId
  - ProductId
  - Quantity
  - UnitPrice
  - TotalPrice

- **ShoppingCart (Aggregate Root)**
  - CartId (GUID)
  - CustomerId
  - CreatedAt, LastModifiedAt
  - ExpiresAt
  - **Methods:**
    - AddItem(productId, quantity)
    - RemoveItem(productId)
    - UpdateQuantity(productId, quantity)
    - ClearCart()

- **CartItem (Entity)**
  - CartItemId (GUID)
  - CartId
  - ProductId
  - Quantity
  - AddedAt

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Order | OrderItem | **BR-01:** Đơn hàng phải có ít nhất một sản phẩm.<br/>**BR-02:** Tổng tiền đơn hàng phải lớn hơn 0.<br/>**BR-03:** Chỉ có thể hủy đơn hàng khi status = Pending hoặc Confirmed.<br/>**BR-04:** Không thể hủy đơn hàng đã giao hàng.<br/>**BR-05:** Chỉ Admin/Manager có thể xác nhận đơn hàng.<br/>**BR-06:** Đơn hàng tự động hủy sau 30 phút nếu chưa thanh toán. |
| ShoppingCart | CartItem | **BR-01:** Giỏ hàng tự động expire sau 30 ngày không hoạt động.<br/>**BR-02:** Số lượng sản phẩm trong giỏ phải lớn hơn 0.<br/>**BR-03:** Không thể thêm sản phẩm đã hết hàng vào giỏ.<br/>**BR-04:** Mỗi Customer chỉ có 1 giỏ hàng active. |

#### **2.5. PaymentContext**

**Domain Model:**
- **PaymentTransaction (Aggregate Root)**
  - TransactionId (GUID)
  - OrderId
  - CustomerId
  - Amount (Số tiền)
  - PaymentMethod (VNPay, MoMo, COD, Wallet)
  - Status (Pending, Success, Failed, Refunded)
  - GatewayTransactionId
  - CreatedAt, ProcessedAt
  - **Methods:**
    - ProcessPayment(orderId, amount, paymentMethod)
    - RefundPayment(reason)
    - UpdatePaymentStatus(status, gatewayResponse)

- **Wallet (Aggregate Root)**
  - WalletId (GUID)
  - CustomerId
  - Balance (Số dư)
  - IsActive
  - CreatedAt
  - **Methods:**
    - TopUp(amount, paymentMethod)
    - Withdraw(amount)
    - TransferToOrder(amount, orderId)

- **WalletTransaction (Entity)**
  - TransactionId (GUID)
  - WalletId
  - Type (TopUp, Withdraw, Payment, Refund)
  - Amount
  - Description
  - CreatedAt

- **PaymentConfiguration (Entity)**
  - ConfigId (GUID)
  - GatewayName (VNPay, MoMo, Stripe)
  - ConfigKey, ConfigValue
  - IsActive

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| PaymentTransaction | WalletTransaction | **BR-01:** Chỉ xử lý thanh toán nếu đơn hàng hợp lệ và chưa thanh toán.<br/>**BR-02:** Không thể hoàn tiền nếu giao dịch bị thất bại.<br/>**BR-03:** Không thể hoàn tiền nhiều hơn số tiền đã thanh toán.<br/>**BR-04:** Giao dịch COD không cần xử lý thanh toán trước.<br/>**BR-05:** Giao dịch online phải có response từ gateway mới được coi là thành công.<br/>**BR-06:** Hoàn tiền phải được Admin/Manager phê duyệt. |
| Wallet | WalletTransaction | **BR-01:** Số dư ví không được âm.<br/>**BR-02:** Nạp tiền phải qua cổng thanh toán hợp lệ.<br/>**BR-03:** Rút tiền phải có số dư đủ.<br/>**BR-04:** Mỗi Customer chỉ có 1 ví điện tử.<br/>**BR-05:** Giao dịch ví phải được log đầy đủ để audit. |

#### **2.6. InventoryContext**

**Domain Model:**
- **Inventory (Aggregate Root)**
  - InventoryId (GUID)
  - ProductId
  - StockQuantity (Số lượng tồn kho)
  - ReservedQuantity (Số lượng đã đặt chỗ)
  - AvailableQuantity (Số lượng có thể bán)
  - MinStockLevel (Mức tồn kho tối thiểu)
  - MaxStockLevel (Mức tồn kho tối đa)
  - LastUpdatedAt
  - **Methods:**
    - IncreaseStock(quantity, reason)
    - DecreaseStock(quantity, reason)
    - ReserveStock(quantity, orderId)
    - ReleaseReservation(orderId)
    - AdjustStock(quantity, reason)

- **StockMovement (Entity)**
  - MovementId (GUID)
  - InventoryId
  - Type (In, Out, Adjustment, Reservation, Release)
  - Quantity
  - Reason
  - ReferenceId (OrderId, etc.)
  - CreatedAt

- **StockAlert (Entity)**
  - AlertId (GUID)
  - ProductId
  - AlertType (LowStock, OutOfStock, OverStock)
  - Threshold
  - IsActive
  - CreatedAt

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Inventory | StockMovement, StockAlert | **BR-01:** Không thể giảm số lượng tồn kho xuống dưới 0.<br/>**BR-02:** Phải kiểm tra tồn kho trước khi cho phép đặt hàng.<br/>**BR-03:** Khi đơn hàng bị hủy, số lượng đã reserve phải được hoàn lại.<br/>**BR-04:** Khi đơn hàng được xác nhận, reserved quantity chuyển thành actual decrease.<br/>**BR-05:** Hệ thống tự động tạo alert khi stock xuống dưới min level.<br/>**BR-06:** Mọi thay đổi tồn kho phải được log với lý do rõ ràng. |

#### **2.7. ShippingContext**

**Domain Model:**
- **Shipment (Aggregate Root)**
  - ShipmentId (GUID)
  - OrderId
  - ShippingMethodId
  - TrackingNumber
  - Status (Preparing, Shipped, InTransit, Delivered, Failed)
  - ShippingCost
  - EstimatedDeliveryDate
  - ActualDeliveryDate
  - CreatedAt
  - **Methods:**
    - CreateShipment(orderId, shippingMethodId)
    - UpdateTrackingNumber(trackingNumber)
    - UpdateStatus(status)
    - MarkAsDelivered()

- **ShippingMethod (Entity)**
  - MethodId (GUID)
  - Name (Standard, Express, Overnight)
  - Description
  - BaseCost
  - IsActive
  - EstimatedDays

- **ShippingZone (Entity)**
  - ZoneId (GUID)
  - Name
  - Countries
  - BaseCost
  - IsActive

- **ShippingRate (Entity)**
  - RateId (GUID)
  - ShippingMethodId
  - ZoneId
  - WeightFrom, WeightTo
  - Cost

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Shipment | ShippingMethod, ShippingZone, ShippingRate | **BR-01:** Mỗi đơn hàng chỉ có thể có 1 shipment.<br/>**BR-02:** Phí vận chuyển được tính dựa trên trọng lượng và khu vực.<br/>**BR-03:** Chỉ có thể tạo shipment khi đơn hàng đã được xác nhận.<br/>**BR-04:** Tracking number phải unique trong hệ thống.<br/>**BR-05:** Status shipment phải tuân theo workflow: Preparing → Shipped → InTransit → Delivered.<br/>**BR-06:** Nếu delivery thất bại, có thể retry tối đa 3 lần. |

#### **2.8. NotificationContext**

**Domain Model:**
- **Notification (Aggregate Root)**
  - NotificationId (GUID)
  - RecipientId (CustomerId hoặc UserId)
  - Type (Email, SMS, Push, InApp)
  - Subject
  - Content
  - Status (Pending, Sent, Failed, Delivered)
  - Priority (Low, Normal, High, Urgent)
  - ScheduledAt, SentAt
  - **Methods:**
    - CreateNotification(recipientId, type, subject, content)
    - SendNotification()
    - MarkAsDelivered()
    - RetryFailedNotification()

- **NotificationTemplate (Entity)**
  - TemplateId (GUID)
  - Name
  - Type (Email, SMS, Push)
  - Subject
  - Content
  - Variables (Danh sách biến có thể thay thế)
  - IsActive

- **NotificationQueue (Entity)**
  - QueueId (GUID)
  - NotificationId
  - Priority
  - ScheduledAt
  - RetryCount
  - MaxRetries

- **EmailConfiguration (Entity)**
  - ConfigId (GUID)
  - SMTPHost, SMTPPort
  - Username, Password
  - FromEmail, FromName
  - IsActive

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Notification | NotificationTemplate, NotificationQueue, EmailConfiguration | **BR-01:** Email phải có địa chỉ recipient hợp lệ.<br/>**BR-02:** SMS phải có số điện thoại đúng định dạng.<br/>**BR-03:** Notification có priority cao được xử lý trước.<br/>**BR-04:** Nếu gửi thất bại, retry tối đa 3 lần với interval tăng dần.<br/>**BR-05:** Template phải có tất cả biến cần thiết mới được sử dụng.<br/>**BR-06:** Notification quan trọng phải có backup channel (SMS nếu email fail). |

#### **2.9. ReportingContext**

**Domain Model:**
- **Report (Aggregate Root)**
  - ReportId (GUID)
  - Name
  - Description
  - ReportType (Sales, Inventory, Customer, Financial)
  - Parameters (JSON chứa các tham số)
  - Status (Draft, Published, Archived)
  - CreatedBy
  - CreatedAt, LastRunAt
  - **Methods:**
    - CreateReport(name, description, reportType, parameters)
    - GenerateReport()
    - ScheduleReport(schedule)
    - ArchiveReport()

- **ReportTemplate (Entity)**
  - TemplateId (GUID)
  - Name
  - ReportType
  - Query (SQL query hoặc data source)
  - Parameters
  - IsActive

- **Dashboard (Aggregate Root)**
  - DashboardId (GUID)
  - Name
  - Description
  - CreatedBy
  - IsPublic
  - CreatedAt
  - **Methods:**
    - CreateDashboard(name, description)
    - AddWidget(widgetId)
    - RemoveWidget(widgetId)
    - ShareDashboard(userIds)

- **Widget (Entity)**
  - WidgetId (GUID)
  - DashboardId
  - Type (Chart, Table, KPI, Graph)
  - Title
  - DataSource
  - Position (X, Y, Width, Height)

**Aggregates & Business Rules:**

| Aggregate Root | Entities / Value Objects | Business Rules |
|---|---|---|
| Report | ReportTemplate | **BR-01:** Chỉ Manager/Admin mới có thể tạo báo cáo.<br/>**BR-02:** Báo cáo phải có tên và mô tả rõ ràng.<br/>**BR-03:** Báo cáo có thể được schedule chạy định kỳ.<br/>**BR-04:** Dữ liệu báo cáo phải được cache để tối ưu performance.<br/>**BR-05:** Báo cáo nhạy cảm chỉ được share với người có quyền.<br/>**BR-06:** Báo cáo cũ hơn 1 năm tự động archive. |
| Dashboard | Widget | **BR-01:** Dashboard có thể được share với nhiều user.<br/>**BR-02:** Widget phải có data source hợp lệ.<br/>**BR-03:** Dashboard public có thể xem bởi tất cả user có quyền.<br/>**BR-04:** Widget có thể được drag & drop để sắp xếp lại.<br/>**BR-05:** Dashboard phải có ít nhất 1 widget. |

### **3. Mối quan hệ giữa các Context**

```
[UserAccessContext] ←→ [CustomerContext]
         ↓                    ↓
[ProductContext] ←→ [InventoryContext] ←→ [OrderContext]
         ↓                    ↓                    ↓
[ShippingContext] ←→ [PaymentContext] ←→ [NotificationContext]
         ↓                    ↓                    ↓
[ReportingContext] ←→ [BackgroundJobs] ←→ [SystemContext]
```

**Mối quan hệ chính:**
- **OrderContext** là trung tâm, kết nối với hầu hết contexts khác
- **UserAccessContext** quản lý authentication cho tất cả contexts
- **InventoryContext** đồng bộ với **ProductContext** và **OrderContext**
- **PaymentContext** xử lý thanh toán cho **OrderContext**
- **NotificationContext** gửi thông báo cho tất cả contexts
- **ReportingContext** tổng hợp dữ liệu từ tất cả contexts

### **4. Tóm tắt Core Aggregates & Business Rules**

#### **4.1. Core Aggregates đã xác định:**

| Bounded Context | Core Aggregates |
|---|---|
| **UserAccessContext** | User, UserSession, SystemConfiguration |
| **CustomerContext** | Customer, CustomerAddress, CustomerPreference |
| **ProductContext** | Product, Category, ProductImage, ProductAttribute |
| **OrderContext** | Order, OrderItem, ShoppingCart, CartItem |
| **PaymentContext** | PaymentTransaction, Wallet, WalletTransaction |
| **InventoryContext** | Inventory, StockMovement, StockAlert |
| **ShippingContext** | Shipment, ShippingMethod, ShippingZone |
| **NotificationContext** | Notification, NotificationTemplate, NotificationQueue |
| **ReportingContext** | Report, ReportTemplate, Dashboard, Widget |

#### **4.2. Business Rules chính:**

**Tính toàn vẹn dữ liệu:**
- Email unique trong hệ thống
- SKU unique cho sản phẩm
- Số dư ví không được âm
- Tổng tiền đơn hàng phải > 0

**Workflow nghiệp vụ:**
- Order status flow: Pending → Confirmed → Processing → Shipped → Delivered
- Payment processing: Pending → Success/Failed → Refunded
- Inventory management: Reserve → Confirm → Release

**Authorization:**
- Role-based access control (Admin, Manager, Staff, Customer)
- Permission validation cho từng action
- Session management và timeout

**Data consistency:**
- Cross-context synchronization
- Event-driven updates
- Transaction boundaries

#### **4.3. Tóm tắt vai trò Context:**

| Context | Vai trò chính | Tương tác với |
|---|---|---|
| **UserAccessContext** | Quản lý authentication & authorization | Tất cả contexts |
| **CustomerContext** | Quản lý thông tin khách hàng | OrderContext, PaymentContext |
| **ProductContext** | Quản lý catalog sản phẩm | InventoryContext, OrderContext |
| **OrderContext** | **Trung tâm** xử lý đơn hàng | Hầu hết contexts |
| **PaymentContext** | Xử lý thanh toán & ví điện tử | OrderContext, CustomerContext |
| **InventoryContext** | Quản lý tồn kho | ProductContext, OrderContext |
| **ShippingContext** | Quản lý vận chuyển | OrderContext |
| **NotificationContext** | Gửi thông báo | Tất cả contexts |
| **ReportingContext** | Tổng hợp dữ liệu & báo cáo | Tất cả contexts |

## Bước 4: Xác định các Event & Workflow giữa các Bounded Contexts (Tích hợp giữa các context)

### **Mục tiêu:**
- Xác định các Domain Events quan trọng trong hệ thống.
- Thiết kế Workflow giữa các Bounded Contexts để đảm bảo luồng xử lý nghiệp vụ diễn ra đúng logic.
- Sử dụng Event-Driven Architecture để liên kết các Contexts theo mô hình publish-subscribe với RabbitMQ.

### **1. Tổng quan về Event & Workflow trong kiến trúc Modular Monolith**

**Domain Events:** Được phát sinh trong một Context khi có sự kiện quan trọng xảy ra.

**Event Handlers:** Các Context khác có thể đăng ký lắng nghe sự kiện để thực hiện hành động phù hợp.

**Workflow:** Là chuỗi các sự kiện diễn ra theo thứ tự để hoàn thành một quy trình nghiệp vụ.

**Message Broker (RabbitMQ)** giúp đảm bảo giao tiếp giữa các Contexts một cách bất đồng bộ.

**Cơ chế hoạt động:**
1. Một sự kiện xảy ra trong Bounded Context A.
2. Bounded Context A phát một sự kiện (Publish) lên Message Bus.
3. Bounded Context B nhận sự kiện (Subscribe) và thực hiện hành động phù hợp.

### **2. Xác định Domain Events quan trọng**

#### **2.1. Sự kiện từ OrderContext**

| Domain Event | Khi nào xảy ra? | Ai lắng nghe? | Hành động của Subscriber |
|---|---|---|---|
| **OrderPlaced** | Khi Customer đặt hàng thành công | InventoryContext<br/>PaymentContext<br/>NotificationContext | Giảm số lượng tồn kho<br/>Xử lý thanh toán<br/>Gửi thông báo đặt hàng |
| **OrderConfirmed** | Khi Admin xác nhận đơn hàng | InventoryContext<br/>ShippingContext<br/>NotificationContext | Chuẩn bị hàng để giao<br/>Tạo shipment<br/>Gửi thông báo xác nhận |
| **OrderCanceled** | Khi Customer hoặc Admin hủy đơn | InventoryContext<br/>PaymentContext<br/>NotificationContext | Hoàn lại số lượng tồn kho<br/>Xử lý hoàn tiền<br/>Gửi thông báo hủy đơn |
| **OrderShipped** | Khi đơn hàng được giao đi | NotificationContext<br/>SystemContext | Gửi email/SMS thông báo giao hàng<br/>Ghi log vận chuyển |
| **OrderDelivered** | Khi đơn hàng được giao thành công | NotificationContext<br/>SystemContext<br/>ReportingContext | Gửi thông báo hoàn tất<br/>Ghi log đơn hàng hoàn tất<br/>Cập nhật báo cáo |

#### **2.2. Sự kiện từ PaymentContext**

| Domain Event | Khi nào xảy ra? | Ai lắng nghe? | Hành động của Subscriber |
|---|---|---|---|
| **PaymentProcessed** | Khi thanh toán thành công | OrderContext<br/>NotificationContext<br/>ReportingContext | Cập nhật trạng thái đơn hàng thành "Confirmed"<br/>Gửi thông báo thanh toán thành công<br/>Cập nhật báo cáo tài chính |
| **PaymentFailed** | Khi thanh toán thất bại | OrderContext<br/>NotificationContext | Hiển thị lỗi, giữ đơn hàng ở trạng thái "Pending"<br/>Gửi thông báo thanh toán thất bại |
| **RefundProcessed** | Khi hoàn tiền thành công | OrderContext<br/>CustomerContext<br/>NotificationContext | Cập nhật trạng thái đơn hàng<br/>Cập nhật ví khách hàng<br/>Gửi thông báo hoàn tiền |

#### **2.3. Sự kiện từ InventoryContext**

| Domain Event | Khi nào xảy ra? | Ai lắng nghe? | Hành động của Subscriber |
|---|---|---|---|
| **StockReserved** | Khi tồn kho được giữ chỗ cho đơn hàng | OrderContext<br/>SystemContext | Tiếp tục xử lý đơn hàng<br/>Ghi log reserve stock |
| **StockUpdated** | Khi số lượng tồn kho thay đổi | SystemContext<br/>ReportingContext<br/>NotificationContext | Ghi log sự thay đổi tồn kho<br/>Cập nhật báo cáo tồn kho<br/>Gửi thông báo nếu hết hàng |
| **StockRestored** | Khi đơn hàng bị hủy | OrderContext<br/>SystemContext | Hoàn lại số lượng tồn kho<br/>Ghi log restore stock |
| **LowStockAlert** | Khi tồn kho xuống dưới mức tối thiểu | NotificationContext<br/>SystemContext | Gửi thông báo cho Manager<br/>Ghi log cảnh báo |

#### **2.4. Sự kiện từ UserAccessContext**

| Domain Event | Khi nào xảy ra? | Ai lắng nghe? | Hành động của Subscriber |
|---|---|---|---|
| **UserLoggedIn** | Khi user đăng nhập thành công | NotificationContext<br/>SystemContext | Gửi thông báo đăng nhập<br/>Ghi log audit |
| **UserRoleChanged** | Khi role của user thay đổi | OrderContext<br/>SystemContext | Cập nhật quyền xem đơn hàng<br/>Ghi log thay đổi quyền |
| **UserAccountLocked** | Khi tài khoản bị khóa | NotificationContext<br/>SystemContext | Gửi thông báo khóa tài khoản<br/>Ghi log bảo mật |

#### **2.5. Sự kiện từ ProductContext**

| Domain Event | Khi nào xảy ra? | Ai lắng nghe? | Hành động của Subscriber |
|---|---|---|---|
| **ProductCreated** | Khi tạo sản phẩm mới | InventoryContext<br/>SystemContext | Tạo tồn kho mới<br/>Ghi log tạo sản phẩm |
| **ProductPriceChanged** | Khi giá sản phẩm thay đổi | OrderContext<br/>SystemContext | Cập nhật giá trong giỏ hàng<br/>Ghi log thay đổi giá |
| **ProductArchived** | Khi sản phẩm bị ngừng bán | OrderContext<br/>InventoryContext | Ẩn sản phẩm khỏi catalog<br/>Ngừng bán sản phẩm |

#### **2.6. Sự kiện từ BackgroundContext**

| Domain Event | Khi nào xảy ra? | Ai lắng nghe? | Hành động của Subscriber |
|---|---|---|---|
| **UnpaidOrderCanceled** | Khi đơn hàng chưa thanh toán bị hủy tự động | InventoryContext<br/>PaymentContext<br/>NotificationContext | Hoàn lại số lượng hàng<br/>Xử lý hoàn tiền<br/>Gửi thông báo hủy đơn |
| **OrderNotificationSent** | Khi hệ thống gửi thông báo đơn hàng | SystemContext | Lưu log thông báo đã gửi |
| **SystemHealthCheck** | Khi kiểm tra sức khỏe hệ thống | SystemContext | Ghi log health check |

### **3. Xây dựng Workflow giữa các Bounded Contexts**

#### **3.1. Workflow: Đặt hàng & Thanh toán**

```
[Customer] → [OrderContext: OrderPlaced] 
   → [InventoryContext: StockReserved] 
   → [PaymentContext: PaymentProcessed] 
   → [OrderContext: OrderConfirmed] 
   → [BackgroundContext: Gửi thông báo]
```

**Chi tiết:**
1. **OrderPlaced** → Hệ thống kiểm tra tồn kho (StockReserved)
2. Nếu hàng có đủ, gửi yêu cầu thanh toán (PaymentProcessed)
3. Nếu thanh toán thành công, đơn hàng chuyển Confirmed
4. Gửi thông báo đến Customer

#### **3.2. Workflow: Hủy đơn hàng & Hoàn tiền**

```
[Customer/Admin] → [OrderContext: OrderCanceled] 
   → [InventoryContext: StockRestored] 
   → [PaymentContext: RefundProcessed] 
   → [BackgroundContext: Gửi thông báo]
```

**Chi tiết:**
1. **OrderCanceled** → Hoàn lại hàng (StockRestored)
2. Nếu đơn hàng đã thanh toán, gửi yêu cầu hoàn tiền (RefundProcessed)
3. Gửi thông báo cập nhật trạng thái đơn hàng

#### **3.3. Workflow: Xử lý đơn hàng chưa thanh toán (Tác vụ nền)**

```
[Background Job: Worker Service] → [UnpaidOrderCanceled] 
   → [InventoryContext: StockRestored] 
   → [PaymentContext: RefundProcessed] 
   → [OrderContext: Cập nhật trạng thái]
```

**Chi tiết:**
1. **UnpaidOrderCanceled** → Kiểm tra đơn hàng quá hạn thanh toán
2. Nếu đơn hàng chưa thanh toán, hủy đơn và StockRestored
3. Nếu có giao dịch pending, gửi yêu cầu RefundProcessed

#### **3.4. Workflow: Giao hàng & Hoàn tất**

```
[Staff] → [ShippingContext: OrderShipped] 
   → [NotificationContext: Gửi thông báo] 
   → [Customer] → [ShippingContext: OrderDelivered] 
   → [NotificationContext: Gửi thông báo hoàn tất]
```

**Chi tiết:**
1. **OrderShipped** → Gửi thông báo tracking number
2. **OrderDelivered** → Gửi thông báo hoàn tất giao hàng
3. Cập nhật báo cáo và log hệ thống

### **4. Integration Events (Cross-Context Communication)**

#### **4.1. Event Categories:**

**Order Integration Events:**
- `OrderCreatedIntegrationEvent`
- `OrderStatusChangedIntegrationEvent`
- `OrderCompletedIntegrationEvent`

**Payment Integration Events:**
- `PaymentCompletedIntegrationEvent`
- `PaymentFailedIntegrationEvent`
- `RefundProcessedIntegrationEvent`

**Inventory Integration Events:**
- `InventoryUpdatedIntegrationEvent`
- `StockAlertIntegrationEvent`
- `ProductAvailabilityChangedIntegrationEvent`

**Notification Integration Events:**
- `NotificationSentIntegrationEvent`
- `NotificationFailedIntegrationEvent`

### **5. Error Handling & Retry Mechanisms**

#### **5.1. Error Scenarios:**

| Error Type | Context | Retry Strategy | Fallback Action |
|---|---|---|---|
| **PaymentFailed** | PaymentContext | 3 lần với exponential backoff | Rollback order status |
| **StockInsufficient** | InventoryContext | Không retry | Reject order immediately |
| **NotificationFailed** | NotificationContext | 3 lần với delay tăng dần | Log error, manual retry |
| **GatewayTimeout** | PaymentContext | 2 lần với timeout tăng | Queue for manual processing |

#### **5.2. Dead Letter Queue:**
- Events không xử lý được sau retry sẽ vào DLQ
- Manual intervention để xử lý
- Monitoring và alerting

### **6. Context Map & Event Flow**

```
[UserAccessContext] ←→ [CustomerContext]
         ↓                    ↓
[ProductContext] ←→ [InventoryContext] ←→ [OrderContext]
         ↓                    ↓                    ↓
[ShippingContext] ←→ [PaymentContext] ←→ [NotificationContext]
         ↓                    ↓                    ↓
[ReportingContext] ←→ [BackgroundJobs] ←→ [SystemContext]
```

**Event Flow Patterns:**
- **Synchronous**: Order → Inventory (immediate validation)
- **Asynchronous**: Order → Payment → Notification (event-driven)
- **Saga Pattern**: Complex workflows with compensation

### **7. Kết luận & Bước tiếp theo**

**Event-Driven Architecture** giúp Contexts giao tiếp hiệu quả, không bị phụ thuộc chặt chẽ vào nhau.

**RabbitMQ** hoặc một Event Bus nội bộ giúp truyền tải các sự kiện bất đồng bộ.

**Workflow** giúp xác định rõ cách các Context tương tác, tránh lỗi nghiệp vụ.

**Monitoring & Observability** cần được thiết kế để theo dõi event flow và performance.