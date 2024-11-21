using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEBSAIGONGLISTEN.Models;
using WEBSAIGONGLISTEN.Services;

namespace WEBSAIGONGLISTEN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Company)]
    public class ThongBaoController : Controller
    {
        private readonly IThongBao _thongBaoService;

        public ThongBaoController(IThongBao thongBaoService)
        {
            _thongBaoService = thongBaoService;
        }

        // GET: Admin/ThongBao/Index
        public async Task<IActionResult> Index()
        {
            var thongBaos = await _thongBaoService.GetAllAsync();
            return View(thongBaos);
        }

        // GET: Admin/ThongBao/Details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var thongBao = await _thongBaoService.GetByIdAsync(id);
            if (thongBao == null)
            {
                return NotFound();
            }
            return View(thongBao);
        }

        // GET: Admin/ThongBao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ThongBao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThongBao thongBao)
        {
            if (ModelState.IsValid)
            {
                await _thongBaoService.CreateAsync(thongBao);
                return RedirectToAction(nameof(Index));
            }
            return View(thongBao);
        }

        [HttpGet("thongbaos/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var thongBao = await _thongBaoService.GetByIdAsync(id);
            if (thongBao == null)
            {
                return NotFound();
            }
            return View(thongBao);
        }

        // POST: Admin/ThongBao/Edit/5
        [HttpPost("thongbaos/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ThongBao thongBao)
        {
            if (id != thongBao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _thongBaoService.UpdateAsync(thongBao);
                return RedirectToAction(nameof(Index));
            }
            return View(thongBao);
        }

        // GET: Admin/ThongBao/Delete/5
        [HttpGet("thongbaos/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var thongBao = await _thongBaoService.GetByIdAsync(id);
            if (thongBao == null)
            {
                return NotFound();
            }
            return View(thongBao);
        }

        // POST: Admin/ThongBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _thongBaoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}