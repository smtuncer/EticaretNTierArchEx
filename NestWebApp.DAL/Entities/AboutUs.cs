using System.ComponentModel.DataAnnotations;

namespace NestWebApp.DAL.Models;

public class AboutUs
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
}

