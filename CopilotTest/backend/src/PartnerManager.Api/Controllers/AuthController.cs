using Microsoft.AspNetCore.Mvc;
using PartnerManager.Api.DTOs;
using PartnerManager.Service.Services;

namespace PartnerManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            try
            {
                var svcReq = new PartnerManager.Service.DTOs.RegisterRequest { UserName = req.UserName, Email = req.Email, Password = req.Password };
                var res = await _auth.RegisterAsync(svcReq);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                var svcReq = new PartnerManager.Service.DTOs.LoginRequest { Email = req.Email, Password = req.Password };
                var res = await _auth.LoginAsync(svcReq);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
