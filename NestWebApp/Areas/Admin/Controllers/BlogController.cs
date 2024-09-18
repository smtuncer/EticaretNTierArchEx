using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models;
using NestWebApp.Models.Data;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;
        public BlogController(ApplicationDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }
        public async Task<IActionResult> BlogList()
        {

            return View(await _context.Blog.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync());
        }
        public IActionResult AddBlog()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBlog(Blog p)
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
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("BlogList");
        }
        public async Task<IActionResult> EditBlog(int id)
        {
            return View(await _context.Blog.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
        }
        [HttpPost]
        public async Task<IActionResult> EditBlog(Blog p)
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
            return RedirectToAction("BlogList");
        }
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var data = await _context.Blog.FirstOrDefaultAsync(x => x.Id == id);
            data.IsDeleted = true;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("BlogList");
        }
    }
}
