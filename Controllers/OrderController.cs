using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using Order = partner_aluro.Models.Order;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkOrder _unitOfWorkOrder;
        private readonly IUnitOfWorkAdress1rozliczeniowy _unitOfWorkAdress1rozliczeniowy;
        private readonly IUnitOfWorkAdress2dostawy _unitOfWorkAdress2dostawy;

        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;



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
        [HttpPost]
        public IActionResult PDFOrder(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var order = _orderService.GetOrder(id);


            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter write = PdfWriter.GetInstance(document, ms);
                document.Open();

                //Image image = iTextSharp.text.Image.GetInstance("wwwroot/images/logo/AluroLogoPdf_120x60.jpg");
                //image.SetAbsolutePosition(200, write.GetVerticalPosition(true));
                //document.Add(image);

                //Image image3 = iTextSharp.text.Image.GetInstance("wwwroot/images/logo/AluroLogoPdf_120x60.jpg");
                //image3.SetAbsolutePosition(write.GetVerticalPosition(true),200);
                //document.Add(image3);


                Image image2 = Image.GetInstance("wwwroot/images/logo/Aluro_logo-x-300_2.png");
                image2.ScaleAbsoluteWidth(150);
                image2.ScaleAbsoluteHeight(75);
                image2.SetAbsolutePosition(45, 730);
                document.Add(image2);


                string text1 = "Data zamówienia: ";
                string text2 = order.OrderPlaced.ToString("dd/MM/yyyy");
                //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD));

                Font bold = new Font(Font.FontFamily.HELVETICA, 10,Font.BOLD);
                Font regular = new Font(Font.FontFamily.HELVETICA, 10);
                Chunk c1 = new Chunk(text1, bold);
                Chunk c2 = new Chunk(text2, regular);   

                Paragraph para1 = new Paragraph();
                para1.Add(c1);
                para1.Add(c2);
                para1.Alignment = Element.ALIGN_RIGHT;
                document.Add(para1);


                Paragraph para2 = new Paragraph("ID # " + order.Id, new Font(Font.FontFamily.HELVETICA, 20,Font.BOLD));
                para2.Alignment = Element.ALIGN_RIGHT;
                para2.SpacingAfter = 40;
                document.Add(para2);

                Paragraph para3 = new Paragraph("NIP: " + order.adresRozliczeniowy.Vat.ToString(), new Font(Font.FontFamily.HELVETICA, 10));
                para3.Alignment = Element.ALIGN_CENTER;
                para3.SpacingAfter = 10;
                document.Add(para3);


                Paragraph para90 = new Paragraph("To jest paragraf3", new Font(Font.FontFamily.HELVETICA, 10));
                para90.Alignment = Element.ALIGN_CENTER;
                para90.SpacingAfter = 10;
                document.Add(para90);

                PdfPTable table = new PdfPTable(4);

                PdfPCell cell1 = new PdfPCell(new Phrase("Data", new Font(Font.FontFamily.HELVETICA, 10)));
                cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell1.BorderWidth = 1;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Kolumna2", new Font(Font.FontFamily.HELVETICA, 10)));
                cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell2.BorderWidth = 1;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Kolumna3", new Font(Font.FontFamily.HELVETICA, 10)));
                cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell3.BorderWidth = 1;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Adres", new Font(Font.FontFamily.HELVETICA, 10)));
                cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell4.BorderWidth = 1;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell4);

                for (int i = 0; i < 20; i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(i.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase((i + 1).ToString()));
                    PdfPCell cell_3 = new PdfPCell(new Phrase((i + 2).ToString()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase((i + 3).ToString()));

                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;

                    table.AddCell(cell_1);
                    table.AddCell(cell_2);
                    table.AddCell(cell_3);
                    table.AddCell(cell_4);
                }
                document.Add(table);
                document.Close();
                write.Close();
                var constant = ms.ToArray();

                return File(constant, "application/vnd", "Firstpdf.pdf");

            }
            return View();
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
            int id = order.Id;

            Order orders = _unitOfWorkOrder.OrderService.GetOrder(id);
            orders.StanZamowienia = order.StanZamowienia;
            _unitOfWorkOrder.OrderService.Update(orders);

            return RedirectToAction("Detail", new { id = id });
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






        [HttpPost]
        public IActionResult PDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter write = PdfWriter.GetInstance(document, ms);
                document.Open();

                var image = iTextSharp.text.Image.GetInstance("wwwroot/images/logo/Aluro_logo-x-300_2.png");
                image.Alignment = Element.ALIGN_CENTER;
                document.Add(image);

                Paragraph para1 = new Paragraph("To jest paragraf1", new Font(Font.FontFamily.HELVETICA, 20));
                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);

                Paragraph para2 = new Paragraph("To jest paragraf2", new Font(Font.FontFamily.HELVETICA, 20));
                para2.Alignment = Element.ALIGN_CENTER;
                document.Add(para2);

                Paragraph para3 = new Paragraph("To jest paragraf3", new Font(Font.FontFamily.HELVETICA, 20));
                para3.Alignment = Element.ALIGN_CENTER;
                para3.SpacingAfter = 10;
                document.Add(para3);

                PdfPTable table = new PdfPTable(4);

                PdfPCell cell1 = new PdfPCell(new Phrase("Data", new Font(Font.FontFamily.HELVETICA, 10)));
                cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell1.BorderWidth = 1;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Kolumna2", new Font(Font.FontFamily.HELVETICA, 10)));
                cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell2.BorderWidth = 1;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Kolumna3", new Font(Font.FontFamily.HELVETICA, 10)));
                cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell3.BorderWidth = 1;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Adres", new Font(Font.FontFamily.HELVETICA, 10)));
                cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell4.BorderWidth = 1;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell4);

                for (int i = 0; i < 20; i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(i.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase((i + 1).ToString()));
                    PdfPCell cell_3 = new PdfPCell(new Phrase((i + 2).ToString()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase((i + 3).ToString()));

                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;

                    table.AddCell(cell_1);
                    table.AddCell(cell_2);
                    table.AddCell(cell_3);
                    table.AddCell(cell_4);
                }
                document.Add(table);
                document.Close();
                write.Close();
                var constant = ms.ToArray();

                return File(constant, "application/vnd", "Firstpdf.pdf");

            }
            return View();
        }
    }
}
