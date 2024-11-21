using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBSAIGONGLISTEN.Models;

namespace WEBSAIGONGLISTEN.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _dbcontext;

        public DashboardController (ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public List<object> GetTotalOders()
        //{
        //    List<object> data = new List<object>();
        //    List<string> labels = _dbcontext.Categories.Select(m => m.Products.).Take(5).ToList();
        //    List<int> total = _dbcontext.Categories.Select(t => t.).Take(5).ToList();
        //    data.Add(labels);
        //    data.Add(total);
        //    return data;
        //}
        [HttpPost]
        public List<object> GetTotalOders()
        {
            List<object> data = new List<object>();

            // Lấy danh sách nhãn (tên danh mục)
            List<string> labels = _dbcontext.Categories
                .Select(c => c.Name)
                .Take(5) // Giới hạn 5 danh mục
                .ToList();

            // Tính tổng đơn đặt hàng cho từng danh mục
            List<int> total = _dbcontext.Categories
                .Include(c => c.Products) // Bao gồm các sản phẩm
                .Select(c => c.Products
                    .SelectMany(p => _dbcontext.Bookings.Where(b => b.ProductId == p.Id)) // Lấy các đơn đặt hàng liên quan
                    .Sum(b => b.QuantityUnder2 + b.Quantity2To10 + b.QuantityAbove10) // Tính tổng số lượng
                )
                .Take(5) // Giới hạn 5 danh mục
                .ToList();

            // Thêm dữ liệu vào danh sách
            data.Add(labels);
            data.Add(total);

            return data;
        }


    }
}
