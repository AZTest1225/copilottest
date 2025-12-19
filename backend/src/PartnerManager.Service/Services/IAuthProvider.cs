using PartnerManager.Service.DTOs;
using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Service.Services
{
    /// <summary>
    /// Interface for authentication providers (local, Google, Microsoft, etc.)
    /// </summary>
    public interface IAuthProvider
    {
        string ProviderName { get; }
        Task<User?> AuthenticateAsync(LoginRequest request);
        Task<bool> ValidateAsync(LoginRequest request);
    }
}
