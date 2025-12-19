using PartnerManager.Service.DTOs;
using PartnerManager.Infrastructure.Models;
using PartnerManager.Infrastructure.Repositories;
using BCrypt.Net;

namespace PartnerManager.Service.Services
{
    /// <summary>
    /// Local (username/password) authentication provider
    /// </summary>
    public class LocalAuthProvider : IAuthProvider
    {
        private readonly IUserRepository _userRepo;

        public LocalAuthProvider(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public string ProviderName => "local";

        public async Task<User?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<bool> ValidateAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return false;

            return await Task.FromResult(true);
        }
    }
}
