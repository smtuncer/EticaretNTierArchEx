using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models;
using NestWebApp.Models.Data;
using NestWebApp.Models.StaticClasses;
using System.Security.Claims;

namespace NestWebApp.Areas.User.Controllers
{
    [Area("User")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("/urun-listesi")]
        public async Task<IActionResult> ProductList()
        {
            return View(await _context.Product.Where(x => x.IsDeleted == false).Include(x => x.ProductCategory).AsNoTracking().ToListAsync());
        }
        [Route("/kategori-detayi/{id}")]
        public async Task<IActionResult> CategoryDetail(int id)
        {
            return View(await _context.ProductCategory
                        .Where(x => x.Id == id)
                        .Include(x => x.Product.Where(x => x.IsDeleted == false)).AsNoTracking().FirstOrDefaultAsync());
        }
        [Route("/urun-ara/{q?}")]
        public async Task<IActionResult> SearchProduct(string? q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                var data = await _context.Product
                    .Where(x => x.Title.Contains(q) || x.Description.Contains(q))
                    .Where(x => x.IsDeleted == false)
                    .AsNoTracking()
                    .ToListAsync();
                TempData["searchText"] = q;
                return View(data);
            }
            return View();
        }
        [Route("/kampanyali-urunler")]
        public async Task<IActionResult> CampaignProduct()
        {
            return View(await _context.Product.Where(x => x.IsDeleted == false).Where(x => x.IsCampaing == true).Include(x => x.ProductCategory).AsNoTracking().ToListAsync());
        }
        [Route("/urun-detayi/{id}")]
        public IActionResult ProductDetail(int id)
        {
            var product = _context.Product
                        .Where(x => x.Id == id)
                        .Include(x => x.ProductCategory).FirstOrDefault();

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = product,
                ProductId = product.Id
            };
            return View(shoppingCart);
        }
        [Route("/urun-detayi/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ProductDetail(ShoppingCart SCart)
        {
            SCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                SCart.AppUserId = claim.Value;
                ShoppingCart cart = _context.ShoppingCart.FirstOrDefault(x => x.AppUserId == SCart.AppUserId && x.ProductId == SCart.ProductId);
                if (cart == null)
                {
                    _context.ShoppingCart.Add(SCart);
                }
                else
                {
                    cart.Count += SCart.Count;
                }
                _context.SaveChanges();
                var count = _context.ShoppingCart.Where(x => x.AppUserId == SCart.AppUserId).ToList().Count();
                HttpContext.Session.SetInt32(Other.ssShopingCart, count);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var product = _context.Product
                        .Where(x => x.Id == SCart.ProductId)
                        .Include(x => x.ProductCategory).FirstOrDefault();

                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id
                };

            }
            return View(SCart);
        }
    }
}
