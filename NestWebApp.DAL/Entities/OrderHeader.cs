using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestWebApp.DAL.Models;

public class OrderHeader
{
    [Key]
    public int Id { get; set; }
    public string? AppUserId { get; set; }
    [ForeignKey("AppUserId")]
    public AppUser? AppUser { get; set; }
    public DateTime OrderDate { get; set; }
    public double? OrderTotalPrice { get; set; }
    public string? Status { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public bool IsDeleted { get; set; }
    public List<OrderDetails>? OrderDetails { get; set; }
}
