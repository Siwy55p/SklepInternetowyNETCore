using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class OrderStatusViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            OrderStatusModel model = new OrderStatusModel();

            model.Orders = await _context.Orders.Where(x => x.StanZamowienia == StanZamowienia.Nowe).ToListAsync();


            return View(model);
        }
    }
}
