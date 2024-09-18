using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NestWebApp.BL.Repositories;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class EmailController : Controller
{
    private readonly IRepository<MailSettings> _context;
    public EmailController(IRepository<MailSettings> context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult Edit()
    {
        var data = _context.GetAll();
        if (data == null)
        {
            return RedirectToAction("Create");
        }
        else
        {
            return View(data);
        }
    }
    [HttpPost]
    public IActionResult Edit(MailSettings p)
    {
        _context.Update(p);
        return RedirectToAction("Edit");
    }
    [HttpGet]
    public IActionResult Create()
    {

        var data = _context.GetAll();
        if (data == null)
        {
            return View();
        }
        else
        {
            return RedirectToAction("Edit");
        }
    }
    [HttpPost]
    public IActionResult Create(MailSettings p)
    {
        _context.Add(p);
        return RedirectToAction("Edit");
    }
}
