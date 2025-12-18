using Microsoft.EntityFrameworkCore;
using PartnerManager.Api.Data;
using PartnerManager.Api.Models;

namespace PartnerManager.Api.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly AppDbContext _db;
        public PartnerRepository(AppDbContext db) { _db = db; }

        public async Task<(IEnumerable<Partner> Items, int Total)> GetAllAsync(string? search = null, int page = 1, int pageSize = 20)
        {
            var query = _db.Partners.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(s) || (p.ContactName != null && p.ContactName.ToLower().Contains(s)) || (p.ContactEmail != null && p.ContactEmail.ToLower().Contains(s)));
            }
            var total = await query.CountAsync();
            var items = await query.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        public async Task<Partner?> GetByIdAsync(int id)
        {
            return await _db.Partners.Include(p => p.PartnerActivities).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Partner> AddAsync(Partner partner)
        {
            _db.Partners.Add(partner);
            await _db.SaveChangesAsync();
            return partner;
        }

        public async Task UpdateAsync(Partner partner)
        {
            _db.Partners.Update(partner);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var p = await _db.Partners.FindAsync(id);
            if (p != null)
            {
                _db.Partners.Remove(p);
                await _db.SaveChangesAsync();
            }
        }
    }
}
