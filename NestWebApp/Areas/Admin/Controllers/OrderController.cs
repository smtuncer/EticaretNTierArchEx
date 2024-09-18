using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models.Data;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OrderList()
        {
            return View(await _context.OrderHeader.Include(x => x.OrderDetails).Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> PriceEdit(int id, double Price)
        {
            var data = await _context.OrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            data.Price = Price;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderDetail", new { id = data.OrderId });
        }
        public async Task<IActionResult> TeklifGonderildi(int id)
        {
            var data = await _context.OrderHeader.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            data.Status = Other.teklifGonderildi;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderDetail", new { id = id });
        }
        public async Task<IActionResult> TeklifBekliyor(int id)
        {
            var data = await _context.OrderHeader.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            data.Status = Other.teklifBekliyor;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderDetail", new { id = id });
        }
        public async Task<IActionResult> OrderDetail(int? id)
        {
            return View(await _context.OrderHeader.Where(x => x.Id == id).Include(x => x.AppUser).Include(x => x.OrderDetails).ThenInclude(x => x.Product).AsNoTracking().FirstOrDefaultAsync());
        }
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var data = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
            data.IsDeleted = true;
            _context.Update(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }
    }
}
