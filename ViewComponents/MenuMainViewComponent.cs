using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;

namespace partner_aluro.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public MenuCategoryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var resul = await _context.Category
            .Include(sb => sb.SubCategories)
            .OrderBy(c => c.kolejnosc)
            .ToListAsync();
            return View(resul);
        }


    }
}
