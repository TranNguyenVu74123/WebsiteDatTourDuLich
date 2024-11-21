# 🌍 **Website Đặt Tour Du Lịch** 🌍

## 🎯 Mô Tả Dự Án

Website Đặt Tour Du Lịch là nền tảng trực tuyến giúp người dùng dễ dàng tìm kiếm và đặt các tour du lịch. Dự án tích hợp nhiều tính năng như giỏ hàng, thanh toán qua MOMO, đăng nhập nhanh với Google, Facebook và GitHub, cùng với việc cung cấp thông tin thời tiết theo thời gian thực từ **OpenWeather**.

Ngoài ra, hệ thống còn cho phép người dùng kiểm tra lịch sử giao dịch của mình, giúp tối ưu hóa trải nghiệm người dùng.

### 🚀 **Tính Năng Chính**:
- **Đăng nhập nhanh chóng** qua **Google**, **Facebook**, và **GitHub**.
- **Giỏ hàng và thanh toán** qua **MOMO**.
- **Thống kê dữ liệu** trực quan với **Chart.js**.
- **Dự báo thời tiết** theo thời gian thực từ **OpenWeather**.
- **Hỗ trợ đa ngôn ngữ** cho người dùng toàn cầu.
- **Lịch sử giao dịch** để khách hàng theo dõi các giao dịch của mình.

---

## ⚙️ **Công Nghệ Sử Dụng**

### Backend:
- **ASP.NET Core**: Framework chính để phát triển API và xử lý logic nghiệp vụ.
- **SQL Server**: Cơ sở dữ liệu để lưu trữ thông tin người dùng, sản phẩm và giao dịch.
- **JWT (JSON Web Token)**: Cơ chế bảo mật để xác thực người dùng và các yêu cầu API.
- **MOMO API**: Tích hợp thanh toán qua ví điện tử MOMO.

### Frontend:
- **HTML5, CSS3, JavaScript**: Các công nghệ cơ bản để xây dựng giao diện người dùng.
- **Chart.js**: Dùng để tạo biểu đồ thống kê trực quan.

### API và Tích Hợp:
- **Google, Facebook, GitHub OAuth**: Đăng nhập nhanh chóng và an toàn.
- **OpenWeather API**: Dự báo thời tiết theo thời gian thực.

---

## 📋 **Chức Năng CRUD**

Website hỗ trợ các chức năng CRUD cho:
- **CRUD Sản Phẩm Tour**: Tạo, đọc, cập nhật, xóa các sản phẩm tour du lịch.
- **CRUD Danh Mục**: Quản lý các danh mục tour.

---

## 💳 **Giỏ Hàng và Thanh Toán**

- **Giỏ hàng**: Người dùng có thể thêm các tour vào giỏ hàng và tiến hành thanh toán.
- **Thanh toán qua MOMO**: Tích hợp MOMO API để thực hiện giao dịch thanh toán nhanh chóng.

---

## 📊 **Thống Kê và Biểu Đồ**

- **Biểu đồ thống kê**: Sử dụng **Chart.js** để tạo các biểu đồ trực quan về số liệu giao dịch và doanh thu.

---

## 🌦️ **Dự Báo Thời Tiết**

- **OpenWeather API**: Cung cấp thông tin thời tiết theo thời gian thực, giúp người dùng cập nhật tình hình thời tiết để lên kế hoạch cho chuyến đi.

---

## 🌍 **Hỗ Trợ Đa Ngôn Ngữ**

Hệ thống hỗ trợ nhiều ngôn ngữ để phục vụ người dùng toàn cầu, từ tiếng Việt đến tiếng Anh và các ngôn ngữ khác.

---

## 📅 **Lịch Sử Giao Dịch**

Người dùng có thể kiểm tra lịch sử giao dịch của mình để theo dõi tất cả các lần đặt tour và thanh toán đã thực hiện.

---

## 🛠️ **Cài Đặt và Chạy Dự Án**

### Yêu Cầu Hệ Thống:
- **.NET 6+** (hoặc .NET 7)
- **SQL Server 2019+**
- **Node.js** (nếu sử dụng frontend React)

### Các Bước Cài Đặt:

1. **Clone repository**:
   ```bash
   git clone https://github.com/TranNguyenVu74123/WebsiteDatTourDuLich.git
