using System.ComponentModel.DataAnnotations;

namespace PartnerManager.Infrastructure.Models
{
    public enum ActivityStatus
    {
        Draft = 0,
        Published = 1,
        Finished = 2
    }

    public class Activity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public ActivityStatus Status { get; set; } = ActivityStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PartnerActivity>? PartnerActivities { get; set; }
    }
}
