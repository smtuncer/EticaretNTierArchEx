using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;

namespace NestWebApp.ViewComponents;

public class NewProductVC : ViewComponent
{
    private readonly ApplicationDbContext _context;
    public NewProductVC(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Product.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).AsNoTracking().Take(3).ToListAsync());
    }
}