using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;
using NestWebApp.Models.ViewModels;
using System.Security.Claims;

namespace NestWebApp.Areas.User.Controllers;

[Area("User")]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    [BindProperty]
    public ShoppingCartVM ShoppingCartVM { get; set; }
    public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Summary()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        ShoppingCartVM = new ShoppingCartVM()
        {
            OrderHeader = new OrderHeader(),
            ListCart = _context.ShoppingCart.Where(x => x.AppUserId == claim.Value).Include(x => x.Product)
        };
        return View(ShoppingCartVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Summary(ShoppingCartVM model)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        ShoppingCartVM.ListCart = _context.ShoppingCart.Where(x => x.AppUserId == claim.Value).Include(x => x.Product);
        ShoppingCartVM.OrderHeader.Status = Other.teklifBekliyor;
        ShoppingCartVM.OrderHeader.AppUserId = claim.Value;
        ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
        _context.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        _context.SaveChanges();
        foreach (var item in ShoppingCartVM.ListCart)
        {
            OrderDetails orderDetails = new OrderDetails()
            {
                ProductId = item.ProductId,
                OrderId = ShoppingCartVM.OrderHeader.Id,
                Count = item.Count
            };
            _context.OrderDetails.Add(orderDetails);
        }
        _context.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
        _context.SaveChanges();
        HttpContext.Session.SetInt32(Other.ssShopingCart, 0);
        return RedirectToAction("Ok");
    }
    public IActionResult Ok()
    {

        return View();
    }
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        ShoppingCartVM = new ShoppingCartVM()
        {
            OrderHeader = new OrderHeader(),
            ListCart = _context.ShoppingCart.Where(x => x.AppUserId == claim.Value).Include(x => x.Product)
        };
        ShoppingCartVM.OrderHeader.OrderTotalPrice = 0;
        ShoppingCartVM.OrderHeader.AppUser = _context.AppUser.FirstOrDefault(x => x.Id == claim.Value);
        return View(ShoppingCartVM);
    }
    public IActionResult Add(int cartId)
    {
        var cart = _context.ShoppingCart.FirstOrDefault(i => i.Id == cartId);
        cart.Count += 1;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    public IActionResult Decrease(int cartId)
    {
        var cart = _context.ShoppingCart.FirstOrDefault(i => i.Id == cartId);
        if (cart.Count == 1)
        {
            var count = _context.ShoppingCart.ToList().Count();
            _context.Remove(cart);
            _context.SaveChanges();
            HttpContext.Session.SetInt32(Other.ssShopingCart, count - 1);
        }
        else
        {
            cart.Count -= 1;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    public IActionResult Remove(int cartId)
    {
        var sessionValue = HttpContext.Session.GetString(Other.ssShopingCart);
        var cart = _context.ShoppingCart.FirstOrDefault(i => i.Id == cartId);
        var count = _context.ShoppingCart.Where(x => x.Id == cartId).ToList().Count();
        _context.Remove(cart);
        _context.SaveChanges();
        HttpContext.Session.SetInt32(Other.ssShopingCart, count - 1);
        return RedirectToAction("Index");
    }
    public IActionResult RemoveAll(int cartId)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var cart = _context.ShoppingCart.Where(x => x.AppUserId == claim.Value).ToList();
        _context.RemoveRange(cart);
        _context.SaveChanges();
        HttpContext.Session.SetInt32(Other.ssShopingCart, 0);
        return RedirectToAction("Index");
    }
}
