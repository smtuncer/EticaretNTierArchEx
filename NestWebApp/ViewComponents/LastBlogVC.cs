using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models.Data;

namespace NestWebApp.ViewComponents
{
    public class LastBlogVC : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public LastBlogVC(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Blog.Where(x => x.IsDeleted == false).AsNoTracking().Take(7).ToListAsync());
        }
    }
}