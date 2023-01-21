using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class CartHeaderViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        public CartHeaderViewComponent(Cart cart, ApplicationDbContext applicationDbContext)
        {
            _cart = cart;
            _context = applicationDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CartHeader model = new CartHeader
            {
                CartCount = await _context.CartItems.Where(ci => ci.CartIds == _cart.CartaId).CountAsync()
            };

            return View(model);
        }
    }
}
