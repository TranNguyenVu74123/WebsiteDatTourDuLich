using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using WEBSAIGONGLISTEN.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System;

namespace WEBSAIGONGLISTEN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ booking từ cơ sở dữ liệu, bao gồm thông tin người dùng và sản phẩm
            var bookings = await _context.Bookings
                .Include(b => b.ApplicationUser)
                .Include(b => b.Product)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var bookings = await _context.Bookings
                .Include(b => b.ApplicationUser)
                .Include(b => b.Product)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Bookings");

                // Tạo tiêu đề
                worksheet.Cells[1, 1].Value = "Booking ID";
                worksheet.Cells[1, 2].Value = "User";
                worksheet.Cells[1, 3].Value = "Product Name";
                worksheet.Cells[1, 4].Value = "Booking Date";
                worksheet.Cells[1, 5].Value = "Total Price";
                worksheet.Cells[1, 6].Value = "Quantity Under 2";
                worksheet.Cells[1, 7].Value = "Quantity 2 to 10";
                worksheet.Cells[1, 8].Value = "Quantity Above 10";
                worksheet.Cells[1, 9].Value = "Status";

                int row = 2;
                foreach (var booking in bookings)
                {
                    worksheet.Cells[row, 1].Value = booking.Id;
                    worksheet.Cells[row, 2].Value = booking.ApplicationUser?.UserName;
                    worksheet.Cells[row, 3].Value = booking.Product?.Name;
                    worksheet.Cells[row, 4].Value = booking.BookingDate.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cells[row, 5].Value = booking.TotalPrice;
                    worksheet.Cells[row, 6].Value = booking.QuantityUnder2;
                    worksheet.Cells[row, 7].Value = booking.Quantity2To10;
                    worksheet.Cells[row, 8].Value = booking.QuantityAbove10;
                    worksheet.Cells[row, 9].Value = booking.Status;

                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"BookingHistory-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportToPdf()
        {
            var bookings = await _context.Bookings
                .Include(b => b.ApplicationUser)
                .Include(b => b.Product)
                .ToListAsync();

            var stream = new MemoryStream();

            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, stream).CloseStream = false;
            document.Open();

            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var title = new Paragraph("Booking History", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            foreach (var booking in bookings)
            {
                var bookingInfo = new Paragraph($"Booking ID: {booking.Id} - User: {booking.ApplicationUser?.UserName}", FontFactory.GetFont("Arial", 12, Font.BOLD));
                document.Add(bookingInfo);

                document.Add(new Paragraph($"Product Name: {booking.Product?.Name}"));
                document.Add(new Paragraph($"Booking Date: {booking.BookingDate:yyyy-MM-dd HH:mm}"));
                document.Add(new Paragraph($"Total Price: {booking.TotalPrice:C}"));
                document.Add(new Paragraph($"Quantity Under 2: {booking.QuantityUnder2}"));
                document.Add(new Paragraph($"Quantity 2 to 10: {booking.Quantity2To10}"));
                document.Add(new Paragraph($"Quantity Above 10: {booking.QuantityAbove10}"));
                document.Add(new Paragraph($"Status: {booking.Status}"));
                document.Add(new Paragraph("\n"));
            }

            document.Close();

            stream.Position = 0;
            string pdfName = $"BookingHistory-{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return File(stream, "application/pdf", pdfName);
        }

        [HttpPost]
        public IActionResult ProcessCancellation(int id, bool approve, string adminResponse)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null || booking.Status != "Awaiting Admin Confirmation")
            {
                TempData["ErrorMessage"] = "Không tìm thấy yêu cầu cần xử lý.";
                return View();
            }

            if (approve)
            {
                booking.Status = "Canceled";
            }
            else
            {
                booking.Status = "Pending";
            }

            booking.AdminProcessedDate = DateTime.Now;
            _context.SaveChanges();

            TempData["SuccessMessage"] = approve ? "Yêu cầu hủy đã được xác nhận." : "Yêu cầu hủy đã bị từ chối.";
            return View();
        }

        [HttpGet("histories/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin Booking với ID đã cho, bao gồm người dùng và sản phẩm
            var booking = await _context.Bookings
                .Include(b => b.ApplicationUser)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound(); // Nếu không tìm thấy Booking, trả về lỗi 404
            }

            return View(booking); // Trả về view để người dùng xác nhận việc xóa
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Tìm booking với id cần xóa
            var booking = await _context.Bookings
                .Include(b => b.ApplicationUser) // Lấy thông tin người dùng
                .Include(b => b.Product)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound(); // Nếu không tìm thấy Booking, trả về lỗi 404
            }

            // Xóa bản ghi Booking
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            // Tạo một thông báo mới cho người dùng
            var thongBao = new ThongBao
            {
                Description = $"Thông báo: Chuyến du lịch bạn đặt là {booking.Product.Name} đã bị hủy. Chúng tôi thành thật xin lỗi",
                UserId = booking.UserId
            };

            // Thêm thông báo vào bảng ThongBaos
            _context.ThongBaos.Add(thongBao);
            await _context.SaveChangesAsync();

            // Điều hướng về trang index sau khi thực hiện thành công
            return RedirectToAction(nameof(Index));
        }

    }
}
