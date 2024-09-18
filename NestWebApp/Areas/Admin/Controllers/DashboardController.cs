using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.UserCount = await _context.AppUser.CountAsync();
        ViewBag.BlogCount = await _context.Blog.Where(x => x.IsDeleted == false).CountAsync();
        ViewBag.ProductCount = await _context.Product.Where(x => x.IsDeleted == false).CountAsync();
        ViewBag.OrderCount = await _context.OrderHeader.Where(x => x.IsDeleted == false).CountAsync();
        ViewBag.TopProduct = await _context.Product
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(p => p.OrderDetails.Count())
            .Take(10)
            .ToListAsync();
        return View();
    }
}
