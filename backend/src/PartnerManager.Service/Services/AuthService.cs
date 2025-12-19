using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using PartnerManager.Service.DTOs;
using PartnerManager.Infrastructure.Repositories;
using PartnerManager.Infrastructure.Models;
using PartnerManager.Infrastructure.Config;

namespace PartnerManager.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly CConfig _config;
        private readonly Dictionary<string, IAuthProvider> _authProviders;

        public AuthService(IUserRepository userRepo, CConfig config, IEnumerable<IAuthProvider> authProviders)
        {
            _userRepo = userRepo;
            _config = config;
            _authProviders = authProviders.ToDictionary(p => p.ProviderName, p => p);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existing = await _userRepo.GetByEmailAsync(request.Email);
            if (existing != null) throw new InvalidOperationException("Email already registered");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User { UserName = request.UserName, Email = request.Email, PasswordHash = hash };
            await _userRepo.AddAsync(user);

            var token = GenerateToken(user);
            return new AuthResponse { Token = token, ExpiresIn = _config.JwtExpireMinutes, User = new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role.ToString() } };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // Default to local provider if not specified
            var providerName = string.IsNullOrWhiteSpace(request.Provider) ? "local" : request.Provider.ToLower();

            if (!_authProviders.TryGetValue(providerName, out var provider))
            {
                throw new InvalidOperationException($"Authentication provider '{providerName}' is not supported");
            }

            // Validate request for the provider
            if (!await provider.ValidateAsync(request))
            {
                throw new InvalidOperationException("Invalid authentication request");
            }

            // Authenticate using the selected provider
            var user = await provider.AuthenticateAsync(request);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid credentials");
            }

            var token = GenerateToken(user);
            return new AuthResponse 
            { 
                Token = token, 
                ExpiresIn = _config.JwtExpireMinutes, 
                User = new UserDto 
                { 
                    Id = user.Id, 
                    UserName = user.UserName, 
                    Email = user.Email, 
                    Role = user.Role.ToString() 
                } 
            };
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config.JwtKey ?? string.Empty);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("username", user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _config.JwtIssuer,
                audience: _config.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.JwtExpireMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
