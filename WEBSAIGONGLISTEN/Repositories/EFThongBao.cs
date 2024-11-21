using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBSAIGONGLISTEN.Models;

namespace WEBSAIGONGLISTEN.Services
{
    public class EFThongBao : IThongBao
    {
        private readonly ApplicationDbContext _context;

        public EFThongBao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThongBao>> GetAllAsync()
        {
            return await _context.ThongBaos
                                            .Include(tb => tb.ApplicationUser)
                                            .ToListAsync();
        }

        public async Task<ThongBao> GetByIdAsync(int id)
        {
            return await _context.ThongBaos
                                            .Include(tb => tb.ApplicationUser)
                                            .FirstOrDefaultAsync(tb => tb.Id == id);
        }

        public async Task CreateAsync(ThongBao thongBao)
        {
            await _context.ThongBaos.AddAsync(thongBao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ThongBao thongBao)
        {
            _context.ThongBaos.Update(thongBao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var thongBao = await _context.ThongBaos.FindAsync(id);
            if (thongBao != null)
            {
                _context.ThongBaos.Remove(thongBao);
                await _context.SaveChangesAsync();
            }
        }
    }
}