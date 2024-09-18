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
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;
        public BrandController(ApplicationDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brand.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> AddBrand(string? Title)
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
            await _context.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditBrand(int id)
        {
            return View(await _context.Brand.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
        }
        [HttpPost]
        public async Task<IActionResult> EditBrand(int Id, string? Title, string? Image)
        {
            var data = await _context.Brand.FirstOrDefaultAsync(x => x.Id == Id);
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
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var data = await _context.Brand.FirstOrDefaultAsync(x => x.Id == id);
            _context.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
