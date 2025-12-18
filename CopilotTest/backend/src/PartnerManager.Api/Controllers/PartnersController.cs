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
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerRepository _repo;
        public PartnersController(IPartnerRepository repo) { _repo = repo; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (items, total) = await _repo.GetAllAsync(search, page, pageSize);
            var dto = items.Select(p => new PartnerDto { Id = p.Id, Name = p.Name, ContactName = p.ContactName, ContactEmail = p.ContactEmail, ContactPhone = p.ContactPhone, Status = p.Status.ToString() });
            return Ok(new DTOs.PagedResult<PartnerDto> { Items = dto, Total = total, Page = page, PageSize = pageSize });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            var dto = new PartnerDto { Id = p.Id, Name = p.Name, ContactName = p.ContactName, ContactEmail = p.ContactEmail, ContactPhone = p.ContactPhone, Status = p.Status.ToString() };
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PartnerCreateDto req)
        {
            var p = new Partner { Name = req.Name, ContactName = req.ContactName, ContactEmail = req.ContactEmail, ContactPhone = req.ContactPhone };
            var created = await _repo.AddAsync(p);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new PartnerDto { Id = created.Id, Name = created.Name, ContactName = created.ContactName, ContactEmail = created.ContactEmail, ContactPhone = created.ContactPhone, Status = created.Status.ToString() });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, PartnerUpdateDto req)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            p.Name = req.Name;
            p.ContactName = req.ContactName;
            p.ContactEmail = req.ContactEmail;
            p.ContactPhone = req.ContactPhone;
            await _repo.UpdateAsync(p);
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
