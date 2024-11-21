using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WEBSAIGONGLISTEN.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WEBSAIGONGLISTEN.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // Thêm UserManager để lấy thông tin người dùng

        public ThongBaoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy UserId của người dùng hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Hoặc: User.Identity.Name

            //if (userId == null)
            //{
            //    return Unauthorized(); // Nếu không tìm thấy userId, trả về Unauthorized
            //}

            if (userId == null) // Nếu người dùng chưa đăng nhập
            {
                // Truyền thông báo vào ViewBag để hiển thị trong View
                ViewBag.Message = "Vui lòng đăng nhập để xem thông báo.";
                return View(Enumerable.Empty<ThongBao>()); // Trả về một danh sách rỗng
            }

            // Lấy danh sách thông báo của người dùng hiện tại từ cơ sở dữ liệu
            var thongBaos = await _context.ThongBaos
                .Where(t => t.UserId == userId) // Lọc thông báo theo UserId
                .Include(t => t.ApplicationUser) // Lấy thông tin người dùng (ApplicationUser) liên kết
                .OrderByDescending(t => t.Id) // Sắp xếp theo Id giảm dần (thông báo mới nhất lên đầu)
                .ToListAsync(); // Thực hiện truy vấn bất đồng bộ để lấy danh sách thông báo

            // Trả về View với danh sách thông báo của người dùng hiện tại
            return View(thongBaos);
        }
    }
}