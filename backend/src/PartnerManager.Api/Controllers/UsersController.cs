using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerManager.Infrastructure.Data;
using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var users = await _db.Users.OrderBy(u => u.Id).Skip((page - 1) * pageSize).Take(pageSize).Select(u => new { u.Id, u.UserName, u.Email, Role = u.Role.ToString(), u.IsActive, u.CreatedAt }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            return Ok(new { u.Id, u.UserName, u.Email, Role = u.Role.ToString(), u.IsActive, u.CreatedAt });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            _db.Users.Remove(u);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string role)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            if (!Enum.TryParse<Role>(role, true, out var r)) return BadRequest(new { error = "Invalid role" });
            u.Role = r;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
