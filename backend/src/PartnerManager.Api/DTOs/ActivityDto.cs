using System.ComponentModel.DataAnnotations;

namespace PartnerManager.Api.DTOs
{
    public class ActivityCreateDto
    {
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }

    public class ActivityUpdateDto : ActivityCreateDto { }

    public class ActivityDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Status { get; set; } = null!;
    }
}
