using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private readonly IUnitOfWorkAdress1rozliczeniowy _unitOfWorkAdress1rozliczeniowy;
        private readonly IUnitOfWorkAdress2dostawy _unitOfWorkAdress2dostawy;


        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(Cart cart, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IUnitOfWorkAdress1rozliczeniowy unitOfWorkAdress1Rozliczeniowy, IUnitOfWorkAdress2dostawy unitOfWorkAdress2Dostawy)
        {
            _cart = cart;
            _context = applicationDbContext;

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

            _unitOfWorkAdress1rozliczeniowy = unitOfWorkAdress1Rozliczeniowy;
            _unitOfWorkAdress2dostawy = unitOfWorkAdress2Dostawy;
        }

        [HttpPost]
        public IActionResult Return(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Lista()
        {

            List<CartItem> lista = _context.CartItems.Include(p => p.Product).Include(u => u.User).ToList();

            IEnumerable<IGrouping<string, CartItem>> g = lista.GroupBy(b => b.CartId);

            CartOrderViewModel vm = new CartOrderViewModel
            {
                cartItems = lista,
                group = g
            };

            //lista.GroupBy(x => x.CartId, (x, y) => new
            //{
            //    y.CartId = x.CartId,

            //}).ToList();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string CartId)
        {

            List<CartItem> lista = _context.CartItems.Include(p => p.Product).Include(u => u.User).ToList();

            IEnumerable<IGrouping<string, CartItem>> g = lista.GroupBy(b => b.CartId);

            CartOrderViewModel vm = new CartOrderViewModel
            {
                cartItems = lista,
                group = g
            };

            //lista.GroupBy(x => x.CartId, (x, y) => new
            //{
            //    y.CartId = x.CartId,

            //}).ToList();

            return View(vm);
        }


        public async Task<IActionResult> Index()
        {
            _cart.CartItems = await _cart.GetAllCartItemsAsync();

            //ViewData["MetodyPlatnosci"] = OrderController.GetMetodyPlatnosci();
            ViewData["MetodyPlatnosci"] = _context.MetodyPlatnosci.ToList();
            ViewData["MetodyDostawy"] = _context.MetodyDostawy.ToList();


            var returnUrl = Request.Headers["Referer"].ToString();

            return Redirect(returnUrl);

            //return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ZlozZamowienie()
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var returnUrl = Request.Headers["Referer"].ToString();

            //ViewData["MetodyPlatnosci"] = OrderController.GetMetodyPlatnosci();
            ViewData["MetodyPlatnosci"] = _context.MetodyPlatnosci.ToList();
            ViewData["MetodyDostawy"] = _context.MetodyDostawy.ToList();

            var products = await _cart.GetAllCartItemsAsync();
            //_cart.CartItems = products;

            //foreach (var product in products)
            //{
            //    product.Product.CenaProduktuDlaUzytkownika = product.Product.CenaProduktu * (1 - (Core.Constants.Rabat / 100));
            //}


            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            CartOrderViewModel vm = new()
            {
                Carts = _cart,
                Orders = new Order() { 
                    User = applicationUser,
                    MetodaPlatnosci = "Przelew"
                },
            };


            vm.Orders.User.Adress1rozliczeniowy = _context.Adress1rozliczeniowy.FirstOrDefault(x => x.UserID == vm.Orders.User.Id);
            vm.Orders.User.Adress2dostawy = _context.Adress2dostawy.FirstOrDefault(x => x.UserID == vm.Orders.User.Id);

            if (vm.Orders.User.Adress1rozliczeniowy == null)
            {
                vm.Orders.User.Adress1rozliczeniowy = new Adress1rozliczeniowy
                {
                    KodPocztowy = "",
                    Miasto = "",
                    Kraj = "",
                    Telefon = ""
                };
            }


            vm.Orders.adresRozliczeniowy = _unitOfWorkAdress1rozliczeniowy.adress1Rozliczeniowy.Get(applicationUser.Id);

            if (vm.Orders.adresRozliczeniowy == null)
            {
                vm.Orders.adresRozliczeniowy = new Adress1rozliczeniowy
                {
                    KodPocztowy = "",
                    Miasto = "",
                    Kraj = "",
                    Ulica = "",
                    Telefon = ""
                };

            }


            if (vm.Orders.User.Adress2dostawy == null)
            {
                Adress2dostawy adres2 = new Adress2dostawy()
                {
                    KodPocztowy = "",
                    Miasto = "",
                    Kraj = "",
                    Telefon = "",
                    Ulica = ""
                };
                vm.Orders.User.Adress2dostawy = adres2;
            }


            vm.Orders.AdressDostawy = _unitOfWorkAdress2dostawy.adress2dostawy.Get(applicationUser.Id);

            if (vm.Orders.AdressDostawy == null)
            {
                Adress2dostawy adres2 = new Adress2dostawy()
                {
                    KodPocztowy = "",
                    Miasto = "",
                    Kraj = "",
                    Ulica = "",
                    Telefon = ""
                };
                vm.Orders.AdressDostawy = adres2;
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
        public async Task AddToCart2(int ProductId, int quantity)
        {
            var selectedProduct = await GetProductId(ProductId);

            if (selectedProduct != null)
            {
                _cart.AddToCart(selectedProduct, quantity);
            }

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
        public async Task RemoveFromCart2(int id)
        {
            var selectedProduct = await GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.RemoveFromCart(selectedProduct);
            }

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
        public async Task ReduceQuantity2(int id)
        {
            var selectedProduct = await GetProductId(id);



            if (selectedProduct != null)
            {
                _cart.ReduceQuantity(selectedProduct);
            }

        }

        public async Task<IActionResult> IncreaseQuantity(int id, int Quantity)
        {
            var selectedProduct = await GetProductId(id);
            if (Quantity < selectedProduct.Ilosc)
            {
                if (selectedProduct != null)
                {
                    _cart.IncreaseQuantity(selectedProduct);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task IncreaseQuantity2(int id, int Quantity)
        {
            var selectedProduct = await GetProductId(id);
            if (Quantity < selectedProduct.Ilosc)
            {
                if (selectedProduct != null)
                {
                    _cart.IncreaseQuantity(selectedProduct);
                }
            }

        }
        public IActionResult ClearCart()
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
