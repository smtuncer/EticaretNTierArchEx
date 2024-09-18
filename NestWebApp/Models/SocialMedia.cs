using System.ComponentModel.DataAnnotations;

namespace NestWebApp.Models
{
    public class SocialMedia
    {
        [Key]
        public int Id { get; set; }
        public string? SocialMediaName { get; set; }
        public string? Link { get; set; }
    }
}
