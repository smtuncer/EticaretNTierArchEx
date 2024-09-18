using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NestWebApp.BL.Repositories;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class AboutUsController : Controller
{
    private readonly IRepository<AboutUs> _context;
    public AboutUsController(IRepository<AboutUs> context)
    {
        _context = context;
    }
    public IActionResult Create()
    {
        var data = _context.GetAll();
        if (data != null)
        {
            return RedirectToAction("Edit");
        }
        return View(data);
    }
    [HttpPost]
    public IActionResult Create(AboutUs p)
    {
        _context.Add(p);
        return RedirectToAction("Edit");
    }
    public IActionResult Edit()
    {
        var data = _context.GetAll();
        if (data == null)
        {
            return RedirectToAction("Create");
        }
        return View(data);
    }
    [HttpPost]
    public IActionResult Edit(AboutUs p)
    {
        _context.Update(p);
        return RedirectToAction("Edit");
    }
}
