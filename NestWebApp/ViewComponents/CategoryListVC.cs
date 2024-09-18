using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models.Data;

namespace NestWebApp.ViewComponents
{
    public class CategoryListVC : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CategoryListVC(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.ProductCategory.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync());
        }
    }
}