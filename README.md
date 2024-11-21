# 🌍 **Website Đặt Tour Du Lịch** 🌍

## 🎯 Mô Tả Dự Án

Website Đặt Tour Du Lịch là một nền tảng trực tuyến giúp người dùng dễ dàng tìm kiếm và đặt tour du lịch, mang lại trải nghiệm người dùng tuyệt vời với các tính năng hiện đại như giỏ hàng, thanh toán qua MOMO, và tích hợp các dịch vụ đăng nhập từ các nền tảng phổ biến như **Google**, **Facebook**, và **GitHub**.

Ngoài ra, hệ thống còn cung cấp thông tin thời tiết theo thời gian thực từ **OpenWeather**, giúp người dùng dễ dàng cập nhật dự báo thời tiết trong quá trình lên kế hoạch cho chuyến đi. 

### 🚀 **Tính Năng Chính**:
- **Đăng nhập nhanh chóng** qua Google, Facebook, GitHub.
- **Giỏ hàng và thanh toán** qua **MOMO**.
- **Thống kê dữ liệu** thông qua **Chart.js**.
- **Dự báo thời tiết** theo thời gian thực từ **OpenWeather**.
- **Hỗ trợ đa ngôn ngữ** cho người dùng toàn cầu.
- **Lịch sử giao dịch** giúp khách hàng theo dõi các giao dịch của mình.

---

## ⚙️ **Công Nghệ Sử Dụng**

### Backend:
- **Spring Boot**: Framework chính để phát triển các API và xử lý logic nghiệp vụ.
- **MySQL**: Cơ sở dữ liệu để lưu trữ thông tin người dùng, sản phẩm, và giao dịch.
- **JWT (JSON Web Token)**: Cơ chế bảo mật để xác thực người dùng và các yêu cầu API.
- **MOMO API**: Tích hợp phương thức thanh toán qua MOMO.

### Frontend:
- **Thymeleaf**: Template engine để render HTML trong Spring Boot.
- **HTML5, CSS3, JavaScript**: Được sử dụng để xây dựng giao diện người dùng.
- **React.js**: Cho các phần giao diện động và trải nghiệm người dùng mượt mà.
- **Chart.js**: Dùng để tạo biểu đồ thống kê trực quan.

### API và Tích Hợp:
- **Google, Facebook, GitHub OAuth**: Đăng nhập nhanh chóng và an toàn.
- **OpenWeather API**: Dự báo thời tiết theo thời gian thực.

---

## 📋 **Chức Năng CRUD**

Các chức năng chính của website bao gồm:
- **CRUD Sản Phẩm Tour**: Tạo, đọc, cập nhật, và xóa các sản phẩm tour du lịch.
- **CRUD Danh Mục**: Quản lý danh mục sản phẩm tour.
  
---

## 💳 **Thanh Toán và Giỏ Hàng**

- **Giỏ hàng**: Người dùng có thể thêm các tour du lịch vào giỏ hàng và tiến hành thanh toán.
- **Thanh toán qua MOMO**: Tích hợp thanh toán qua ví điện tử MOMO để người dùng dễ dàng thực hiện giao dịch.

---

## 📊 **Thống Kê và Biểu Đồ**

- **Biểu đồ thống kê**: Sử dụng **Chart.js** để tạo ra các biểu đồ trực quan về số liệu giao dịch, doanh thu, và các thống kê khác.

---

## 🌦️ **Dự Báo Thời Tiết**

- **OpenWeather API**: Tích hợp để cung cấp thông tin thời tiết theo thời gian thực cho người dùng, giúp họ lên kế hoạch cho chuyến đi một cách chính xác.

---

## 🌍 **Hỗ Trợ Đa Ngôn Ngữ**

- Hệ thống hỗ trợ nhiều ngôn ngữ để phục vụ người dùng trên toàn cầu, từ tiếng Việt, tiếng Anh đến các ngôn ngữ khác.

---

## 📅 **Lịch Sử Giao Dịch**

- Người dùng có thể kiểm tra lịch sử giao dịch của mình để theo dõi tất cả các lần đặt tour và thanh toán.

---

## 🛠️ **Cài Đặt và Chạy Dự Án**

### Yêu Cầu Hệ Thống:
- **Java 11+**
- **Spring Boot 2.x**
- **MySQL 5.x+**
- **Node.js** (nếu sử dụng frontend React)

### Các Bước Cài Đặt:
1. **Clone repository**:
   ```bash
   git clone https://github.com/TranNguyenVu74123/WebsiteDatTourDuLich.git
