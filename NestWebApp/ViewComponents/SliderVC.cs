using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;

namespace NestWebApp.ViewComponents;

public class SliderVC : ViewComponent
{
    private readonly ApplicationDbContext _context;
    public SliderVC(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Slider.AsNoTracking().ToListAsync());
    }
}