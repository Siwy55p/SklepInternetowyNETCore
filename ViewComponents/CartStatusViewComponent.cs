using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class CartStatusViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        public CartStatusViewComponent(Cart cart, ApplicationDbContext applicationDbContext)
        {
            _cart = cart;
            _context = applicationDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CartStatusModel model = new CartStatusModel();

            _cart.CartItems = await _cart.GetAllCartItemsAsync();

            model.Cart = _cart;
            model.Cart.GetCartTotalBrutto();
            model.Cart.GetCartTotalNetto();

            return View(model);
        }
    }
}
