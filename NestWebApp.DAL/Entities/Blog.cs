using System.ComponentModel.DataAnnotations;

namespace NestWebApp.DAL.Models;

public class Blog
{
    [Key]
    public int Id { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? Title { get; set; }
    public string? BlogText { get; set; }
    public string? Image { get; set; }
    public bool IsDeleted { get; set; }
}
