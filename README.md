# Project API - Authentication & User Session Management

Backend API được xây dựng bằng ASP.NET Core 8, tập trung vào quy trình xác thực người dùng và quản lý phiên đăng nhập theo thiết bị. Dự án được tổ chức theo hướng tách lớp giữa Application, Business và DataAccess, giúp phân tách phần HTTP endpoint, nghiệp vụ và truy cập dữ liệu.

## Điểm nổi bật

- Đăng ký tài khoản và kiểm tra email đã tồn tại.
- Mã hóa mật khẩu bằng BCrypt trước khi lưu vào cơ sở dữ liệu.
- Đăng nhập bằng JWT access token.
- Cấp và xoay vòng refresh token; refresh token được lưu trong HttpOnly cookie.
- Theo dõi session theo `DeviceID`, IP và User-Agent.
- Thu hồi session cũ khi đăng nhập lại hoặc khi refresh token hết hạn/sai thiết bị.
- Đăng xuất và thu hồi refresh token.
- Upload avatar cho tài khoản đã xác thực, đồng thời xóa avatar cũ.
- Swagger/OpenAPI hỗ trợ kiểm tra API trong môi trường Development.

## Công nghệ sử dụng

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- JWT Bearer Authentication
- BCrypt.Net
- Swagger / OpenAPI

## Kiến trúc solution

```text
Application/   HTTP pipeline, controller và cấu hình ứng dụng
Business/      Interface, service nghiệp vụ, JWT và middleware
DataAccess/    Entity, DbContext và cấu hình truy cập dữ liệu
```

## API chính

| Method | Endpoint | Mô tả |
| --- | --- | --- |
| `POST` | `/Home/Register` | Tạo tài khoản mới |
| `POST` | `/Home/Login` | Đăng nhập và nhận access token |
| `POST` | `/Home/Refresh_Token` | Cấp access token mới từ refresh token |
| `POST` | `/Home/Logout` | Thu hồi session hiện tại |
| `POST` | `/Home/Post_Avatar` | Upload avatar, yêu cầu JWT |

## Chạy project

### Yêu cầu

- .NET SDK 8.0+
- SQL Server

### Cấu hình

Đặt connection string và các thiết lập JWT trong `Application/appsettings.Development.json`. Không commit secret thật lên repository; nên dùng User Secrets hoặc biến môi trường khi triển khai.

### Khởi động

```bash
dotnet restore
dotnet build
dotnet run --project Application/Application.csproj
```

Khi chạy ở môi trường Development, mở URL Swagger được hiển thị trong terminal để xem và thử các endpoint.

## Mục tiêu học tập

Dự án thể hiện khả năng xây dựng một Web API có xác thực, làm việc với cơ sở dữ liệu quan hệ, mã hóa mật khẩu, quản lý vòng đời token và tổ chức code theo nhiều project. Đây là nền tảng để tiếp tục bổ sung validation bằng DTO, migration, phân quyền, logging, test tự động và triển khai production.


