using System.ComponentModel.DataAnnotations;

namespace PartnerManager.Api.DTOs
{
    public class PartnerCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
    }

    public class PartnerUpdateDto : PartnerCreateDto
    {
    }

    public class PartnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string Status { get; set; } = null!;
    }
}
