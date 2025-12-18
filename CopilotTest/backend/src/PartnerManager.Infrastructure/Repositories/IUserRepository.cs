using PartnerManager.Infrastructure.Models;

namespace PartnerManager.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
