using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class CartHeaderViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        public CartHeaderViewComponent(Cart cart, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IProfildzialalnosciService profildzialalnosciService)
        {
            _cart = cart;
            _context = applicationDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CartStatusModel model = new CartStatusModel();

            var products = await _cart.GetAllCartItemsAsync();
            _cart.CartItems = products;


            model.Cart = _cart;

            //model.User = _userManager.GetUserAsync(Request.HttpContext.User).Result;

            //int idProfil = 0;
            //if (model.User.IdProfilDzialalnosci != null)
            //{
            //    idProfil = (int)model.User.IdProfilDzialalnosci;
            //}

            //model.User.ProfilDzialalnosci = _profildzialalnosciService.GetProfil(idProfil);

            return View(model);
        }
    }
}
