using System.ComponentModel.DataAnnotations;

namespace NestWebApp.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Image { get; set; }
    }
}
