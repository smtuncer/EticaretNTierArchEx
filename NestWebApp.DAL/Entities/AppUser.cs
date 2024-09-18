using Microsoft.AspNetCore.Identity;

namespace NestWebApp.DAL.Models;

public class AppUser : IdentityUser
{
    public string? NameSurname { get; set; }

    public List<OrderHeader>? OrderHeader { get; set; }
}
