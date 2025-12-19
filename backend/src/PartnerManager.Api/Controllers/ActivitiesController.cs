using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerManager.Api.DTOs;
using PartnerManager.Infrastructure.Models;
using PartnerManager.Infrastructure.Repositories;

namespace PartnerManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository _repo;
        public ActivitiesController(IActivityRepository repo) { _repo = repo; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (items, total) = await _repo.GetAllAsync(search, from, to, page, pageSize);
            var dto = items.Select(a => new ActivityDto { Id = a.Id, Title = a.Title, Description = a.Description, StartAt = a.StartAt, EndAt = a.EndAt, Status = a.Status.ToString() });
            return Ok(new DTOs.PagedResult<ActivityDto> { Items = dto, Total = total, Page = page, PageSize = pageSize });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return NotFound();
            var dto = new ActivityDto { Id = a.Id, Title = a.Title, Description = a.Description, StartAt = a.StartAt, EndAt = a.EndAt, Status = a.Status.ToString() };
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ActivityCreateDto req)
        {
            var a = new Activity { Title = req.Title, Description = req.Description, StartAt = req.StartAt, EndAt = req.EndAt };
            var created = await _repo.AddAsync(a);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new ActivityDto { Id = created.Id, Title = created.Title, Description = created.Description, StartAt = created.StartAt, EndAt = created.EndAt, Status = created.Status.ToString() });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ActivityUpdateDto req)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return NotFound();
            a.Title = req.Title;
            a.Description = req.Description;
            a.StartAt = req.StartAt;
            a.EndAt = req.EndAt;
            await _repo.UpdateAsync(a);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
