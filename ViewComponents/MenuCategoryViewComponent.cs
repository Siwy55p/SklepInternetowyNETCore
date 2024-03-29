﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;

namespace partner_aluro.ViewComponents
{
    public class MenuMainViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuMainViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var resul = await _context.Category
                .AsNoTracking()
                .OrderBy(c => c.kolejnosc)
                .ToListAsync();
            return View(resul);
        }

    }
}
