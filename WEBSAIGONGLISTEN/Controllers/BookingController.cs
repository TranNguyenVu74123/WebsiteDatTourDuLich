using Microsoft.AspNetCore.Mvc;
using WEBSAIGONGLISTEN.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WEBSAIGONGLISTEN.Services;
using WEBSAIGONGLISTEN.Extensions;

namespace WEBSAIGONGLISTEN.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMomoService _momoService;
        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMomoService momoService)
        {
            _context = context;
            _userManager = userManager;
            _momoService = momoService;
        }

        //[HttpPost]
        //public IActionResult BookTour(int productId, int quantityUnder2, int quantity2To10, int quantityAbove10)
        //{
        //    var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        //    if (product == null)
        //    {
        //        TempData["ErrorMessage"] = "Product not found";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Calculate total price
        //    var totalPrice = (quantityUnder2 * product.PriceUnder2) +
        //                     (quantity2To10 * product.Price2To10) +
        //                     (quantityAbove10 * product.PriceAbove10);

        //    // Create new booking
        //    var booking = new Booking
        //    {
        //        ProductId = productId,
        //        QuantityUnder2 = quantityUnder2,
        //        Quantity2To10 = quantity2To10,
        //        QuantityAbove10 = quantityAbove10,
        //        TotalPrice = totalPrice,
        //        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) // Hoặc cách khác để lấy UserId nếu người dùng đã đăng nhập
        //    };

        //    _context.Bookings.Add(booking);
        //    _context.SaveChanges();


        //    return RedirectToAction("Confirmation", new { id = booking.Id });
        //}

        [HttpPost]
        public IActionResult BookTour(int productId, int quantityUnder2, int quantity2To10, int quantityAbove10)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found";
                return RedirectToAction("Display", "Home");
            }

            // Kiểm tra số lượng còn lại của sản phẩm
            int totalRequestedQuantity = quantityUnder2 + quantity2To10 + quantityAbove10;
            if (product == null || totalRequestedQuantity > product.Quantity)
            {
                TempData["ErrorMessage"] = "Số lượng không đủ cho yêu cầu của bạn. Vui lòng giảm số lượng hoặc chọn chuyến đi phẩm khác.";
                //return RedirectToAction("Display", "Home", new { id = productId, quantity = totalRequestedQuantity });
                // Chuyển hướng đến Home/Display sau khi hoàn tất đặt tour
                return RedirectToAction("Display", "Home", new { productId = productId, quantity = totalRequestedQuantity });
            }

            // Lấy UserId của người dùng đã đăng nhập
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kiểm tra xem người dùng có đăng nhập hay không
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt tour.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Tính tổng tiền
            var totalPrice = (quantityUnder2 * product.PriceUnder2) +
                             (quantity2To10 * product.Price2To10) +
                             (quantityAbove10 * product.PriceAbove10);

            // Tạo booking mới
            var booking = new Booking
            {
                ProductId = productId,
                QuantityUnder2 = quantityUnder2,
                Quantity2To10 = quantity2To10,
                QuantityAbove10 = quantityAbove10,
                TotalPrice = totalPrice,
                UserId = userId // Gán UserId cho booking
            };

            // Giảm số lượng tồn kho của sản phẩm
            product.Quantity -= totalRequestedQuantity;

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Confirmation", new { id = booking.Id });
        }


        // Phương thức xác nhận thanh toán
        [HttpGet("Confirmation/{id}")]
        public IActionResult Confirmation(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.Product)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();  // Trả về lỗi 404 nếu không tìm thấy booking
            }

            return View(booking);  // Trả về view nếu tìm thấy booking
        }


        [HttpPost]
        public IActionResult CompletePayment(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found";
                return RedirectToAction("BookingHistory");
            }

            // Check if status is already paid to prevent redundant updates
            if (booking.Status == "Paid")
            {
                TempData["ErrorMessage"] = "Payment already completed";
                return RedirectToAction("PaymentSuccess");
            }

            // Update status to "Paid"
            booking.Status = "Paid";  // Ensure that Status is a string or update comparison if it's an int
            _context.SaveChanges();

            return RedirectToAction("PaymentSuccess");
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }

        public async Task<IActionResult> BookingHistory()
        {
            // Lấy thông tin người dùng hiện tại (đã đăng nhập)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Hoặc dùng User.Identity.Name nếu cần

            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Lấy danh sách booking của người dùng hiện tại
            var bookings = await _context.Bookings
                .Include(b => b.ApplicationUser) // Bao gồm thông tin người dùng
                .Include(b => b.Product) // Bao gồm thông tin sản phẩm (tour)
                .Where(b => b.ApplicationUser.Id == userId) // Lọc theo userId
                .ToListAsync();

            return View(bookings);
        }

        public BookingStatus GetTourStatus(DateTime startDate, DateTime endDate)
        {
            var today = DateTime.Today;

            if (today > endDate)
            {
                return BookingStatus.DaDi;  // Finished
            }
            else if (today < startDate)
            {
                return BookingStatus.DaHuy;  // Canceled or not started yet
            }
            else
            {
                return BookingStatus.DangDi;  // Ongoing
            }
        }
        public IActionResult Pay(int productId, int quantityUnder2, int quantity2To10, int quantityAbove10)
        {
            var product = _context.Products.Find(productId);

            // Tính toán tổng giá trị đơn hàng
            decimal totalPrice = 0;
            totalPrice += quantityUnder2 * product.PriceUnder2;
            totalPrice += quantity2To10 * product.Price2To10;
            totalPrice += quantityAbove10 * product.PriceAbove10;

            // Tạo đơn hàng mới
            var booking = new Booking
            {
                ProductId = productId,
                QuantityUnder2 = quantityUnder2,
                Quantity2To10 = quantity2To10,
                QuantityAbove10 = quantityAbove10,
                TotalPrice = totalPrice,
                BookingDate = DateTime.Now
            };

            // Hiển thị view thanh toán với đơn hàng
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(Transaction model)
        {
            var response = await _momoService.CreatePaymentAsync(model);

            if (!string.IsNullOrEmpty(response.PayUrl))
            {
                return Redirect(response.PayUrl); // Điều hướng tới URL thanh toán
            }
            else
            {
                TempData["ErrorMessage"] = "Lỗi khi tạo đường dẫn thanh toán.";
                return RedirectToAction("Error", "Booking");
            }
        }

        /*[HttpPost]
        public IActionResult CancelBooking(int id, string cancellationReason)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái và lưu lý do hủy
            booking.Status = "Canceled";
            booking.CancellationReason = cancellationReason;
            booking.CancellationDate = DateTime.Now;

            _context.Bookings.Update(booking);
            _context.SaveChanges();

            return RedirectToAction("BookingHistory"); // Hoặc điều hướng đến trang cần thiết
        }*/

        [HttpPost]
        public IActionResult CancelBooking(int id, string cancellationReason)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null || booking.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Không thể hủy booking này.";
                return RedirectToAction("BookingHistory");
            }

            booking.Status = "Awaiting Admin Confirmation";
            booking.CancellationReason = cancellationReason;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Yêu cầu hủy tour đã được gửi đến quản trị viên.";
            return RedirectToAction("BookingHistory");
        }

      


        [HttpGet]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);

            if (response != null) // Kiểm tra thanh toán thành công
            {
                // Lấy thông tin đơn hàng (có thể từ Session hoặc khác)
                var booking = HttpContext.Session.GetObjectFromJson<Booking>("");  // Giả sử bạn lưu Booking vào Session

                if (booking != null)
                {
                    var product = await _context.Products.FindAsync(booking.ProductId);

                    if (product != null)
                    {
                        // Cập nhật lại số lượng sản phẩm sau khi thanh toán
                        product.Quantity -= (booking.QuantityUnder2 + booking.Quantity2To10 + booking.QuantityAbove10);

                        // Lưu thay đổi vào cơ sở dữ liệu
                        await _context.SaveChangesAsync();
                    }

                    // Hiển thị thông báo thanh toán thành công
                    TempData["SuccessMessage"] = "Thanh toán thành công!";
                    return RedirectToAction("Success");
                }
            }
            else
            {
                // Xử lý khi thanh toán thất bại
                TempData["ErrorMessage"] = "Thanh toán không thành công.";
                return RedirectToAction("Error", "Booking");
            }

            return View(response);
        }


    }
}
