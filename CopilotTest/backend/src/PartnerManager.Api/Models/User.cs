using System.ComponentModel.DataAnnotations;

namespace PartnerManager.Api.Models
{
    public enum Role
    {
        User = 0,
        Admin = 1
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public Role Role { get; set; } = Role.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
