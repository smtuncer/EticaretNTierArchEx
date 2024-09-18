using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class SliderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _he;
    public SliderController(ApplicationDbContext context, IWebHostEnvironment he)
    {
        _context = context;
        _he = he;
    }
    public async Task<IActionResult> SliderList()
    {
        return View(await _context.Slider.AsNoTracking().ToListAsync());
    }
    public async Task<IActionResult> AddSlider(string? Title, string? SubTitle)
    {
        Slider slider = new Slider();
        slider.Title = Title;
        slider.SubTitle = SubTitle;
        var files = HttpContext.Request.Form.Files;
        if (files.Count > 0)
        {

            var ext = Path.GetExtension(files[0].FileName).ToLower();
            string fileName = Guid.NewGuid().ToString();
            var upload = Path.Combine(_he.WebRootPath, @"files");
            using (var filesStreams = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
            {
                files[0].CopyTo(filesStreams);
            }
            slider.Image = @"/files/" + fileName + ext;
        }
        await _context.AddAsync(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction("SliderList");
    }
    public async Task<IActionResult> EditSlider(int id)
    {
        return View(await _context.Slider.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
    }
    [HttpPost]
    public async Task<IActionResult> EditSlider(Slider p)
    {
        var files = HttpContext.Request.Form.Files;
        if (files.Count > 0)
        {

            var ext = Path.GetExtension(files[0].FileName).ToLower();
            string fileName = Guid.NewGuid().ToString();
            var upload = Path.Combine(_he.WebRootPath, @"files");
            using (var filesStreams = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
            {
                files[0].CopyTo(filesStreams);
            }
            p.Image = @"/files/" + fileName + ext;
        }
        _context.Update(p);
        await _context.SaveChangesAsync();
        return RedirectToAction("SliderList");
    }
    public async Task<IActionResult> DeleteSlider(int id)
    {
        var data = await _context.Slider.FirstOrDefaultAsync(x => x.Id == id);
        _context.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction("SliderList");
    }
}
