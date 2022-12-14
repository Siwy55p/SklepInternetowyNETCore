using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;

namespace partner_aluro.ViewComponents
{
    public class MenuMainViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        //private readonly CategorySubCategory cat;
        public MenuMainViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var resul = await _context.Category.ToListAsync();

            var resul = await _context.Category
                .OrderBy(c => c.kolejnosc)
                .ToListAsync();
            //cat.SubCategory = await _context.SubCategory.ToListAsync();
            return View(resul);
        }

    }
}
