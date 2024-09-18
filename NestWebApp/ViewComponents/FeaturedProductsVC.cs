using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;

namespace NestWebApp.ViewComponents;

public class FeaturedProductsVC : ViewComponent
{
    private readonly ApplicationDbContext _context;
    public FeaturedProductsVC(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Product
            .Where(x => x.IsFeatured == true)
            .Where(x => x.IsDeleted == false)
            .AsNoTracking().Take(4).ToListAsync());
    }
}