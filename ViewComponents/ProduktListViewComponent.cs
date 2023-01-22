using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.ViewComponents
{
    public class ProduktListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ProduktListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> list = await _context.Products
                .AsNoTracking()
                .ToListAsync();

            return View(list);
        }
    }
}
