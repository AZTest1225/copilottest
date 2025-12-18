using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using PartnerManager.Api.DTOs;
using PartnerManager.Api.Models;
using PartnerManager.Api.Repositories;

namespace PartnerManager.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existing = await _userRepo.GetByEmailAsync(request.Email);
            if (existing != null) throw new InvalidOperationException("Email already registered");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User { UserName = request.UserName, Email = request.Email, PasswordHash = hash };
            await _userRepo.AddAsync(user);

            var token = GenerateToken(user);
            return new AuthResponse { Token = token, ExpiresIn = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60"), User = new DTOs.UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role.ToString() } };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) throw new InvalidOperationException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) throw new InvalidOperationException("Invalid credentials");

            var token = GenerateToken(user);
            return new AuthResponse { Token = token, ExpiresIn = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60"), User = new DTOs.UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role.ToString() } };
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("username", user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"] ?? "60")),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
