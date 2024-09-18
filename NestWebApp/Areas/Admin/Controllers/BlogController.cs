using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NestWebApp.BL.Repositories;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class BlogController : Controller
{
    private readonly IRepository<Blog> _context;
    private readonly IWebHostEnvironment _he;
    public BlogController(IRepository<Blog> context, IWebHostEnvironment he)
    {
        _context = context;
        _he = he;
    }

    public IActionResult BlogList()
    {
        return View(_context.GetAll(x => x.IsDeleted == false));
    }
    public IActionResult AddBlog()
    {
        return View();
    }
    [HttpPost]
    public IActionResult AddBlog(Blog p)
    {
        p.CreateDate = DateTime.Now;
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
        _context.Add(p);
        return RedirectToAction("BlogList");
    }
    public IActionResult EditBlog(int id)
    {
        return View(_context.GetBy(x => x.Id == id));
    }
    [HttpPost]
    public IActionResult EditBlog(Blog p)
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
        return RedirectToAction("BlogList");
    }
    public IActionResult DeleteBlog(int id)
    {
        var data = _context.GetBy(x => x.Id == id);
        data.IsDeleted = true;
        _context.Update(data);
        return RedirectToAction("BlogList");
    }
}
