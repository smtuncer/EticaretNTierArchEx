using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;

namespace NestWebApp.ViewComponents;

public class SocialMediaVC : ViewComponent
{
    private readonly ApplicationDbContext _context;
    public SocialMediaVC(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.SocialMedia.AsNoTracking().ToListAsync());
    }
}