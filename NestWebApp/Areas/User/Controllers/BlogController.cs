using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;

namespace NestWebApp.Areas.User.Controllers;

[Area("User")]
public class BlogController : Controller
{
    private readonly ApplicationDbContext _context;
    public BlogController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("/blog")]
    public async Task<IActionResult> BlogList()
    {
        return View(await _context.Blog.AsNoTracking().ToListAsync());
    }
    [Route("/blog-detayi/{id}")]
    public async Task<IActionResult> BlogDetail(int id)
    {
        return View(await _context.Blog.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
    }
    [Route("/blog-ara/{q?}")]
    public async Task<IActionResult> SearchBlog(string? q)
    {
        if (!string.IsNullOrEmpty(q))
        {
            var blogList = await _context.Blog
                .Where(x => x.BlogText.Contains(q) || x.Title.Contains(q))
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();
            TempData["searchText"] = q;
            return View(blogList);
        }
        return View();
    }
}
