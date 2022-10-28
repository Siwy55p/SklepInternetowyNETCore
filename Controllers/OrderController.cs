
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System;
using static NuGet.Packaging.PackagingConstants;
using Order = partner_aluro.Models.Order;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUnitOfWorkAdress1rozliczeniowy _unitOfWorkAdress1rozliczeniowy;
        private readonly IUnitOfWorkAdress2dostawy _unitOfWorkAdress2dostawy;

        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWorkOrder _unitOfWorkOrder;

        public OrderController(IUnitOfWorkOrder unitOfWorkOrder ,ApplicationDbContext context, Cart cart, UserManager<ApplicationUser> userManager, IOrderService orderService, IUnitOfWork unitOfWork, IUnitOfWorkAdress1rozliczeniowy unitOfWorkAdress1rozliczeniowy, IUnitOfWorkAdress2dostawy unitOfWorkAdress2dostawy)
        {
            _context = context;
            _cart = cart;

            _orderService = orderService;
            _userManager = userManager;

            _unitOfWork = unitOfWork;

            _unitOfWorkOrder = unitOfWorkOrder;

            _unitOfWorkAdress1rozliczeniowy = unitOfWorkAdress1rozliczeniowy;
            _unitOfWorkAdress2dostawy = unitOfWorkAdress2dostawy;
        }

        [HttpGet]
        public IActionResult Checkout()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Checkout(CartOrderViewModel CartOrder) //ZAPIS BARDZO WAZNA FUNKCJA
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

            if (_cart.CartItems.Count == 0 && _cart != null)
            {
                ModelState.AddModelError("", "Koszyk jest pusty, proszę dodać pierwszy produkt.");
            }
            var user = _unitOfWork.User.GetUser(CartOrder.Orders.User.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.Imie = CartOrder.Orders.User.Imie;
            user.Nazwisko = CartOrder.Orders.User.Nazwisko;
            user.Email = CartOrder.Orders.User.Email;

            _unitOfWork.User.UpdateUser(user); // Zapis 


            var nowyAdres1rozliczeniowy = _unitOfWorkAdress1rozliczeniowy.adress1Rozliczeniowy.Get(CartOrder.Orders.User.Id);
            nowyAdres1rozliczeniowy.Ulica = CartOrder.Orders.User.Adress1rozliczeniowy.Ulica;
            nowyAdres1rozliczeniowy.Kraj = CartOrder.Orders.User.Adress1rozliczeniowy.Kraj;
            nowyAdres1rozliczeniowy.Miasto = CartOrder.Orders.User.Adress1rozliczeniowy.Miasto;
            nowyAdres1rozliczeniowy.KodPocztowy = CartOrder.Orders.User.Adress1rozliczeniowy.KodPocztowy;
            nowyAdres1rozliczeniowy.Telefon = CartOrder.Orders.User.Adress1rozliczeniowy.Telefon;



            //nowyAdres1rozliczeniowy.UserID = CartOrder.Orders.User.Id;
            var nowyAdres2dostawy = _unitOfWorkAdress2dostawy.adress2dostawy.Get(CartOrder.Orders.User.Id);
            nowyAdres2dostawy.Ulica = CartOrder.Orders.User.Adress2dostawy.Ulica;
            nowyAdres2dostawy.Kraj = CartOrder.Orders.User.Adress2dostawy.Kraj;
            nowyAdres2dostawy.Miasto = CartOrder.Orders.User.Adress2dostawy.Miasto;
            nowyAdres2dostawy.KodPocztowy = CartOrder.Orders.User.Adress2dostawy.KodPocztowy;
            nowyAdres2dostawy.Email = CartOrder.Orders.User.Adress2dostawy.Email;
            nowyAdres2dostawy.Imie = CartOrder.Orders.User.Adress2dostawy.Imie;
            nowyAdres2dostawy.Nazwisko = CartOrder.Orders.User.Adress2dostawy.Nazwisko;
            //nowyAdres2dostawy.UserID = CartOrder.Orders.User.Id;


            Adress1rozliczeniowy OrderAdres1 = new Adress1rozliczeniowy();
            OrderAdres1.Ulica = CartOrder.Orders.User.Adress1rozliczeniowy.Ulica;
            OrderAdres1.Kraj = CartOrder.Orders.User.Adress1rozliczeniowy.Kraj;
            OrderAdres1.Miasto = CartOrder.Orders.User.Adress1rozliczeniowy.Miasto;
            OrderAdres1.KodPocztowy = CartOrder.Orders.User.Adress1rozliczeniowy.KodPocztowy;
            OrderAdres1.Telefon = CartOrder.Orders.User.Adress1rozliczeniowy.Telefon;
            OrderAdres1.NrNieruchomosci = nowyAdres1rozliczeniowy.NrNieruchomosci;
            OrderAdres1.NrLokalu = nowyAdres1rozliczeniowy.NrLokalu;
            OrderAdres1.Vat = nowyAdres1rozliczeniowy.Vat;
            OrderAdres1.Wojewodztwo = nowyAdres1rozliczeniowy.Wojewodztwo;
            OrderAdres1.Powiat = nowyAdres1rozliczeniowy.Powiat;
            OrderAdres1.Gmina = nowyAdres1rozliczeniowy.Gmina;
            OrderAdres1.Regon = nowyAdres1rozliczeniowy.Regon;
            OrderAdres1.UserID = user.Id;

            Adress2dostawy OrderAdres2 = new Adress2dostawy();
            OrderAdres2.Ulica = CartOrder.Orders.User.Adress2dostawy.Ulica;
            OrderAdres2.Kraj = CartOrder.Orders.User.Adress2dostawy.Kraj;
            OrderAdres2.Miasto = CartOrder.Orders.User.Adress2dostawy.Miasto;
            OrderAdres2.KodPocztowy = CartOrder.Orders.User.Adress2dostawy.KodPocztowy;
            OrderAdres2.Email = CartOrder.Orders.User.Adress2dostawy.Email;
            OrderAdres2.Imie = CartOrder.Orders.User.Adress2dostawy.Imie;
            OrderAdres2.Nazwisko = CartOrder.Orders.User.Adress2dostawy.Nazwisko;

            ModelState.Remove("Orders.UserID");
            ModelState.Remove("Carts");
            ModelState.Remove("Orders.User.Adress2dostawy.Adres2UserID");
            ModelState.Remove("Orders.User.Adress1rozliczeniowy.Adres1UserID");
            ModelState.Remove("Orders.User.Adres2.ApplicationUser");
            ModelState.Remove("Orders.User.ProfilDzialalnosci");
            ModelState.Remove("Orders.User.Adress1rozliczeniowy.Wojewodztwo");
 

            CartOrder.Orders.User.Adress1rozliczeniowy.Wojewodztwo = _context.Adress1rozliczeniowy.FirstOrDefault(x => x.Adres1rozliczeniowyId == user.Adress1rozliczeniowyId).Wojewodztwo;

            CartOrder.Orders.adresRozliczeniowy = OrderAdres1;
            CartOrder.Orders.AdressDostawy = OrderAdres2;

            ModelState.Remove("Orders.AdressDostawy");
            ModelState.Remove("Orders.adresRozliczeniowy");
            if (ModelState.IsValid)
            {
                _unitOfWorkAdress1rozliczeniowy.adress1Rozliczeniowy.Update(nowyAdres1rozliczeniowy);
                _unitOfWorkAdress2dostawy.adress2dostawy.Update(nowyAdres2dostawy);

                Order order = new Order();
                order.Komentarz = CartOrder.Orders.Komentarz;
                order.MessageToOrder = CartOrder.Orders.MessageToOrder;
                order.adresRozliczeniowy = OrderAdres1;
                order.AdressDostawy = OrderAdres2;


                CartOrder.Orders = order;

                CreateOrder(CartOrder.Orders);
                _cart.ClearCart();

                return View("CheckoutComplete", CartOrder.Orders);
            }
            else
            {
                var items = _cart.GetAllCartItems();
                _cart.CartItems = items;
                CartOrder.Carts = _cart;
                return View(CartOrder);

            }
        }

        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }


        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            var cartItems = _cart.CartItems;

            foreach (var item in cartItems)
            {

                item.Product.CenaProduktuDlaUzytkownika = item.Product.CenaProduktu * (1 - (Core.Constants.Rabat / 100));

                var orderItem = new OrderItem()
                {
                    Quantity = item.Quantity,
                    ProductId = item.Product.ProductId,
                    OrderId = order.Id,
                    Cena = (int)(item.Product.CenaProduktuDlaUzytkownika * item.Quantity)
                    //Cena = (int)(item.Product.CenaProduktu * item.Quantity)
                };
                
                order.UserID = _userManager.GetUserId(HttpContext.User);



                order.RabatZamowienia = Core.Constants.Rabat;

                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.Cena;
                order.StanZamowienia = StanZamowienia.Nowe;
            }
            _orderService.Add(order);
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        public IActionResult ListaZamowien() // To jest widok listy zamowien w panelu dashoboards
        {
            List<Order> orders = _orderService.ListOrdersAll();

            return View(orders);
        }
        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.User}")]
        public IActionResult ListaZamowienZalogowanegoUzytkownika() // To jest widok listy zamowien w panelu dashoboards
        {
            var UserID = _userManager.GetUserId(HttpContext.User);

            List<Order> orders = _orderService.ListOrdersUser(UserID);

            return View(orders);
        }

        [HttpPost]
        public IActionResult ZmienStatus(Order order)
        {
            //Wyslij e mail do klienta

            Order orders = _unitOfWorkOrder.OrderService.GetOrder(order.Id);
            orders.StanZamowienia = order.StanZamowienia;
            _unitOfWorkOrder.OrderService.Update(orders);

            return View(orders);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = _orderService.List(id);
            var order = _orderService.GetOrder(id);

            order.OrderItems = orderItems;


            ViewBag.StanyZamowienia = GetStanyZamowienia();

            var adres1 = _orderService.GetUserAdress1(order.UserID);
            var adres2 = _orderService.GetUserAdress2(order.UserID);
            order.User.Adress1rozliczeniowy = adres1;
            order.User.Adress2dostawy = adres2;

            return View(order);
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        [HttpPost]
        public IActionResult ZapiszNotatke(Order order)
        {

            var user = _unitOfWork.User.GetUser(order.User.Id);

            user.NotatkaOsobista = order.User.NotatkaOsobista;

            _unitOfWork.User.UpdateUser(user);

            int id = order.Id;

            //user.NotatkaOsobista = notatka;
            return RedirectToAction("Detail", new {id = id});
        }




        private List<SelectListItem> GetStanyZamowienia()
        {
            var lstStanZamowien = new List<SelectListItem>();

            foreach (StanZamowienia suit in (StanZamowienia[])Enum.GetValues(typeof(StanZamowienia)))
            {
                var dmyItemA = new SelectListItem()
                {
                    Value = suit.ToString(),
                    Text = suit.ToString()
                };
                lstStanZamowien.Insert(0, dmyItemA);
            }

            return lstStanZamowien;
        }
    }
}
