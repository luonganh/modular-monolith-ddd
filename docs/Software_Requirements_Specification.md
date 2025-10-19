# E-commerce Lite — Software Requirements Specification (SRS)

## **1. Tổng quan**

### **1.1. Mục đích tài liệu**
Tài liệu này mô tả chi tiết các yêu cầu chức năng và phi chức năng của hệ thống E-commerce Lite, được phát triển theo kiến trúc Modular Monolith với Domain-Driven Design.

### **1.2. Phạm vi hệ thống**
Hệ thống E-commerce Lite hỗ trợ:
- Quản lý sản phẩm và catalog
- Xử lý đơn hàng và thanh toán
- Quản lý khách hàng và tài khoản
- Quản lý tồn kho và vận chuyển
- Báo cáo và analytics
- Hệ thống thông báo

### **1.3. Đối tượng sử dụng**
- **Customer**: Người mua hàng
- **Administrator**: Quản trị viên cấp cao
- **Manager**: Quản lý
- **Staff**: Nhân viên
- **System**: Background jobs và external services

---

## **2. Yêu cầu chức năng (Functional Requirements)**

### **2.1. UserAccess Module**

#### **FR-001: Quản lý tài khoản quản trị**
**Mô tả**: Hệ thống cho phép tạo, quản lý và xác thực tài khoản quản trị

**Use Cases**:
- UC-001: Đăng nhập tài khoản quản trị
- UC-002: Đăng xuất tài khoản quản trị
- UC-003: Xem/Chỉnh sửa thông tin tài khoản
- UC-004: Đổi mật khẩu tài khoản
- UC-005: Reset mật khẩu tài khoản cấp dưới

**Acceptance Criteria**:
- Username và Email phải duy nhất trong hệ thống
- Mật khẩu phải đáp ứng policy (tối thiểu 8 ký tự, có chữ hoa, số, ký tự đặc biệt)
- Tài khoản phải kích hoạt mới có thể đăng nhập
- Admin có thể khóa/bỏ khóa tài khoản Manager và Staff
- Manager chỉ có thể quản lý Staff, không thể quản lý Admin
- Sau 5 lần đăng nhập sai, tài khoản bị khóa 30 phút
- Session tự động expire sau 8 giờ không hoạt động

#### **FR-002: Phân quyền người dùng**
**Mô tả**: Hệ thống hỗ trợ phân quyền theo role (Admin, Manager, Staff)

**Acceptance Criteria**:
- Admin có quyền cao nhất, quản lý tất cả
- Manager quản lý Staff và business operations
- Staff có quyền hạn chế, chỉ xem và cập nhật dữ liệu
- Mọi thao tác phải được log để audit

### **2.2. Customer Module**

#### **FR-003: Quản lý tài khoản khách hàng**
**Mô tả**: Khách hàng có thể đăng ký, đăng nhập và quản lý thông tin cá nhân

**Use Cases**:
- UC-006: Đăng ký tài khoản
- UC-007: Đăng nhập Customer
- UC-008: Xem/Chỉnh sửa thông tin cá nhân
- UC-009: Đổi mật khẩu
- UC-010: Quên mật khẩu

**Acceptance Criteria**:
- Email phải duy nhất trong hệ thống
- Số điện thoại phải đúng định dạng Việt Nam
- Tài khoản phải verify email trước khi có thể đặt hàng
- Customer có thể có nhiều địa chỉ, chỉ 1 địa chỉ mặc định
- Sau 3 lần đăng nhập sai, tài khoản bị khóa 15 phút

#### **FR-004: Quản lý ví điện tử**
**Mô tả**: Khách hàng có thể nạp tiền, rút tiền và thanh toán bằng ví

**Use Cases**:
- UC-011: Nạp tiền vào ví
- UC-012: Thanh toán bằng ví

**Acceptance Criteria**:
- Số dư ví không được âm
- Nạp tiền phải qua cổng thanh toán hợp lệ
- Rút tiền phải có số dư đủ
- Mỗi Customer chỉ có 1 ví điện tử
- Giao dịch ví phải được log đầy đủ để audit

### **2.3. Product Module**

#### **FR-005: Quản lý sản phẩm**
**Mô tả**: Quản lý catalog sản phẩm, danh mục và thuộc tính

**Use Cases**:
- UC-013: Quản lý danh mục sản phẩm
- UC-014: Quản lý sản phẩm
- UC-015: Duyệt danh sách sản phẩm
- UC-016: Xem chi tiết sản phẩm
- UC-017: Tìm kiếm sản phẩm
- UC-018: Lọc sản phẩm

**Acceptance Criteria**:
- SKU phải duy nhất trong hệ thống
- Tên sản phẩm không được trống và tối đa 200 ký tự
- Giá sản phẩm phải lớn hơn 0
- Sản phẩm phải có ít nhất 1 hình ảnh mới được publish
- Không thể xóa sản phẩm đã có đơn hàng
- Chỉ sản phẩm Active mới hiển thị cho Customer

### **2.4. Order Module**

#### **FR-006: Quản lý đơn hàng**
**Mô tả**: Xử lý đặt hàng, giỏ hàng và quản lý đơn hàng

**Use Cases**:
- UC-019: Thêm sản phẩm vào giỏ hàng
- UC-020: Đặt hàng COD
- UC-021: Đặt hàng Online
- UC-022: Theo dõi đơn hàng
- UC-023: Hủy đơn hàng
- UC-024: Quản lý đơn hàng (Admin/Manager)

**Acceptance Criteria**:
- Đơn hàng phải có ít nhất một sản phẩm
- Tổng tiền đơn hàng phải lớn hơn 0
- Chỉ có thể hủy đơn hàng khi status = Pending hoặc Confirmed
- Không thể hủy đơn hàng đã giao hàng
- Chỉ Admin/Manager có thể xác nhận đơn hàng
- Đơn hàng tự động hủy sau 30 phút nếu chưa thanh toán
- Giỏ hàng tự động expire sau 30 ngày không hoạt động

### **2.5. Payment Module**

#### **FR-007: Xử lý thanh toán**
**Mô tả**: Xử lý thanh toán qua các cổng thanh toán và ví điện tử

**Use Cases**:
- UC-025: Xử lý giao dịch (Payment Gateway)
- UC-026: Hoàn tiền giao dịch (Payment Gateway)
- UC-027: Quản lý thanh toán (Manager)
- UC-028: Xử lý hoàn tiền (Manager)

**Acceptance Criteria**:
- Chỉ xử lý thanh toán nếu đơn hàng hợp lệ và chưa thanh toán
- Không thể hoàn tiền nếu giao dịch bị thất bại
- Không thể hoàn tiền nhiều hơn số tiền đã thanh toán
- Giao dịch COD không cần xử lý thanh toán trước
- Giao dịch online phải có response từ gateway mới được coi là thành công
- Hoàn tiền phải được Admin/Manager phê duyệt

### **2.6. Inventory Module**

#### **FR-008: Quản lý tồn kho**
**Mô tả**: Quản lý số lượng tồn kho và cảnh báo

**Use Cases**:
- UC-029: Quản lý tồn kho (Manager)
- UC-030: Xem tồn kho (Staff)
- UC-031: Chỉnh sửa tồn kho (Staff)

**Acceptance Criteria**:
- Không thể giảm số lượng tồn kho xuống dưới 0
- Phải kiểm tra tồn kho trước khi cho phép đặt hàng
- Khi đơn hàng bị hủy, số lượng đã reserve phải được hoàn lại
- Khi đơn hàng được xác nhận, reserved quantity chuyển thành actual decrease
- Hệ thống tự động tạo alert khi stock xuống dưới min level
- Mọi thay đổi tồn kho phải được log với lý do rõ ràng

### **2.7. Shipping Module**

#### **FR-009: Quản lý vận chuyển**
**Mô tả**: Xử lý vận chuyển và giao hàng

**Use Cases**:
- UC-032: Hỗ trợ vận chuyển (Staff)
- UC-033: Cập nhật trạng thái đơn (Staff)

**Acceptance Criteria**:
- Mỗi đơn hàng chỉ có thể có 1 shipment
- Phí vận chuyển được tính dựa trên trọng lượng và khu vực
- Chỉ có thể tạo shipment khi đơn hàng đã được xác nhận
- Tracking number phải unique trong hệ thống
- Status shipment phải tuân theo workflow: Preparing → Shipped → InTransit → Delivered
- Nếu delivery thất bại, có thể retry tối đa 3 lần

### **2.8. Notification Module**

#### **FR-010: Gửi thông báo**
**Mô tả**: Gửi thông báo qua Email, SMS và Push notification

**Use Cases**:
- UC-034: Gửi thông báo cho Customer (Background Jobs)
- UC-035: Quản lý cấu hình email (Administrator)
- UC-038: Hủy đơn quá hạn (Background Jobs)
- UC-039: Gửi thông báo định kỳ (Background Jobs)

**Acceptance Criteria**:
- Email phải có địa chỉ recipient hợp lệ
- SMS phải có số điện thoại đúng định dạng
- Notification có priority cao được xử lý trước
- Nếu gửi thất bại, retry tối đa 3 lần với interval tăng dần
- Template phải có tất cả biến cần thiết mới được sử dụng
- Notification quan trọng phải có backup channel (SMS nếu email fail)

### **2.9. Reporting Module**

#### **FR-011: Báo cáo và Analytics**
**Mô tả**: Tạo và xem báo cáo, dashboard

**Use Cases**:
- UC-036: Tạo và xem báo cáo (Manager)
- UC-037: Xem báo cáo (Staff)
- UC-040: Kiểm tra sức khỏe hệ thống (Background Jobs)
- UC-041: Dọn dẹp dữ liệu cũ (Background Jobs)

**Acceptance Criteria**:
- Chỉ Manager/Admin mới có thể tạo báo cáo
- Báo cáo phải có tên và mô tả rõ ràng
- Báo cáo có thể được schedule chạy định kỳ
- Dữ liệu báo cáo phải được cache để tối ưu performance
- Báo cáo nhạy cảm chỉ được share với người có quyền
- Báo cáo cũ hơn 1 năm tự động archive

---

## **3. Yêu cầu phi chức năng (Non-Functional Requirements)**

### **3.1. Performance Requirements**

#### **NFR-001: Response Time**
- **API Response Time**: < 200ms cho 95% requests
- **Database Query Time**: < 100ms cho 95% queries
- **Page Load Time**: < 3 giây cho 95% page loads
- **Payment Processing**: < 5 giây cho 95% transactions

#### **NFR-002: Throughput**
- **Concurrent Users**: Hỗ trợ 1000 users đồng thời
- **API Requests**: 10,000 requests/phút
- **Database Transactions**: 5,000 transactions/phút
- **Event Processing**: 1,000 events/phút

#### **NFR-003: Scalability**
- **Horizontal Scaling**: Hỗ trợ scale out theo nhu cầu
- **Database Scaling**: Hỗ trợ read replicas
- **Event Processing**: Hỗ trợ multiple consumers
- **Caching**: Redis cache cho performance

### **3.2. Security Requirements**

#### **NFR-004: Authentication & Authorization**
- **Multi-factor Authentication**: Hỗ trợ 2FA cho admin accounts
- **Session Management**: Secure session với timeout
- **Password Policy**: Mạnh mẽ với complexity requirements
- **Account Lockout**: Sau failed attempts

#### **NFR-005: Data Protection**
- **Data Encryption**: AES-256 cho sensitive data
- **HTTPS**: Bắt buộc cho tất cả communications
- **PCI DSS**: Compliance cho payment data
- **GDPR**: Compliance cho customer data

#### **NFR-006: Audit & Logging**
- **Audit Trail**: Log tất cả business operations
- **Security Logging**: Log authentication, authorization
- **Data Access Logging**: Log data access patterns
- **Retention Policy**: 7 năm cho audit logs

### **3.3. Reliability Requirements**

#### **NFR-007: Availability**
- **Uptime**: 99.9% availability (8.76 hours downtime/year)
- **Recovery Time**: < 4 giờ cho disaster recovery
- **Backup Strategy**: Daily backups với 30-day retention
- **Monitoring**: 24/7 system monitoring

#### **NFR-008: Error Handling**
- **Graceful Degradation**: System continues với reduced functionality
- **Error Recovery**: Automatic retry mechanisms
- **Circuit Breaker**: Prevent cascade failures
- **Dead Letter Queue**: Handle failed events

### **3.4. Usability Requirements**

#### **NFR-009: User Experience**
- **Mobile Responsive**: Hoạt động tốt trên mobile devices
- **Accessibility**: WCAG 2.1 AA compliance
- **Multi-language**: Hỗ trợ tiếng Việt và tiếng Anh
- **Progressive Web App**: PWA capabilities

#### **NFR-010: Admin Interface**
- **Dashboard**: Real-time metrics và KPIs
- **Bulk Operations**: Hỗ trợ bulk import/export
- **Search & Filter**: Advanced search capabilities
- **Data Visualization**: Charts và graphs

### **3.5. UI/UX Design Requirements**

#### **NFR-011: Admin Portal Layout**
**Mô tả**: Thiết kế layout cho admin portal sử dụng react-dashboard-template

**Layout Structure**:
- **Header**: Logo, user profile, notifications, theme toggle
- **Sidebar**: Navigation menu với collapsible functionality
- **Main Content**: Dynamic content area với breadcrumb navigation
- **Footer**: System information và copyright

**Navigation Menu**:
- **Dashboard**: Overview metrics và KPIs
- **Products**: Product management (CRUD, categories, inventory)
- **Orders**: Order management (list, detail, status updates)
- **Customers**: Customer management (profiles, orders, preferences)
- **Payments**: Payment transactions và refunds
- **Inventory**: Stock management và alerts
- **Shipping**: Delivery tracking và logistics
- **Reports**: Analytics và business intelligence
- **Settings**: System configuration và user management

**Page Templates**:
- **List Pages**: Data tables với pagination, sorting, filtering
- **Detail Pages**: Form layouts với validation
- **Dashboard Pages**: Widget-based layout với charts
- **Modal Dialogs**: Quick actions và confirmations

#### **NFR-012: Customer Portal Layout**
**Mô tả**: Thiết kế layout cho customer portal (e-commerce website)

**Layout Structure**:
- **Header**: Logo, search bar, cart icon, user menu
- **Navigation**: Category menu, breadcrumbs
- **Main Content**: Product listings, product details, checkout flow
- **Footer**: Links, contact info, social media

**Page Templates**:
- **Home Page**: Hero section, featured products, categories
- **Product Catalog**: Grid layout với filters và sorting
- **Product Detail**: Image gallery, description, add to cart
- **Shopping Cart**: Item list, quantity updates, checkout button
- **Checkout**: Multi-step form (shipping, payment, confirmation)
- **User Account**: Profile, order history, preferences

#### **NFR-013: Responsive Design**
**Mô tả**: Thiết kế responsive cho tất cả devices

**Breakpoints**:
- **Mobile**: 320px - 768px (mobile-first approach)
- **Tablet**: 768px - 1024px
- **Desktop**: 1024px - 1440px
- **Large Desktop**: 1440px+

**Mobile Considerations**:
- Touch-friendly buttons (min 44px)
- Swipe gestures cho navigation
- Optimized forms cho mobile input
- Fast loading trên slow connections

#### **NFR-014: Visual Design System**
**Mô tả**: Hệ thống thiết kế thống nhất cho toàn bộ ứng dụng

**Color Palette**:
- **Primary**: Brand colors cho main actions
- **Secondary**: Supporting colors cho accents
- **Neutral**: Grays cho text và backgrounds
- **Status**: Success, warning, error, info colors
- **Dark/Light Theme**: Theme switching support

**Typography**:
- **Font Family**: System fonts (Inter, Roboto, etc.)
- **Font Sizes**: Responsive typography scale
- **Font Weights**: Regular, medium, semibold, bold
- **Line Heights**: Optimal readability

**Component Library**:
- **Buttons**: Primary, secondary, outline, ghost variants
- **Forms**: Input fields, selects, checkboxes, radio buttons
- **Tables**: Sortable, filterable data tables
- **Cards**: Product cards, info cards, metric cards
- **Charts**: Line, bar, pie, donut charts
- **Modals**: Confirmation dialogs, form modals
- **Navigation**: Sidebar, breadcrumbs, pagination

#### **NFR-015: User Experience Flow**
**Mô tả**: User journey và interaction patterns

**Admin User Flow**:
1. **Login** → Dashboard overview
2. **Product Management** → Add/Edit products → Publish
3. **Order Management** → View orders → Update status
4. **Customer Support** → View customer issues → Resolve
5. **Reports** → Generate reports → Export data

**Customer User Flow**:
1. **Browse** → Search products → View details
2. **Add to Cart** → Review cart → Proceed to checkout
3. **Checkout** → Shipping info → Payment → Confirmation
4. **Track Order** → View status → Receive notifications
5. **Account** → View history → Update profile

#### **NFR-016: Accessibility Requirements**
**Mô tả**: Đảm bảo accessibility cho tất cả users

**WCAG 2.1 AA Compliance**:
- **Keyboard Navigation**: Full keyboard accessibility
- **Screen Reader**: Proper ARIA labels và descriptions
- **Color Contrast**: Minimum 4.5:1 ratio
- **Focus Management**: Clear focus indicators
- **Alternative Text**: Images có alt text
- **Form Labels**: Proper form labeling

#### **NFR-017: Performance Requirements**
**Mô tả**: Performance requirements cho UI/UX

**Loading Performance**:
- **First Contentful Paint**: < 1.5s
- **Largest Contentful Paint**: < 2.5s
- **Cumulative Layout Shift**: < 0.1
- **First Input Delay**: < 100ms
- **Time to Interactive**: < 3s

**User Experience Metrics**:
- **Page Load Time**: < 3s cho 95% pages
- **API Response Time**: < 200ms cho 95% requests
- **Image Optimization**: WebP format với fallbacks
- **Code Splitting**: Lazy loading cho performance

### **3.5. Integration Requirements**

#### **NFR-011: External Services**
- **Payment Gateways**: VNPay, MoMo, Stripe integration
- **Email Service**: SMTP/SendGrid integration
- **SMS Service**: Twilio/Viettel integration
- **Shipping APIs**: Giao hàng nhanh, Viettel Post

#### **NFR-012: API Requirements**
- **RESTful APIs**: Standard REST endpoints
- **API Versioning**: Backward compatibility
- **Rate Limiting**: Prevent abuse
- **Documentation**: OpenAPI/Swagger specs

---

## **4. Constraints & Assumptions**

### **4.1. Technical Constraints**
- **Performance**: Hệ thống phải xử lý 1000 concurrent users
- **Security**: Phải tuân thủ PCI DSS và GDPR
- **Integration**: Phải tích hợp với VNPay, MoMo, Stripe
- **Availability**: 99.9% uptime requirement
- **Data Volume**: Hỗ trợ 1M+ products, 100K+ orders

### **4.2. Business Constraints**
- **Budget**: Limited budget cho infrastructure và third-party services
- **Timeline**: Development trong 6-12 tháng
- **Team Size**: 1 developer (solo development)
- **Compliance**: PCI DSS cho payment data, GDPR cho customer data
- **Market Requirements**: Phải cạnh tranh với các e-commerce hiện có

### **4.3. Assumptions**
- **User Behavior**: Users có kiến thức cơ bản về online shopping
- **Data Growth**: Tăng trưởng dữ liệu ổn định, không đột biến
- **Third-party Services**: VNPay, MoMo, Stripe hoạt động ổn định
- **Infrastructure**: Có thể scale theo nhu cầu business
- **Regulatory**: Không có thay đổi lớn về compliance requirements

---

## **5. Acceptance Criteria Summary**

### **5.1. Functional Acceptance**
- Tất cả Use Cases hoạt động đúng specification
- Business Rules được enforce correctly
- Data validation hoạt động properly
- Error handling graceful

### **5.2. Non-Functional Acceptance**
- Performance metrics đạt target
- Security requirements được meet
- Reliability targets achieved
- Usability standards met

### **5.3. Integration Acceptance**
- External services integration working
- API contracts implemented correctly
- Event-driven communication functional
- Database consistency maintained

---

## **6. Risks & Mitigation**

### **6.1. Technical Risks**
- **Performance Issues**: Load testing, optimization
- **Security Vulnerabilities**: Security audits, penetration testing
- **Integration Failures**: Comprehensive testing, fallback mechanisms

### **6.2. Business Risks**
- **Scope Creep**: Change management process
- **Timeline Delays**: Agile methodology, regular reviews
- **Budget Overrun**: Cost monitoring, resource optimization

---

## **7. Appendices**

### **7.1. Glossary**
- **Bounded Context**: Domain boundary trong DDD
- **Aggregate**: Domain entity cluster
- **Event Sourcing**: Event-driven data storage
- **CQRS**: Command Query Responsibility Segregation

### **7.2. References**
- Business Analysis.md
- Domain-Driven Design patterns
- .NET 9 documentation
- RabbitMQ documentation

### **7.3. Change History**
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-19 | System Analyst | Initial version |
