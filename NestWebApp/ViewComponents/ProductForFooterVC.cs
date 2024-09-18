﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestWebApp.Models.Data;

namespace NestWebApp.ViewComponents
{
    public class ProductForFooterVC : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ProductForFooterVC(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Product.Where(x => x.IsDeleted == false).AsNoTracking().Take(7).ToListAsync());
        }
    }
}