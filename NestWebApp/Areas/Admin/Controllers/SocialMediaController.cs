using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class SocialMediaController : Controller
{
    private readonly ApplicationDbContext _context;
    public SocialMediaController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> SocialMediaList()
    {
        return View(await _context.SocialMedia.AsNoTracking().ToListAsync());
    }
    public async Task<IActionResult> AddSocialMedia(string? SocialMediaName, string? Link)
    {
        SocialMedia socialMedia = new SocialMedia();
        socialMedia.SocialMediaName = SocialMediaName;
        socialMedia.Link = Link;
        await _context.AddAsync(socialMedia);
        await _context.SaveChangesAsync();
        return RedirectToAction("SocialMediaList");
    }
    public async Task<IActionResult> EditSocialMedia(int id)
    {
        return View(await _context.SocialMedia.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
    }
    [HttpPost]
    public async Task<IActionResult> EditSocialMedia(SocialMedia p)
    {
        _context.Update(p);
        await _context.SaveChangesAsync();
        return RedirectToAction("SocialMediaList");
    }
    public async Task<IActionResult> DeleteSocialMedia(int id)
    {
        var data = await _context.SocialMedia.FirstOrDefaultAsync(x => x.Id == id);
        _context.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction("SocialMediaList");
    }
}