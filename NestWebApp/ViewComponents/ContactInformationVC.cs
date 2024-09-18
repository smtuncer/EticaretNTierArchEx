using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models.Data;

namespace NestWebApp.ViewComponents
{
    public class ContactInformationVC : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ContactInformationVC(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Contact.AsNoTracking().FirstOrDefaultAsync());
        }
    }
}