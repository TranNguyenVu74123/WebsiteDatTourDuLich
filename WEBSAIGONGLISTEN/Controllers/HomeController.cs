using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBSAIGONGLISTEN.Extensions;
using WEBSAIGONGLISTEN.Models;
using WEBSAIGONGLISTEN.Repositories;
using WEBSAIGONGLISTEN.Services;

namespace WEBSAIGONGLISTEN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;// day la hung
        private readonly IMomoService _momoService;
        private readonly ITourDayRepository _tourdayRepository;
        private readonly int _pageSize = 8;
        public HomeController(IProductRepository productRepository, ApplicationDbContext context, IMomoService momoService, ITourDayRepository tourdayRepository) // cho nay la hung
        {
            _context = context; // cho nay la hung
            _productRepository = productRepository;
            _momoService = momoService;
            _tourdayRepository = tourdayRepository;
        }

        //public async Task<IActionResult> Index(int page = 1, DateTime? startDate = null, DateTime? endDate = null, int? categoryId = null, string query = null)
        //{
          
        //    // Lưu trữ giá trị tìm kiếm vào ViewData để hiển thị trong form
        //    ViewData["Query"] = query;
        //    ViewData["CategoryId"] = categoryId;

        //    // Lấy danh sách các category để hiển thị trong dropdown
        //    var categories = await _context.Categories.ToListAsync();
        //    ViewData["Categories"] = categories;

        //    if (query == null)
        //    {
        //        return View(new List<Product>());
        //    }

        //    query = query.Trim();
        //    var keywords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //    IQueryable<Product> queryableProducts = _context.Products;

        //    // Lọc theo Category nếu có
        //    if (categoryId.HasValue && categoryId.Value > 0)
        //    {
        //        queryableProducts = queryableProducts.Where(p => p.CategoryId == categoryId.Value);
        //    }

        //    // Thêm logic lọc theo ngày bắt đầu và ngày kết thúc nếu có
        //    if (startDate.HasValue)
        //    {
        //        queryableProducts = queryableProducts.Where(p => p.StartDate >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        queryableProducts = queryableProducts.Where(p => p.EndDate <= endDate.Value);
        //    }

        //    queryableProducts = queryableProducts.Where(p => keywords.Any(keyword => p.Name.Contains(keyword)));

        //    int totalProducts = await queryableProducts.CountAsync();
        //    int totalPages = (int)Math.Ceiling((double)totalProducts / _pageSize);

        //    var paginatedProducts = await queryableProducts
        //         .Skip((page - 1) * _pageSize)
        //         .Take(_pageSize)
        //         .ToListAsync();

        //    // Cập nhật ViewData để hiển thị thông tin phân trang và các ngày
        //    ViewData["TotalPages"] = totalPages;
        //    ViewData["CurrentPage"] = page;
        //    ViewData["StartDate"] = startDate ?? DateTime.Now; // Ngày bắt đầu
        //    ViewData["EndDate"] = endDate ?? DateTime.Now.AddDays(7); // Ngày kết thúc
        //    ViewData["CategoryId"] = categoryId; // Thêm CategoryId vào ViewData để giữ giá trị đã chọn
        //    ViewData["Keyword"] = query; // Thêm từ khóa vào ViewData để giữ giá trị đã nhập

        //    return View(paginatedProducts);
        //}

        public async Task<IActionResult> Index(int page = 1, DateTime? startDate = null, DateTime? endDate = null, int? categoryId = null, string query = null)
        {
            // Lưu trữ giá trị tìm kiếm vào ViewData để hiển thị trong form
            ViewData["Query"] = query;
            ViewData["CategoryId"] = categoryId;

            // Lấy danh sách các category để hiển thị trong dropdown
            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = categories;

            // Nếu không có từ khóa và các bộ lọc khác, hiển thị tất cả sản phẩm
            IQueryable<Product> queryableProducts = _context.Products;

            // Lọc theo Category nếu có
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                queryableProducts = queryableProducts.Where(p => p.CategoryId == categoryId.Value);
            }

            // Thêm logic lọc theo ngày bắt đầu và ngày kết thúc nếu có
            if (startDate.HasValue)
            {
                queryableProducts = queryableProducts.Where(p => p.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                queryableProducts = queryableProducts.Where(p => p.EndDate <= endDate.Value);
            }

            // Nếu có từ khóa, thực hiện lọc theo từ khóa
            if (!string.IsNullOrEmpty(query))
            {
                query = query.Trim();
                var keywords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                queryableProducts = queryableProducts.Where(p => keywords.Any(keyword => p.Name.Contains(keyword)));
            }

            int totalProducts = await queryableProducts.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalProducts / _pageSize);

            var paginatedProducts = await queryableProducts
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            // Cập nhật ViewData để hiển thị thông tin phân trang và các ngày
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;
            ViewData["StartDate"] = startDate ?? DateTime.Now;
            ViewData["EndDate"] = endDate ?? DateTime.Now.AddDays(7);
            ViewData["CategoryId"] = categoryId;
            ViewData["Keyword"] = query;

            return View(paginatedProducts);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult weatherForecast()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Tour()
        {
            var tours = _context.Products.ToList(); // Lấy danh sách tour từ cơ sở dữ liệu
            return View(tours);
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int? productId, int? quantity)
        {
            if (productId == null)
            {
                return NotFound();
            }
            //var product = await _productRepository.GetByIdAsync(productId.Value);

            var product = await _context.Products
           .FirstOrDefaultAsync(p => p.Id == productId);
            var tourdays = await _tourdayRepository.GetTourdaysByProductIdAsync(productId.Value);

            if (product == null)
            {
                return NotFound();
            }


            var viewModel = new ProductDisplayViewModel
            {
                Product = product,
                TourDays = tourdays
            };
            // Xử lý thêm cho quantity nếu cần
            return View(viewModel);
        }

        public async Task<IActionResult> Search(string query, DateTime? departureDate = null, DateTime? returnDate = null, int page = 1)
        {
            if (query == null)
            {
                return View("Search", new List<Product>());
            }
            query = query.Trim();
            var keywords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            IQueryable<Product> queryableProducts = _context.Products;
            queryableProducts = queryableProducts.Where(p => keywords.Any(keyword => p.Name.Contains(keyword)));
            int totalProducts = await queryableProducts.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalProducts / _pageSize);
            var paginatedProducts = await queryableProducts
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();
            ViewData["DepartureDate"] = departureDate ?? DateTime.Now;
            ViewData["ReturnDate"] = returnDate ?? DateTime.Now.AddDays(7);
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;
            return View("Search", paginatedProducts);
        }

        public IActionResult Pay()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống
                return RedirectToAction("Index");
            }

            var totalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

            var order = new Order
            {
                TotalPrice = (double)totalPrice
            };

            return View(order);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(Transaction model)
        {
            var response = await _momoService.CreatePaymentAsync(model);

            if (!string.IsNullOrEmpty(response.PayUrl))
            {
                return Redirect(response.PayUrl);
            }
            else
            {
                return RedirectToAction("Error", "ShoppingCart");
            }
        }

        //[HttpGet]
        //public IActionResult PaymentCallBack()
        //{
        //    var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
        //    return View(response);
        //}

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }

        // Phương thức để gửi bình luận
        [HttpPost]
        public async Task<IActionResult> SubmitReview(int productId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new { success = false, message = "Nội dung bình luận không thể trống." });
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            var comment = new Comment
            {
                ProductId = productId,
                UserName = User.Identity.Name,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

    }
}