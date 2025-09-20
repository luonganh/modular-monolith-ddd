# Hướng Dẫn Cấu Hình OpenIddict Không Sử Dụng EF Core

## Tổng Quan

Tài liệu này hướng dẫn cách cấu hình OpenIddict mà không sử dụng Entity Framework Core trong dự án Modular Monolith DDD.

## Các Phương Pháp Cấu Hình

### 1. In-Memory Stores (Khuyến nghị cho Development)

**Khi nào sử dụng:**
- Development và testing
- Prototype và demo
- Khi không cần lưu trữ dữ liệu persistent

**Ưu điểm:**
- Setup đơn giản, không cần database
- Performance cao
- Không cần cấu hình connection string

**Nhược điểm:**
- Dữ liệu bị mất khi restart application
- Không phù hợp cho production

**Cách sử dụng:**

1. **Thay thế cấu hình trong Program.cs:**
```csharp
// Thay vì
builder.Services.AddUserAccessAuthentication(builder.Configuration);

// Sử dụng
builder.Services.AddUserAccessAuthenticationInMemory(builder.Configuration);
```

2. **Cập nhật seeder:**
```csharp
// Thay vì
await OpenIddictSeeder.SeedAsync(sp, app.Configuration);

// Sử dụng
await OpenIddictInMemorySeeder.SeedAsync(app.Services, app.Configuration);
```

3. **Loại bỏ EF Core dependencies:**
```csharp
// Xóa các dòng này
builder.Services.AddDbContextFactory<UserAccessContext>(...);
builder.Services.AddScoped<DbContext>(...);
```

### 2. Custom Stores (Cho Production)

**Khi nào sử dụng:**
- Production environment
- Khi cần tích hợp với database khác (MongoDB, Redis, etc.)
- Khi cần control hoàn toàn việc lưu trữ dữ liệu

**Ưu điểm:**
- Dữ liệu persistent
- Linh hoạt trong việc chọn storage
- Có thể tối ưu performance

**Nhược điểm:**
- Phức tạp hơn, cần implement nhiều interfaces
- Cần quản lý connection và transaction

**Cách sử dụng:**

1. **Implement các Custom Stores:**
```csharp
public class CustomApplicationStore : IOpenIddictApplicationStore<OpenIddictApplication>
{
    // Implement tất cả methods từ interface
}

public class CustomScopeStore : IOpenIddictScopeStore<OpenIddictScope>
{
    // Implement tất cả methods từ interface
}

public class CustomTokenStore : IOpenIddictTokenStore<OpenIddictToken>
{
    // Implement tất cả methods từ interface
}
```

2. **Cấu hình trong Program.cs:**
```csharp
builder.Services.AddUserAccessAuthenticationCustomStores(builder.Configuration);
```

## So Sánh Các Phương Pháp

| Tính năng | EF Core | In-Memory | Custom Stores |
|-----------|---------|-----------|---------------|
| **Setup** | Phức tạp | Đơn giản | Rất phức tạp |
| **Performance** | Tốt | Rất tốt | Tùy implementation |
| **Persistence** | Có | Không | Có |
| **Production Ready** | Có | Không | Có |
| **Flexibility** | Trung bình | Thấp | Cao |
| **Maintenance** | Thấp | Thấp | Cao |

## Migration Từ EF Core

### Bước 1: Backup cấu hình hiện tại
```bash
# Backup file Program.cs hiện tại
cp Program.cs Program_EFCore.cs
```

### Bước 2: Thay đổi cấu hình OpenIddict
```csharp
// Trong OpenIddictRegistration.cs, thay đổi:
o.UseEntityFrameworkCore().UseDbContext<UserAccessContext>();

// Thành:
o.UseInMemory(); // hoặc
o.UseCustomStores().ReplaceDefaultApplicationStore<CustomApplicationStore>();
```

### Bước 3: Cập nhật Program.cs
```csharp
// Thay đổi method call
builder.Services.AddUserAccessAuthenticationInMemory(builder.Configuration);

// Cập nhật seeder
await OpenIddictInMemorySeeder.SeedAsync(app.Services, app.Configuration);
```

### Bước 4: Loại bỏ EF Core dependencies
```csharp
// Xóa các dòng này
builder.Services.AddDbContextFactory<UserAccessContext>(...);
builder.Services.AddScoped<DbContext>(...);
```

## Files Đã Tạo

### 1. In-Memory Configuration
- `OpenIddictInMemoryRegistration.cs` - Cấu hình OpenIddict với In-Memory stores
- `OpenIddictInMemorySeeder.cs` - Seeder cho In-Memory stores
- `ProgramInMemory.cs` - Ví dụ Program.cs không sử dụng EF Core

### 2. Custom Stores Configuration
- `OpenIddictCustomStoreRegistration.cs` - Cấu hình với Custom Stores
- Các class `CustomApplicationStore`, `CustomScopeStore`, `CustomTokenStore`

### 3. Documentation
- `README_OpenIddict_NoEF.md` - Tài liệu chi tiết
- `OpenIddict_NoEF_Guide.md` - Hướng dẫn này

## Ví Dụ Sử Dụng

### Development Environment
```csharp
// Program.cs
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Cấu hình In-Memory stores
        builder.Services.AddUserAccessAuthenticationInMemory(builder.Configuration);
        
        var app = builder.Build();
        
        // Seed dữ liệu
        await OpenIddictInMemorySeeder.SeedAsync(app.Services, app.Configuration);
        
        app.Run();
    }
}
```

### Production Environment
```csharp
// Program.cs
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Cấu hình Custom Stores
        builder.Services.AddUserAccessAuthenticationCustomStores(builder.Configuration);
        
        var app = builder.Build();
        
        app.Run();
    }
}
```

## Troubleshooting

### Lỗi thường gặp:

1. **"Store not found"**
   - Đảm bảo đã register custom stores
   - Kiểm tra cấu hình UseCustomStores()

2. **"Scope not found"**
   - Kiểm tra seeder có chạy đúng không
   - Verify scope name trong configuration

3. **"Client not found"**
   - Kiểm tra client ID
   - Verify client configuration trong seeder

4. **"Token validation failed"**
   - Kiểm tra certificate configuration
   - Verify token store implementation

### Debug Tips:

1. **Enable logging:**
```csharp
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
```

2. **Check OpenIddict configuration:**
```csharp
// Thêm vào Program.cs để debug
app.MapGet("/debug/openiddict", (IOpenIddictApplicationManager appManager) => 
{
    // Debug code here
});
```

## Kết Luận

- **Development/Testing**: Sử dụng In-Memory stores
- **Production**: Sử dụng Custom Stores hoặc quay lại EF Core
- **Migration**: Thực hiện từng bước, test kỹ lưỡng
- **Performance**: Custom Stores có thể tối ưu hơn EF Core nếu implement đúng

Chọn phương pháp phù hợp với nhu cầu và môi trường của bạn!
