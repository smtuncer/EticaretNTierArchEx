using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;

namespace NestWebApp.ViewComponents;

public class CategoriesForProductsPageVC : ViewComponent
{
    private readonly ApplicationDbContext _context;
    public CategoriesForProductsPageVC(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.ProductCategory.Where(x => x.IsDeleted == false).Include(x => x.Product
            .Where(x => x.IsDeleted == false)).AsNoTracking().ToListAsync());
    }
}