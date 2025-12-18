using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerManager.Infrastructure.Data;
using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Api.Controllers
{
    [ApiController]
    [Route("api/activities/{activityId}/partners")]
    [Authorize]
    public class PartnerActivitiesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PartnerActivitiesController(AppDbContext db) { _db = db; }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPartnerToActivity(int activityId, [FromBody] int partnerId)
        {
            var activity = await _db.Activities.FindAsync(activityId);
            if (activity == null) return NotFound(new { error = "Activity not found" });

            var partner = await _db.Partners.FindAsync(partnerId);
            if (partner == null) return NotFound(new { error = "Partner not found" });

            var existing = await _db.PartnerActivities.FindAsync(partnerId, activityId);
            // Since composite key is not defined, check manually
            if (await _db.PartnerActivities.AnyAsync(pa => pa.PartnerId == partnerId && pa.ActivityId == activityId))
            {
                return Conflict(new { error = "Association already exists" });
            }

            var pa = new PartnerActivity { PartnerId = partnerId, ActivityId = activityId };
            _db.PartnerActivities.Add(pa);
            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpDelete("{partnerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemovePartnerFromActivity(int activityId, int partnerId)
        {
            var pa = await _db.PartnerActivities.FirstOrDefaultAsync(x => x.ActivityId == activityId && x.PartnerId == partnerId);
            if (pa == null) return NotFound();
            _db.PartnerActivities.Remove(pa);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
