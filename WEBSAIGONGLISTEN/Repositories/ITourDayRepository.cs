using System.Collections.Generic;
using System.Threading.Tasks;
using WEBSAIGONGLISTEN.Models;

namespace WEBSAIGONGLISTEN.Repositories
{
    public interface ITourDayRepository
    {
        Task<IEnumerable<TourDay>> GetAllTourDaysAsync();
        Task<TourDay?> GetTourDayByIdAsync(int id);
        Task AddTourDayAsync(TourDay tourDay);
        Task UpdateTourDayAsync(TourDay tourDay);
        Task DeleteTourDayAsync(int id);
        Task<List<TourDay>> GetTourdaysByProductIdAsync(int productId);
    }
}