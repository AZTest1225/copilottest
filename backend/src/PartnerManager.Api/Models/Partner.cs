using System.ComponentModel.DataAnnotations;

namespace PartnerManager.Api.Models
{
    public enum PartnerStatus
    {
        Inactive = 0,
        Active = 1
    }

    public class Partner
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string? ContactName { get; set; }

        [MaxLength(200)]
        public string? ContactEmail { get; set; }

        [MaxLength(50)]
        public string? ContactPhone { get; set; }

        public PartnerStatus Status { get; set; } = PartnerStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PartnerActivity>? PartnerActivities { get; set; }
    }
}
