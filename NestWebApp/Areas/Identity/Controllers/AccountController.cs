using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;
using NestWebApp.Models.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace NestWebApp.Areas.Identity.Controllers;

[Area("Identity")]
public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                NameSurname = registerVM.NameSurname,
                PhoneNumber = registerVM.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(Role.Role_Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Role_Admin));
                }
                if (!await _roleManager.RoleExistsAsync(Role.Role_User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Role_User));
                }
                //await _userManager.AddToRoleAsync(user, Role.Role_Admin);
                await _userManager.AddToRoleAsync(user, Role.Role_User);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(registerVM);
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            var roles = await _userManager.GetRolesAsync(user);

            var count = await _context.ShoppingCart.Where(x => x.AppUserId == user.Id).CountAsync();
            HttpContext.Session.SetInt32(Other.ssShopingCart, count);
            if (roles.Contains(Role.Role_Admin))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (roles.Contains(Role.Role_User))
            {
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            return RedirectToAction("Login");
        }

        return View(loginVM);
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, protocol: HttpContext.Request.Scheme);

            var mailSettings = await _context.MailSettings.FirstOrDefaultAsync();
            if (mailSettings != null)
            {
                MailMessage msg = new MailMessage();
                msg.Subject = "Şifremi Unuttum";
                msg.From = new MailAddress(mailSettings.FromEmailAddress, mailSettings.FromEmailAddressDisplayName);
                msg.To.Add(new MailAddress(email, mailSettings.SendEmailAddressDisplayName));
                msg.IsBodyHtml = true;
                msg.Body =
                    "Şifrenizi sıfırlamak için lütfen: " + callbackUrl;
                msg.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient(mailSettings.SmtpHost, Int32.Parse(mailSettings.SmtpPort));
                NetworkCredential AccountInfo = new NetworkCredential(mailSettings.EmailAddress, mailSettings.EmailAddressPassword);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = AccountInfo;
                smtp.EnableSsl = false;
                try
                {
                    smtp.Send(msg);
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                catch
                {

                }
            }
        }

        // Kullanıcıya başarılı bir işlem yapıldığını belirten bir mesaj göster
        return View("ForgotPasswordConfirmation");
    }
    public async Task<IActionResult> PriceOffers()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var data = await _context.OrderHeader.Where(x => x.AppUserId == claim.Value).Include(x => x.OrderDetails).ThenInclude(x => x.Product).AsNoTracking().ToListAsync();
        return View(data);
    }
    public async Task<IActionResult> PriceOfferDetails(int id)
    {
        return View(await _context.OrderHeader.Where(x => x.Id == id).Include(x => x.AppUser).Include(x => x.OrderDetails).ThenInclude(x => x.Product).AsNoTracking().FirstOrDefaultAsync());
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
}
