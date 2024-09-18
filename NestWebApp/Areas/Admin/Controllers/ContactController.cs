using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NestWebApp.BL.Repositories;
using NestWebApp.DAL.Models;
using NestWebApp.Models.StaticClasses;

namespace NestWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.Role_Admin)]
public class ContactController : Controller
{
    private readonly IRepository<Contact> _context;
    public ContactController(IRepository<Contact> context)
    {
        _context = context;
    }
    public IActionResult Create()
    {
        var data = _context.GetAll().FirstOrDefault();
        if (data != null)
        {
            return RedirectToAction("Edit");
        }
        return View(data);
    }
    [HttpPost]
    public IActionResult Create(Contact p)
    {
        _context.Add(p);
        return RedirectToAction("Edit");
    }
    public IActionResult Edit()
    {
        var data = _context.GetAll().FirstOrDefault();
        if (data == null)
        {
            return RedirectToAction("Create");
        }
        return View(data);
    }
    [HttpPost]
    public IActionResult Edit(Contact p)
    {
        _context.Update(p);
        return RedirectToAction("Edit");
    }
}

