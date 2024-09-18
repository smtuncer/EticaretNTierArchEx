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
    public class AboutUsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AboutUsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create()
        {
            var data = await _context.AboutUs.AsNoTracking().FirstOrDefaultAsync();
            if (data != null)
            {
                return RedirectToAction("Edit");
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AboutUs p)
        {
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }
        public async Task<IActionResult> Edit()
        {
            var data = await _context.AboutUs.AsNoTracking().FirstOrDefaultAsync();
            if (data == null)
            {
                return RedirectToAction("Create");
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AboutUs p)
        {
            _context.Update(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }
    }
}
