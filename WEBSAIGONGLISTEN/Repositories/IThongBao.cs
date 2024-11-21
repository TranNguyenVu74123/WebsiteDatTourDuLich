using System.Collections.Generic;
using System.Threading.Tasks;
using WEBSAIGONGLISTEN.Models;

namespace WEBSAIGONGLISTEN.Services
{
    public interface IThongBao
    {
        Task<IEnumerable<ThongBao>> GetAllAsync();
        Task<ThongBao> GetByIdAsync(int id);
        Task CreateAsync(ThongBao thongBao);
        Task UpdateAsync(ThongBao thongBao);
        Task DeleteAsync(int id);
    }
}