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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }
        public async Task<IActionResult> CategoriesList()
        {
            return View(await _context.ProductCategory.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> AddCategory(string? Title)
        {
            ProductCategory productCategory = new ProductCategory();
            productCategory.Title = Title;
            await _context.AddAsync(productCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("CategoriesList");
        }
        public async Task<IActionResult> EditCategory(int? id)
        {
            return View(await _context.ProductCategory.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(ProductCategory p)
        {
            _context.Update(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("CategoriesList");
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await _context.ProductCategory.FirstOrDefaultAsync(x => x.Id == id);
            data.IsDeleted = true;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("CategoriesList");
        }
        public async Task<IActionResult> ProductList()
        {

            return View(await _context.Product.Where(x => x.IsDeleted == false).Include(x => x.ProductCategory).AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.ProductCategoryList = await _context.ProductCategory.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product p)
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
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
        public async Task<IActionResult> EditProduct(int id)
        {
            ViewBag.ProductCategoryList = await _context.ProductCategory.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync();
            return View(await _context.Product.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync());
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product p)
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
            return RedirectToAction("ProductList");
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var data = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
            data.IsDeleted = true;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
    }
}
