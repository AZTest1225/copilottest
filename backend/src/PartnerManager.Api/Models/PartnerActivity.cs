using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartnerManager.Api.Models
{
    public class PartnerActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PartnerId { get; set; }
        public Partner? Partner { get; set; }

        [Required]
        public int ActivityId { get; set; }
        public Activity? Activity { get; set; }

        // Optional additional info about the relation
        [MaxLength(200)]
        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
