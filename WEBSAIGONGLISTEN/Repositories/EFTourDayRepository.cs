using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEBSAIGONGLISTEN.Models;

namespace WEBSAIGONGLISTEN.Repositories
{
    public class EFTourDayRepository : ITourDayRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTourDayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các ngày tour
        public async Task<IEnumerable<TourDay>> GetAllTourDaysAsync()
        {
            return await _context.TourDays.Include(p => p.Category).ToListAsync();
        }

        // Lấy thông tin ngày tour theo Id
        public async Task<TourDay?> GetTourDayByIdAsync(int id)
        {
            return await _context.TourDays.Include(p =>
       p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm mới ngày tour
        public async Task AddTourDayAsync(TourDay tourDay)
        {
            await _context.TourDays.AddAsync(tourDay);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin ngày tour
        public async Task UpdateTourDayAsync(TourDay tourDay)
        {
            _context.TourDays.Update(tourDay);
            await _context.SaveChangesAsync();
        }

        // Xóa ngày tour theo Id
        public async Task DeleteTourDayAsync(int id)
        {
            var tourDay = await _context.TourDays.FindAsync(id);
            if (tourDay != null)
            {
                _context.TourDays.Remove(tourDay);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TourDay>> GetTourdaysByProductIdAsync(int productId)
        {
            return await _context.TourDays
                .Where(t => t.ProductId == productId) // Giả sử có trường ProductId trong TourDay
                .ToListAsync();
        }
    }
}