using Microsoft.AspNetCore.Identity;

namespace NestWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string? NameSurname { get; set; }
       
        public List<OrderHeader>? OrderHeader { get; set; }
    }
}
