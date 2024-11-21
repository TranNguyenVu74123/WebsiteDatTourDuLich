using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEBSAIGONGLISTEN.Models;
using WEBSAIGONGLISTEN.Repositories;

namespace WEBSAIGONGLISTEN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
    public class TourDayController : Controller
    {
        private readonly ITourDayRepository _tourDayRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public TourDayController(ITourDayRepository tourDayRepository, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _tourDayRepository = tourDayRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // Danh sách các ngày tour
        public async Task<IActionResult> Index()
        {
            var tourDays = await _tourDayRepository.GetAllTourDaysAsync();

            // Lấy danh sách sản phẩm để truyền vào ViewBag nếu cần hiển thị trong View
            var products = await _productRepository.GetAllAsync();
            ViewBag.Products = products; // Truyền danh sách sản phẩm đến ViewBag nếu cần
            return View(tourDays);
        }

        [HttpGet("tourday/details/{id}")]
        // Hiển thị chi tiết của một ngày tour
        public async Task<IActionResult> Details(int id)
        {
            var tourDay = await _tourDayRepository.GetTourDayByIdAsync(id);
            if (tourDay == null)
            {
                return NotFound();
            }

            // Lấy danh sách sản phẩm để có thể hiển thị nếu cần trong View
            var products = await _productRepository.GetAllAsync();
            ViewBag.Products = products;
            return View(tourDay);
        }

        // Hiển thị form thêm ngày tour mới
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync(); // Lấy danh sách sản phẩm
            ViewBag.Categories = new SelectList(categories, "Id", "Name"); // Truyền danh sách danh mục đến View
            ViewBag.Products = new SelectList(products, "Id", "Name"); // Truyền danh sách sản phẩm đến View
            return View();
        }

        // Thêm ngày tour mới vào cơ sở dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourDay tourDay, IFormFile
        imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    tourDay.ImageUrl = await SaveImage(imageUrl);

                }

                await _tourDayRepository.AddTourDayAsync(tourDay);
                return RedirectToAction(nameof(Index));
            }

            // Truyền lại danh sách danh mục và sản phẩm nếu model không hợp lệ
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return View(tourDay);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); //Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }

        // Hiển thị form cập nhật ngày tour
        [HttpGet("tourday/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var tourDay = await _tourDayRepository.GetTourDayByIdAsync(id);
            if (tourDay == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync(); // Lấy danh sách sản phẩm
            ViewBag.Categories = new SelectList(categories, "Id", "Name"); // Truyền danh sách danh mục đến View
            ViewBag.Products = new SelectList(products, "Id", "Name"); // Truyền danh sách sản phẩm đến View
            return View(tourDay);
        }

        // Cập nhật ngày tour trong cơ sở dữ liệu
        [HttpPost("tourday/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourDay tourDay, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != tourDay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //await _tourDayRepository.UpdateTourDayAsync(tourDay);
                var existingTourDay = await _tourDayRepository.GetTourDayByIdAsync(id);

                if (imageUrl == null)
                {
                    tourDay.ImageUrl = existingTourDay.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới

                    tourDay.ImageUrl = await SaveImage(imageUrl);

                }

                existingTourDay.ImageUrl = tourDay.ImageUrl;

                await _tourDayRepository.UpdateTourDayAsync(existingTourDay);
                return RedirectToAction(nameof(Index));
            }

            // Truyền lại danh sách danh mục và sản phẩm nếu model không hợp lệ
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return View(tourDay);
        }

        // Hiển thị form xác nhận xóa ngày tour
        [HttpGet("tourday/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tourDay = await _tourDayRepository.GetTourDayByIdAsync(id);
            if (tourDay == null)
            {
                return NotFound();
            }
            return View(tourDay);
        }

        // Xóa ngày tour khỏi cơ sở dữ liệu
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tourDayRepository.DeleteTourDayAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}