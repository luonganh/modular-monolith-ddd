# OpenIddict Configuration Without EF Core

Tài liệu này hướng dẫn cách cấu hình OpenIddict mà không sử dụng Entity Framework Core.

## Các Phương Pháp Cấu Hình

### 1. In-Memory Stores (Khuyến nghị cho Development/Testing)

**Ưu điểm:**
- Đơn giản, không cần database
- Phù hợp cho development và testing
- Không cần cấu hình database connection

**Nhược điểm:**
- Dữ liệu bị mất khi restart application
- Không phù hợp cho production

**Cách sử dụng:**
```csharp
// Trong Program.cs
builder.Services.AddUserAccessAuthenticationInMemory(builder.Configuration);

// Seed dữ liệu
await OpenIddictInMemorySeeder.SeedAsync(app.Services, app.Configuration);
```

**Files liên quan:**
- `OpenIddictInMemoryRegistration.cs` - Cấu hình OpenIddict với In-Memory stores
- `OpenIddictInMemorySeeder.cs` - Seeder cho In-Memory stores
- `ProgramInMemory.cs` - Ví dụ Program.cs không sử dụng EF Core

### 2. Custom Stores (Cho Production)

**Ưu điểm:**
- Dữ liệu persistent
- Có thể tích hợp với bất kỳ storage nào (MongoDB, Redis, SQL Server trực tiếp, etc.)
- Linh hoạt trong việc quản lý dữ liệu

**Nhược điểm:**
- Phức tạp hơn, cần implement các store interfaces
- Cần quản lý connection và transaction

**Cách sử dụng:**
```csharp
// Trong Program.cs
builder.Services.AddUserAccessAuthenticationCustomStores(builder.Configuration);
```

**Files liên quan:**
- `OpenIddictCustomStoreRegistration.cs` - Cấu hình với Custom Stores
- Cần implement: `CustomApplicationStore`, `CustomScopeStore`, `CustomTokenStore`

## So Sánh Với EF Core

| Tính năng | EF Core | In-Memory | Custom Stores |
|-----------|---------|-----------|---------------|
| Setup | Phức tạp | Đơn giản | Phức tạp |
| Performance | Tốt | Rất tốt | Tùy implementation |
| Persistence | Có | Không | Có |
| Production Ready | Có | Không | Có |
| Flexibility | Trung bình | Thấp | Cao |

## Migration từ EF Core

### Bước 1: Thay đổi cấu hình
```csharp
// Thay vì
o.UseEntityFrameworkCore().UseDbContext<UserAccessContext>();

// Sử dụng
o.UseInMemory(); // hoặc
o.UseCustomStores().ReplaceDefaultApplicationStore<CustomApplicationStore>();
```

### Bước 2: Loại bỏ EF Core dependencies
```csharp
// Xóa các dòng này
builder.Services.AddDbContextFactory<UserAccessContext>(...);
builder.Services.AddScoped<DbContext>(...);
```

### Bước 3: Cập nhật seeder
```csharp
// Thay vì
await OpenIddictSeeder.SeedAsync(sp, app.Configuration);

// Sử dụng
await OpenIddictInMemorySeeder.SeedAsync(app.Services, app.Configuration);
```

## Lưu Ý Quan Trọng

1. **In-Memory Stores**: Chỉ dùng cho development/testing
2. **Custom Stores**: Cần implement đầy đủ các interface methods
3. **Certificates**: Vẫn cần cấu hình certificates cho production
4. **Scopes và Clients**: Vẫn cần seed dữ liệu ban đầu

## Ví Dụ Sử Dụng

### Development Environment
```csharp
// Program.cs
builder.Services.AddUserAccessAuthenticationInMemory(builder.Configuration);

// Seed
await OpenIddictInMemorySeeder.SeedAsync(app.Services, app.Configuration);
```

### Production Environment
```csharp
// Program.cs
builder.Services.AddUserAccessAuthenticationCustomStores(builder.Configuration);

// Implement custom stores với database của bạn
```

## Troubleshooting

1. **Lỗi "Store not found"**: Đảm bảo đã register custom stores
2. **Lỗi "Scope not found"**: Kiểm tra seeder có chạy đúng không
3. **Lỗi "Client not found"**: Kiểm tra client ID và configuration
