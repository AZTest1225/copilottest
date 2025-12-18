using PartnerManager.Infrastructure.Models;
using System.Collections.Generic;

namespace PartnerManager.Infrastructure.Repositories
{
    public interface IActivityRepository
    {
        Task<(IEnumerable<Activity> Items, int Total)> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 20);
        Task<Activity?> GetByIdAsync(int id);
        Task<Activity> AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
    }
}
