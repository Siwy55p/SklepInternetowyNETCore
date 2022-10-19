using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System.Security.Claims;

namespace partner_aluro.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(Cart cart, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _cart = cart;
            _context = applicationDbContext;

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public IActionResult Return(string returnUrl)
        {
            return Redirect(returnUrl);
        }


        public async Task<IActionResult> Index()
        {
            var products = _cart.GetAllCartItems();
            _cart.CartItems = products;

            var returnUrl = Request.Headers["Referer"].ToString();

            return Redirect(returnUrl);

            //return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ZlozZamowienie()
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var returnUrl = Request.Headers["Referer"].ToString();

            var products = _cart.GetAllCartItems();
            _cart.CartItems = products;

            foreach (var product in products)
            {
                product.Product.CenaProduktuDlaUzytkownika = product.Product.CenaProduktu * (1 - (Core.Constants.Rabat / 100));
            }


            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            CartOrderViewModel vm = new CartOrderViewModel
            {
                Carts = _cart,
                Orders = new Order() { User = applicationUser },
            };


            Adress1rozliczeniowy adresRozliczeniowy = new Adress1rozliczeniowy();
            vm.Orders.User.Adres1 = _context.Adress1rozliczeniowy.Where(a => a.UserID == Core.Constants.UserId).FirstOrDefault();

            if (vm.Orders.User.Adres1 == null)
            {
                vm.Orders.User.Adres1 = new Adress1rozliczeniowy
                {
                    KodPocztowy = "00000",
                    Miasto = "",
                    Kraj = "",
                    Telefon = "123123123"
                };
            }

            vm.Orders.User.Adres2 = _context.Adress2dostawy.Where(a => a.UserID == Core.Constants.UserId).FirstOrDefault();
            if (vm.Orders.User.Adres2 == null)
            {
                vm.Orders.User.Adres2 = new Adress2dostawy
                {
                    KodPocztowy = vm.Orders.User.Adres1.KodPocztowy,
                    Miasto = vm.Orders.User.Adres1.Miasto,
                    Kraj = vm.Orders.User.Adres1.Kraj,
                    Telefon = vm.Orders.User.Adres1.Telefon
                };
            }

            return View(vm);
        }


        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var selectedProduct = await GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.AddToCart(selectedProduct, quantity);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var selectedProduct = await GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.RemoveFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ReduceQuantity(int id)
        {
            var selectedProduct = await GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.ReduceQuantity(selectedProduct);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var selectedProduct = await GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.IncreaseQuantity(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction("Index");
        }

        public async Task<Product> GetProductId(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}
