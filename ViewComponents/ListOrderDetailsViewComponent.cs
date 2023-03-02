using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.ViewModels;

namespace partner_aluro.ListOrderDetailsViewComponent
{
    public class ListOrderDetailsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ListOrderDetailsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = _context.Orders.Where(x=>x.Id==id)
                .Include(x=>x.OrderItems)
                .ThenInclude(x=>x.Product)
                .First();
            return View(model);
        }
    }
}
