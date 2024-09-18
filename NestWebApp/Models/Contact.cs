using System.ComponentModel.DataAnnotations;

namespace NestWebApp.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? OpeningHours { get; set; }
        public string? GoogleMapIframeSrc { get; set; }
    }
}
