using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.DAL.Context;
using NestWebApp.Models.StaticClasses;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace NestWebApp.Areas.User.Controllers;

[Area("User")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("")]
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null)
        {
            var count = _context.ShoppingCart.Where(x => x.AppUserId == claim.Value).ToList().Count();
            HttpContext.Session.SetInt32(Other.ssShopingCart, count);
        }
        return View();
    }
    [Route("/hakkimizda")]
    public async Task<IActionResult> AboutUs()
    {
        return View(await _context.AboutUs.AsNoTracking().FirstOrDefaultAsync());
    }
    [Route("/markalarimiz")]
    public async Task<IActionResult> Brands()
    {
        return View(await _context.Brand.AsNoTracking().ToListAsync());
    }
    [Route("/iletisim")]
    public async Task<IActionResult> Contact()
    {
        return View(await _context.Contact.AsNoTracking().FirstOrDefaultAsync());
    }
    [HttpPost]
    [Route("/iletisim")]
    public IActionResult Contact(string NameSurname, string Email, string PhoneNumber, string Subject, string Message)
    {
        var mailSettings = _context.MailSettings.FirstOrDefault();
        if (mailSettings != null)
        {
            if (NameSurname != null && Email != null && PhoneNumber != null && Subject != null && Message != null)
            {
                MailMessage msg = new MailMessage();
                msg.Subject = "Yeni bir iletişim mesajı";
                msg.From = new MailAddress(mailSettings.FromEmailAddress, mailSettings.FromEmailAddressDisplayName);
                msg.To.Add(new MailAddress(mailSettings.SendEmailAddress, mailSettings.SendEmailAddressDisplayName));
                msg.IsBodyHtml = true;
                msg.Body =
                    "Ad Soyad: " + NameSurname +
                    "<br>" + "Email: " + Email +
                    "<br>" + "Telefon Numarası: " + PhoneNumber +
                    "<br>" + "Konu: " + Subject +
                    "<br>" + "Mesaj: " + Message;
                msg.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient(mailSettings.SmtpHost, Int32.Parse(mailSettings.SmtpPort));
                NetworkCredential AccountInfo = new NetworkCredential(mailSettings.EmailAddress, mailSettings.EmailAddressPassword);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = AccountInfo;
                smtp.EnableSsl = false;
                try
                {
                    smtp.Send(msg);
                    return RedirectToAction("Contact");
                }
                catch (Exception)
                {
                    return RedirectToAction("Contact");
                }
            }
            else
            {
            }
        }
        return RedirectToAction("Contact");
    }

    public IActionResult Error1(int code)
    {
        return View();
    }
}
