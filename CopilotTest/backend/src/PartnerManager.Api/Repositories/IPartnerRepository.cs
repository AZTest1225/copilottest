using PartnerManager.Api.Models;
using System.Collections.Generic;

namespace PartnerManager.Api.Repositories
{
    public interface IPartnerRepository
    {
        Task<(IEnumerable<Partner> Items, int Total)> GetAllAsync(string? search = null, int page = 1, int pageSize = 20);
        Task<Partner?> GetByIdAsync(int id);
        Task<Partner> AddAsync(Partner partner);
        Task UpdateAsync(Partner partner);
        Task DeleteAsync(int id);
    }
}
