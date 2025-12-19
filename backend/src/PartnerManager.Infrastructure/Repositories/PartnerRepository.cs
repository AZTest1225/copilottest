using Microsoft.EntityFrameworkCore;
using PartnerManager.Infrastructure.Data;
using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Infrastructure.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly AppDbContext _db;
        public PartnerRepository(AppDbContext db) { _db = db; }

        public async Task<(IEnumerable<Partner> Items, int Total)> GetAllAsync(
            string? search = null,
            string? sex = null,
            string? sortBy = null,
            string? sortOrder = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _db.Partners.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(s)
                    || (p.ContactName != null && p.ContactName.ToLower().Contains(s))
                    || (p.ContactEmail != null && p.ContactEmail.ToLower().Contains(s))
                );
            }

            // Sex filter
            if (!string.IsNullOrWhiteSpace(sex))
            {
                var f = sex.Trim();
                query = query.Where(p => p.Sex != null && p.Sex == f);
            }

            var total = await query.CountAsync();

            // Sorting
            var sortKey = (sortBy ?? "Name").Trim().ToLower();
            var sortAsc = string.Equals((sortOrder ?? "asc").Trim(), "asc", StringComparison.OrdinalIgnoreCase);

            // Map UI sort keys to entity properties
            string propertyName = sortKey switch
            {
                "name" => "Name",
                "displayname" => "ContactName",
                "email" => "ContactEmail",
                "sex" => "Sex",
                "createdat" => "CreatedAt",
                _ => "Name"
            };

            // Dynamic OrderBy using EF.Property
            query = sortAsc
                ? query.OrderBy(e => EF.Property<object>(e, propertyName))
                : query.OrderByDescending(e => EF.Property<object>(e, propertyName));

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
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
