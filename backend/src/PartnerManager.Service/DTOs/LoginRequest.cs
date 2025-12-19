using System.ComponentModel.DataAnnotations;

namespace PartnerManager.Service.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Authentication provider: "local" (default), "google", "microsoft", etc.
        /// </summary>
        public string? Provider { get; set; } = "local";

        /// <summary>
        /// External OAuth token for third-party authentication providers
        /// </summary>
        public string? ExternalToken { get; set; }
    }
}
