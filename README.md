# Project API - Authentication, Session Management & Product Backend

Dự án này là một backend ASP.NET Core 8 được xây dựng cho một hệ thống thương mại điện tử đơn giản, tập trung vào xác thực người dùng, quản lý session và phục vụ dữ liệu sản phẩm cho giao diện storefront. Đây là một project backend thực tế, tích hợp nhiều thành phần quan trọng như đăng nhập, refresh token, kiểm soát thiết bị, avatar user và API danh mục/sản phẩm.

## Mục tiêu dự án

- Xây dựng hệ thống xác thực người dùng an toàn
- Quản lý phiên đăng nhập theo từng thiết bị và trình duyệt
- Cấp phát và làm mới JWT access token bằng refresh token
- Tạo API dữ liệu sản phẩm cho giao diện web/mobile
- Tách rõ tầng controller, business logic và data access để code dễ bảo trì
- Làm nền tảng cho các dự án backend có thể mở rộng thêm quyền truy cập, quản trị, giỏ hàng và thanh toán

## Tính năng chính

- Đăng ký tài khoản mới
- Kiểm tra email đã tồn tại trước khi tạo user
- Mã hóa mật khẩu bằng BCrypt
- Đăng nhập với JWT access token
- Làm mới token bằng refresh token trong cookie HttpOnly
- Giới hạn session theo `DeviceID`, `User-Agent` và IP
- Thu hồi session cũ khi user đăng nhập lại hoặc token không hợp lệ
- Đăng xuất và hủy session hiện tại
- Upload avatar cho tài khoản đã xác thực
- API lấy danh sách sản phẩm và chi tiết sản phẩm
- Sử dụng cache phân tán để tăng hiệu suất truy vấn sản phẩm
- Tích hợp Swagger/OpenAPI cho kiểm tra API trong môi trường phát triển

## Công nghệ sử dụng

- ASP.NET Core 8
- C#
- SQL Server
- Entity Framework Core
- Dapper
- JWT Bearer Authentication
- BCrypt.Net
- Redis / Distributed Cache
- Swagger / OpenAPI

## Kiến trúc dự án

```text
Application/   Chứa controllers, routing, middleware và cấu hình API
Business/      Chứa service nghiệp vụ, JWT, auth, session và logic sản phẩm
DataAccess/    Chứa model, DbContext, entity và kết nối dữ liệu
```

## Kỹ năng thể hiện trong dự án

- Backend API development
- Authentication & Authorization
- JWT and refresh token flow
- User session and device tracking
- File upload and avatar handling
- Product API design
- Caching for performance optimization
- Clean layered architecture
- Database interaction with EF Core and Dapper

## Giá trị của dự án

Dự án này thể hiện khả năng xây dựng một backend thực tế cho hệ thống thương mại điện tử, với các vấn đề bảo mật, quản lý session và API dữ liệu quan trọng. Nó phù hợp để giới thiệu trong CV hoặc portfolio khi muốn chứng minh kỹ năng về ASP.NET Core, JWT, bảo mật API, thiết kế backend và xử lý dữ liệu sản phẩm trong môi trường thực tế.


