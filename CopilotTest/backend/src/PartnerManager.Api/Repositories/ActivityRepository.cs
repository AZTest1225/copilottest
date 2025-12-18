using Microsoft.EntityFrameworkCore;
using PartnerManager.Api.Data;
using PartnerManager.Api.Models;

namespace PartnerManager.Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _db;
        public ActivityRepository(AppDbContext db) { _db = db; }

        public async Task<(IEnumerable<Activity> Items, int Total)> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 20)
        {
            var query = _db.Activities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(a => a.Title.ToLower().Contains(s) || (a.Description != null && a.Description.ToLower().Contains(s)));
            }
            if (from.HasValue) query = query.Where(a => a.StartAt >= from.Value);
            if (to.HasValue) query = query.Where(a => a.EndAt <= to.Value);
            var total = await query.CountAsync();
            var items = await query.OrderBy(a => a.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _db.Activities.Include(a => a.PartnerActivities).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Activity> AddAsync(Activity activity)
        {
            _db.Activities.Add(activity);
            await _db.SaveChangesAsync();
            return activity;
        }

        public async Task UpdateAsync(Activity activity)
        {
            _db.Activities.Update(activity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await _db.Activities.FindAsync(id);
            if (a != null)
            {
                _db.Activities.Remove(a);
                await _db.SaveChangesAsync();
            }
        }
    }
}
