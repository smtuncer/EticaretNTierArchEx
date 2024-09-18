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
    public class ContactController : Controller
    {

        private readonly ApplicationDbContext _context;
        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create()
        {
            var data = await _context.Contact.AsNoTracking().FirstOrDefaultAsync();
            if (data != null)
            {
                return RedirectToAction("Edit");
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Contact p)
        {
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }
        public async Task<IActionResult> Edit()
        {
            var data = await _context.Contact.AsNoTracking().FirstOrDefaultAsync();
            if (data == null)
            {
                return RedirectToAction("Create");
            }
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Contact p)
        {
            _context.Update(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }
    }
}

