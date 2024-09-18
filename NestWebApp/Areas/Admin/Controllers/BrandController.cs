using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NestWebApp.BL.Repositories;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class BrandController : Controller
{
    private readonly IRepository<Brand> _context;
    private readonly IWebHostEnvironment _he;
    public BrandController(IRepository<Brand> context, IWebHostEnvironment he)
    {
        _context = context;
        _he = he;
    }
    public IActionResult Index()
    {
        return View(_context.GetAll());
    }
    public IActionResult AddBrand(string? Title)
    {
        Brand brand = new Brand();
        brand.Title = Title;
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
            brand.Image = @"/files/" + fileName + ext;
        }
        _context.Add(brand);
        return RedirectToAction("Index");
    }
    public IActionResult EditBrand(int id)
    {
        return View(_context.GetBy(x => x.Id == id));
    }
    [HttpPost]
    public IActionResult EditBrand(int Id, string? Title, string? Image)
    {
        var data = _context.GetBy(x => x.Id == Id);
        data.Title = Title;
        data.Image = Image;
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
            data.Image = @"/files/" + fileName + ext;
        }
        _context.Update(data);
        return RedirectToAction("Index");
    }
    public IActionResult DeleteBrand(int id)
    {
        var data = _context.GetBy(x => x.Id == id);
        _context.Delete(data);
        return RedirectToAction("Index");
    }
}
