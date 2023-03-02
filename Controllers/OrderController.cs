
using iText.IO.Image;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System.ComponentModel.DataAnnotations;

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

        private readonly IProductService _productService;

        private readonly IEmailService _emailService;


        public OrderController(IProductService productService, IUnitOfWorkOrder unitOfWorkOrder, ApplicationDbContext context, Cart cart, UserManager<ApplicationUser> userManager, IOrderService orderService, IUnitOfWork unitOfWork, IUnitOfWorkAdress1rozliczeniowy unitOfWorkAdress1rozliczeniowy, IUnitOfWorkAdress2dostawy unitOfWorkAdress2dostawy, IEmailService emailService)
        {
            _context = context;
            _cart = cart;

            _orderService = orderService;
            _userManager = userManager;

            _unitOfWork = unitOfWork;
            _unitOfWorkOrder = unitOfWorkOrder;

            _unitOfWorkAdress1rozliczeniowy = unitOfWorkAdress1rozliczeniowy;
            _unitOfWorkAdress2dostawy = unitOfWorkAdress2dostawy;

            _productService = productService;

            _emailService = emailService;
        }


        protected string GenerateID()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

                characters += alphabets + small_alphabets + numbers;

            int length = 5;
            string id = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (id.IndexOf(character) != -1);
                id += character;
            }
            return  id;
        }



        [HttpGet]
        public IActionResult Checkout()
        {
            //ViewData["MetodyPlatnosci"] = GetMetodyPlatnosci();
            ViewData["MetodyPlatnosci"] = _context.MetodyPlatnosci.ToList();
            ViewData["MetodyDostawy"] = _context.MetodyDostawy.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartOrderViewModel CartOrder) //ZAPIS BARDZO WAZNA FUNKCJA
        {
            //ViewData["MetodyPlatnosci"] = GetMetodyPlatnosci();
            ViewData["MetodyPlatnosci"] = _context.MetodyPlatnosci.ToList();
            ViewData["MetodyDostawy"] = _context.MetodyDostawy.ToList();

            var cartItems = await _cart.GetAllCartItemsAsync();
            _cart.CartItems = cartItems;
            ModelState.Remove("Orders.UserID");
            ModelState.Remove("Orders.group");

            if (CartOrder.Orders.AdresDostawyInny == false)
            {

                ModelState.Remove("Orders.AdressDostawy.Ulica");
                CartOrder.Orders.AdressDostawy.Ulica = CartOrder.Orders.adresRozliczeniowy.Ulica;
                ModelState.Remove("Orders.AdressDostawy.Miasto");
                CartOrder.Orders.AdressDostawy.Miasto = CartOrder.Orders.adresRozliczeniowy.Miasto;
                ModelState.Remove("Orders.AdressDostawy.KodPocztowy");
                CartOrder.Orders.AdressDostawy.KodPocztowy = CartOrder.Orders.adresRozliczeniowy.KodPocztowy;


            }

            if (!ModelState.IsValid)
            {
                CartOrder.Carts = _cart;

                return View(CartOrder);
            }


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

            if (nowyAdres1rozliczeniowy == null)
            {
                nowyAdres1rozliczeniowy = new Adress1rozliczeniowy();
                nowyAdres1rozliczeniowy.UserID = CartOrder.Orders.User.Id;
                nowyAdres1rozliczeniowy.Adres1UserID = CartOrder.Orders.User.Id;
                nowyAdres1rozliczeniowy.Ulica = CartOrder.Orders.adresRozliczeniowy.Ulica;
                nowyAdres1rozliczeniowy.Kraj = CartOrder.Orders.adresRozliczeniowy.Kraj;
                nowyAdres1rozliczeniowy.Miasto = CartOrder.Orders.adresRozliczeniowy.Miasto;
                nowyAdres1rozliczeniowy.KodPocztowy = CartOrder.Orders.adresRozliczeniowy.KodPocztowy;
                nowyAdres1rozliczeniowy.Telefon = CartOrder.Orders.adresRozliczeniowy.Telefon;
                nowyAdres1rozliczeniowy.Vat = CartOrder.Orders.adresRozliczeniowy.Vat;
                nowyAdres1rozliczeniowy.Regon = CartOrder.Orders.adresRozliczeniowy.Regon;

                _context.Adress1rozliczeniowy.Add(nowyAdres1rozliczeniowy);
                _context.SaveChanges();
            }
            else
            {
                nowyAdres1rozliczeniowy.UserID = CartOrder.Orders.User.Id;
                nowyAdres1rozliczeniowy.Adres1UserID = CartOrder.Orders.User.Id;
                nowyAdres1rozliczeniowy.Ulica = CartOrder.Orders.adresRozliczeniowy.Ulica;
                nowyAdres1rozliczeniowy.Kraj = CartOrder.Orders.adresRozliczeniowy.Kraj;
                nowyAdres1rozliczeniowy.Miasto = CartOrder.Orders.adresRozliczeniowy.Miasto;
                nowyAdres1rozliczeniowy.KodPocztowy = CartOrder.Orders.adresRozliczeniowy.KodPocztowy;
                nowyAdres1rozliczeniowy.Telefon = CartOrder.Orders.adresRozliczeniowy.Telefon;
                nowyAdres1rozliczeniowy.Vat = CartOrder.Orders.adresRozliczeniowy.Vat;
                nowyAdres1rozliczeniowy.Regon = CartOrder.Orders.adresRozliczeniowy.Regon;
                _context.Adress1rozliczeniowy.Update(nowyAdres1rozliczeniowy);
                _context.SaveChanges();
            }


            //nowyAdres1rozliczeniowy.UserID = CartOrder.Orders.User.Id;
            var nowyAdres2dostawy = _unitOfWorkAdress2dostawy.adress2dostawy.Get(CartOrder.Orders.User.Id);

            if (nowyAdres2dostawy == null)
            {
                nowyAdres2dostawy = new Adress2dostawy();
                nowyAdres2dostawy.UserID = CartOrder.Orders.User.Id;
                nowyAdres2dostawy.Adres2UserID = CartOrder.Orders.User.Id;
                nowyAdres2dostawy.Ulica = CartOrder.Orders.AdressDostawy.Ulica;
                nowyAdres2dostawy.Kraj = CartOrder.Orders.AdressDostawy.Kraj;
                nowyAdres2dostawy.Miasto = CartOrder.Orders.AdressDostawy.Miasto;
                nowyAdres2dostawy.KodPocztowy = CartOrder.Orders.AdressDostawy.KodPocztowy;
                nowyAdres2dostawy.Telefon = CartOrder.Orders.AdressDostawy.Telefon;
                nowyAdres2dostawy.Email = CartOrder.Orders.AdressDostawy.Email;
                nowyAdres2dostawy.Imie = CartOrder.Orders.AdressDostawy.Imie;
                nowyAdres2dostawy.Nazwisko = CartOrder.Orders.AdressDostawy.Nazwisko;
                _context.Adress2dostawy.Add(nowyAdres2dostawy);
                _context.SaveChanges();
            }
            else
            {
                nowyAdres2dostawy.UserID = CartOrder.Orders.User.Id;
                nowyAdres2dostawy.Adres2UserID = CartOrder.Orders.User.Id;
                nowyAdres2dostawy.Ulica = CartOrder.Orders.AdressDostawy.Ulica;
                nowyAdres2dostawy.Kraj = CartOrder.Orders.AdressDostawy.Kraj;
                nowyAdres2dostawy.Miasto = CartOrder.Orders.AdressDostawy.Miasto;
                nowyAdres2dostawy.KodPocztowy = CartOrder.Orders.AdressDostawy.KodPocztowy;
                nowyAdres2dostawy.Email = CartOrder.Orders.AdressDostawy.Email;
                nowyAdres2dostawy.Telefon = CartOrder.Orders.AdressDostawy.Telefon;
                nowyAdres2dostawy.Imie = CartOrder.Orders.AdressDostawy.Imie;
                nowyAdres2dostawy.Nazwisko = CartOrder.Orders.AdressDostawy.Nazwisko;
                _context.Adress2dostawy.Update(nowyAdres2dostawy);
                _context.SaveChanges();
            }

            Adress1rozliczeniowy OrderAdres1 = new()
            {
                Ulica = CartOrder.Orders.adresRozliczeniowy.Ulica,
                Kraj = CartOrder.Orders.adresRozliczeniowy.Kraj,
                Miasto = CartOrder.Orders.adresRozliczeniowy.Miasto,
                KodPocztowy = CartOrder.Orders.adresRozliczeniowy.KodPocztowy,
                Telefon = CartOrder.Orders.adresRozliczeniowy.Telefon,
                NrNieruchomosci = nowyAdres1rozliczeniowy.NrNieruchomosci,
                NrLokalu = nowyAdres1rozliczeniowy.NrLokalu,
                Vat = nowyAdres1rozliczeniowy.Vat,
                Wojewodztwo = nowyAdres1rozliczeniowy.Wojewodztwo,
                Powiat = nowyAdres1rozliczeniowy.Powiat,
                Gmina = nowyAdres1rozliczeniowy.Gmina,
                Regon = nowyAdres1rozliczeniowy.Regon,
                UserID = user.Id
            };

            Adress2dostawy OrderAdres2 = new()
            {
                Ulica = CartOrder.Orders.AdressDostawy.Ulica,
                Kraj = CartOrder.Orders.AdressDostawy.Kraj,
                Miasto = CartOrder.Orders.AdressDostawy.Miasto,
                KodPocztowy = CartOrder.Orders.AdressDostawy.KodPocztowy,
                Telefon = CartOrder.Orders.AdressDostawy.Telefon,
                Email = CartOrder.Orders.AdressDostawy.Email,
                Imie = CartOrder.Orders.AdressDostawy.Imie,
                Nazwisko = CartOrder.Orders.AdressDostawy.Nazwisko,
                UserID = user.Id
            };

            ModelState.Remove("Orders.UserID");
            ModelState.Remove("Carts");
            ModelState.Remove("Orders.User.Adress2dostawy.Adres2UserID");
            ModelState.Remove("Orders.User.Adress1rozliczeniowy.Adres1UserID");
            ModelState.Remove("Orders.User.Adres2.ApplicationUser");
            ModelState.Remove("Orders.User.ProfilDzialalnosci");
            ModelState.Remove("Orders.User.Adress1rozliczeniowy.Wojewodztwo");


            //CartOrder.Orders.User.Adress1rozliczeniowy.Wojewodztwo = _context.Adress1rozliczeniowy.FirstOrDefault(x => x.Adres1rozliczeniowyId == user.).Wojewodztwo;

            CartOrder.Orders.adresRozliczeniowy = OrderAdres1;
            CartOrder.Orders.AdressDostawy = OrderAdres2;


            if (ModelState.IsValid)
            {
                _unitOfWorkAdress1rozliczeniowy.adress1Rozliczeniowy.Update(nowyAdres1rozliczeniowy);
                _unitOfWorkAdress2dostawy.adress2dostawy.Update(nowyAdres2dostawy);

                Order order = new()
                {
                    Komentarz = CartOrder.Orders.Komentarz,
                    MessageToOrder = CartOrder.Orders.MessageToOrder,
                    adresRozliczeniowy = OrderAdres1,
                    AdressDostawy = OrderAdres2,
                    MetodaDostawy = CartOrder.Orders.MetodaDostawy,
                    MetodaPlatnosci = CartOrder.Orders.MetodaPlatnosci

                };
                order.NrZamowienia = GenerateID();

                CartOrder.Orders = order;

                CreateOrder(CartOrder.Orders); // Stworz zamowienie jesli sie udalo to wyslij email
                
                    //_cart.ClearCart();


                    EmailDto email = new EmailDto()
                    {
                        To = user.Email,
                        Subject = "Dziękujemy za złożenie zamówienia. Nr: #" + order.NrZamowienia + "",
                        Body = $"<h1> Dziękujemy za złożenie zamówienia.</h1>Potwierdzenie zamówienia <br/>Twoje zamówienie zostało przyjęte.<br/>Po skompletowaniu Twojego zamówienia otrzymasz email z kosztem dostawy i łączną sumą do zapłaty.<br/>W razie pytań lub wątpliwości, prosimy o kontakt z naszą obsługą klienta.<br/>" +
                        "Sposób dostawy: <b>" + order.MetodaDostawy + "</b>, metoda płatności: <b>" + order.MetodaPlatnosci + "</b>"
                    };
                    _emailService.SendEmailAsync(email);


                    string startEmail = $"Nowe zamówienie na platformie Aluro Nr: #" + order.NrZamowienia + "<br>" +
                        "Zamówienie Nr: <b>" + order.NrZamowienia + "</b>, Metoda płatności: <b>" + order.MetodaPlatnosci + "</b> , Metoda dostawy: <b>" + order.MetodaDostawy + "</b><br>" +
                        "Data złożenia zamówienia: <b>" + order.OrderPlaced + "</b><br>" +
                        "Nazwa firmy: <b>" + user.NazwaFirmy + "</b><br>" +
                        "Imie Nazwisko: <b>" + user.Imie + " " + user.Nazwisko + "</b><br>" +
                        "Adres rozliczeniowy: <br>" +
                        "" + OrderAdres1.KodPocztowy + " " + OrderAdres1.Miasto + " <br>" +
                        "ul. " + OrderAdres1.Ulica + "<br>" +
                        "Adres dostawy: <br>" +
                        "" + OrderAdres2.KodPocztowy + " " + OrderAdres2.Miasto + "<br>" +
                        "ul. " + OrderAdres2.Ulica + "<br>" +
                        "Kontakt do Adresu dostawy: <br>" + OrderAdres2.Telefon + ", e-mail: " + OrderAdres2.Email + "<br>" +
                        "Wiadomosc do zamówienia: " + order.Komentarz + " " + order.MessageToOrder + "<br>" +
                        "Kontakt: " + user.Email + " " + user.Adress1rozliczeniowy.Telefon + "<br>" +
                        "Wartość zamówienia: " + order.OrderTotal + " zł<br>" +
                        "Zamówienie ID:#" + order.NrZamowienia + "<br>" +
                        "<table style=\"border:1px solid black\">" +
                        "<thead style=\"border:1px solid greey\"><tr><th>Nazwa produktu</th>" +
                        "<th>Symbol</th>" +
                        "<th>CenaTotal jed. pro.</th>" +
                        "<th>Ilość</th>" +
                        "<th>Razem</th>" +
                        "</tr>" +
                        "</thead>" +
                        "<tbody style=\"border:1px solid greey\">";

                    string ProductList = "";
                    foreach (var item in order.OrderItems)
                    {

                        ProductList += "<tr>" +

                            "<td>" + @item.Product.Name + "</td>" +
                            "<td>" + @item.Product.Symbol + "</td>" +
                            "<td>" + @item.CenaJednProductuBrutto.ToString("0.00") + "</td>" +
                            "<td>" + @item.Quantity.ToString("#") + "</td>" +
                            "<td>" + (@item.CenaTotal).ToString("## ###.00") + "</td>" +
                            "</tr>";

                    }

                    string endEmail = "</tbody><tfoot><tr><th></th><th></th><th></th><th>Suma:</th><th>" + order.OrderTotal.ToString("## ###.00") + "</th></tr></tfoot></table>";


                    string emailMessage = startEmail + ProductList + endEmail;
                    //EmailDto emailDzialTechniczny1 = new EmailDto()
                    //{
                    //    To = "marcin@aluro.pl",
                    //    Subject = "Nowe zamówienie! " + order.Id + ":" + user.Imie + " " + user.Nazwisko + " " + user.NazwaFirmy + "",
                    //    Body = emailMessage
                    //};
                    //EmailDto emailDzialTechniczny2 = new EmailDto()
                    //{
                    //    To = "mariusz@aluro.pl",
                    //    Subject = "Nowe zamówienie! " + order.Id + ":" + user.Imie + " " + user.Nazwisko + " " + user.NazwaFirmy + "",
                    //    Body = emailMessage
                    //};

                    EmailDto emailDzialTechniczny3 = new EmailDto()
                    {
                        To = "szuminski.p@gmail.com",
                        Subject = "Nowe zamówienie! " + order.Id + ":" + user.Imie + " " + user.Nazwisko + " " + user.NazwaFirmy + "",
                        Body = emailMessage
                    };
                    //_emailService.SendEmailAsync(emailDzialTechniczny1);
                    //_emailService.SendEmailAsync(emailDzialTechniczny2);
                    _emailService.SendEmailAsync(emailDzialTechniczny3);


                    //var cart = _context.Carts.Where(c => c.CartaId == _cart.CartaId).FirstOrDefault();
                    //var cart = _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefault();
                    //var cart = _context.Carts
                    //.Where(x => x.UserId == user.Id)
                    //.Where(x => x.CartaId == _cart.CartaId)
                    //.FirstOrDefault();

                    //if (cart != null)
                    //{
                    _cart.Zrealizowane = true;
                    //_cart.ClearCart();

                    _context.Carts.Update(_cart);
                    _context.SaveChanges();
                    //}



                    return View("CheckoutComplete", CartOrder.Orders);
                
            }
            else
            {
                _cart.CartItems = await _cart.GetAllCartItemsAsync();
                CartOrder.Carts = _cart;

                return View(CartOrder);
            }
        }

        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> PDFOrderAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = await _orderService.ListAsync(id);
            Order order = await _orderService.GetOrder(id);

            order.OrderItems = orderItems;

            Document document = new Document(iTextSharp.text.PageSize.A4);

            byte[] pdfBytes;
            using (var ms = new MemoryStream())
            {
                using (PdfWriter write = PdfWriter.GetInstance(document, ms))
                {


                    Font bold = new(BaseFont.CreateFont(@"wwwroot\css\font\arial.ttf", BaseFont.CP1250, true), 10, Font.BOLD);
                    Font regular = new(BaseFont.CreateFont(@"wwwroot\css\font\arial.ttf", BaseFont.CP1250, true), 10);


                    //Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    write.PageEvent = new PageHeaderFooter();
                    document.Open();

                    //Image image = iTextSharp.text.Image.GetInstance("wwwroot/img/logo/AluroLogoPdf_120x60.jpg");
                    //image.SetAbsolutePosition(200, write.GetVerticalPosition(true));
                    //document.Add(image);

                    //Image image3 = iTextSharp.text.Image.GetInstance("wwwroot/img/logo/AluroLogoPdf_120x60.jpg");
                    //image3.SetAbsolutePosition(write.GetVerticalPosition(true),200);
                    //document.Add(image3);



                    Image image2 = Image.GetInstance("wwwroot/img/logo/Aluro_logo-x-300_2.png");
                    image2.ScaleAbsoluteWidth(150);
                    image2.ScaleAbsoluteHeight(75);
                    image2.SetAbsolutePosition(45, 730);
                    document.Add(image2);


                    string text1 = "Data zamówienia: ";
                    string text2 = order.OrderPlaced.ToString("dd/MM/yyyy");
                    //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                    Chunk c1 = new(text1, bold);
                    Chunk c2 = new(text2, regular);

                    Paragraph para1 = new();
                    para1.Add(c1);
                    para1.Add(c2);
                    para1.Alignment = Element.ALIGN_RIGHT;
                    document.Add(para1);


                    Paragraph para2 = new("ID #" + order.NrZamowienia, new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD))
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingAfter = 8
                    };
                    document.Add(para2);

                    Paragraph para2_1 = new(order.MetodaDostawy, regular)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingAfter = 15
                    };
                    document.Add(para2_1);

                    Paragraph para3 = new("NIP: " + order.adresRozliczeniowy.Vat, bold)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingAfter = 10
                    };
                    document.Add(para3);


                    string text2_1 = order.User.NazwaFirmy + "\n";
                    string text2_2 = order.User.Imie + " " + order.User.Nazwisko + "\n";
                    string text2_3 = order.adresRozliczeniowy.Ulica + "\n";
                    string text2_4 = order.adresRozliczeniowy.KodPocztowy + " " + order.adresRozliczeniowy.Miasto + "\n";
                    string text2_5 = order.adresRozliczeniowy.Telefon + "\n";
                    string text2_6 = order.adresRozliczeniowy.Vat + "\n";
                    string text2_7 = order.User.Email + "\n";
                    //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                    Chunk c2_1 = new(text2_1, bold);
                    Chunk c2_2 = new(text2_2, regular);
                    Chunk c2_3 = new(text2_3, regular);
                    Chunk c2_4 = new(text2_4, regular);
                    Chunk c2_5 = new(text2_5, regular);
                    Chunk c2_6 = new(text2_6, bold);
                    Chunk c2_7 = new(text2_7, regular);

                    Paragraph para4 = new()
                    {
                        c2_1,
                        c2_2,
                        c2_3,
                        c2_4,
                        c2_5
                    };
                    para4.Alignment = Element.ALIGN_LEFT;
                    //document.Add(para4);



                    string text4_1 = order.AdressDostawy.Imie + " " + order.AdressDostawy.Nazwisko + "\n";
                    string text4_1_1 = order.AdressDostawy.NazwaFirmy + "\n";
                    string text4_2 = order.User.Imie + " " + order.User.Nazwisko + "\n";
                    string text4_3 = order.AdressDostawy.Ulica + "\n";
                    string text4_4 = order.AdressDostawy.KodPocztowy + " " + order.AdressDostawy.Miasto + "\n";
                    string text4_5 = order.AdressDostawy.Telefon + "\n";

                    string text4_7 = order.User.Email + "\n";
                    //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                    //Chunk c4_1 = new(text4_1, bold);
                    Chunk c4_1_1 = new(text4_1_1, bold);
                    Chunk c4_2 = new(text4_2, regular);
                    Chunk c4_3 = new(text4_3, regular);
                    Chunk c4_4 = new(text4_4, regular);
                    Chunk c4_5 = new(text4_5, regular);
                    Chunk c4_6 = new(text2_6, bold);
                    Chunk c4_7 = new(text4_7, regular);

                    Paragraph para5 = new Paragraph
                    {
                        //c4_1,
                        c4_1_1,
                        c4_2,
                        c4_3,
                        c4_4,
                        c4_5
                    };

                    para5.Alignment = Element.ALIGN_LEFT;
                    //document.Add(para4);

                    PdfPTable table1 = new(5);

                    table1.TotalWidth = 500f;
                    table1.LockedWidth = true;
                    float[] widths = new float[] { 200f, 5f, 240f, 10f, 90f };
                    table1.SetWidths(widths);


                    PdfPCell cell1_tab1 = new(new Phrase("Faktura dla", bold));
                    cell1_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell1_tab1.BorderWidth = 0;
                    cell1_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1_tab1.VerticalAlignment = Element.ALIGN_LEFT;
                    table1.AddCell(cell1_tab1);


                    PdfPCell cell2_tab1 = new(new Phrase("", regular));
                    cell2_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell2_tab1.BorderWidth = 0;
                    cell2_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell2_tab1.VerticalAlignment = Element.ALIGN_LEFT;
                    table1.AddCell(cell2_tab1);

                    PdfPCell cell3_tab1 = new(new Phrase("Dostawa do", bold))
                    {
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_LEFT
                    };
                    table1.AddCell(cell3_tab1);

                    PdfPCell cell4_tab1 = new(new Phrase("", regular))
                    {
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_LEFT
                    };
                    table1.AddCell(cell4_tab1);

                    PdfPCell cell5_tab1 = new(new Phrase("", regular))
                    {
                        //cell5_tab1.BackgroundColor = BaseColor.LIGHT_GRAY;
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_LEFT
                    };

                    table1.AddCell(cell5_tab1);


                    for (int i = 0; i < 1; i++)
                    {
                        PdfPCell cell_1 = new(new Phrase(para4));
                        cell_1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell_1.BorderWidth = 0;

                        PdfPCell cell_2 = new(); //odstep
                        cell_2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell_2.BorderWidth = 0;

                        PdfPCell cell_3 = new(new Phrase(para5));

                        PdfPCell cell_4 = new();//odstep
                        cell_4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell_4.BorderWidth = 0;

                        PdfPCell cell_5 = new(new Phrase("WYMIARY"));

                        cell_1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_3.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_4.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_5.HorizontalAlignment = Element.ALIGN_LEFT;

                        table1.AddCell(cell_1);
                        table1.AddCell(cell_2);
                        table1.AddCell(cell_3);
                        table1.AddCell(cell_4);
                        table1.AddCell(cell_5);
                    }
                    document.Add(table1);



                    string text6_1 = "NIP: " + order.adresRozliczeniowy.Vat + "\n";
                    string text6_3 = "Email: " + order.User.Email + "\n";
                    //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                    Chunk c6_1 = new(text6_1, bold);
                    Chunk c6_3 = new(text6_3, regular);

                    Paragraph para6 = new();
                    para6.Add(c6_1);
                    para6.Add(c6_3);
                    para6.Alignment = Element.ALIGN_LEFT;
                    para6.SpacingAfter = 15;
                    document.Add(para6);

                    //Paragraph para90 = new Paragraph("To jest paragraf3", regular);
                    //para90.Alignment = Element.ALIGN_CENTER;
                    //para90.SpacingAfter = 10;
                    //document.Add(para90);

                    PdfPTable table2 = new(8);

                    table2.TotalWidth = 500f;
                    table2.LockedWidth = true;
                    float[] widths2 = new float[] { 10f, 55f, 200f, 50f, 40f, 40f, 40f, 40f };
                    table2.SetWidths(widths2);

                    PdfPCell cell1_tab2 = new(new Phrase("#", regular));
                    cell1_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell1_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell1_tab2.BorderWidth = 1;
                    cell1_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                    table2.AddCell(cell1_tab2);

                    PdfPCell cell2_tab2 = new(new Phrase("Obrazek", regular));
                    cell2_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell2_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell2_tab2.BorderWidth = 1;
                    cell2_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell2_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                    table2.AddCell(cell2_tab2);

                    PdfPCell cell3_tab2 = new(new Phrase("Nazwa / Kod kreskowy", regular));
                    cell3_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell3_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell3_tab2.BorderWidth = 1;
                    cell3_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell3_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                    table2.AddCell(cell3_tab2);

                    PdfPCell cell4_tab2 = new(new Phrase("Symbol", regular))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_CENTER
                    };
                    table2.AddCell(cell4_tab2);

                    PdfPCell cell5_tab2 = new(new Phrase("Ilość", regular))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_CENTER
                    };
                    table2.AddCell(cell5_tab2);

                    PdfPCell cell6_tab2 = new(new Phrase("Rabat", regular))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_CENTER
                    };
                    table2.AddCell(cell6_tab2);

                    PdfPCell cell7_tab2 = new(new Phrase("CenaTotal (brutto)", regular))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_CENTER
                    };
                    table2.AddCell(cell7_tab2);

                    PdfPCell cell8_tab2 = new(new Phrase("Wartość (brutto)", regular))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                        BorderWidth = 1,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_CENTER
                    };
                    table2.AddCell(cell8_tab2);


                    foreach (var item in order.OrderItems)
                    {
                        decimal cenaJednostkowa = item.Product.CenaProduktuBrutto * (1 - ((decimal)order.RabatZamowienia / 100));
                        decimal cenaJednostkowaIlosc = cenaJednostkowa * @item.Quantity;

                        PdfPCell cell_1 = new(new Phrase(item.Id));
                        PdfPCell cell_2 = new();
                        //partneralluro.hostingasp.pl / wwwroot / wwwroot / img / p / 4
                        string path = "";


                        var imageProduct = _context.Images.Where(x => x.ImageId == item.Product.ProductImagesId).FirstOrDefault();
                        if (imageProduct != null)
                        {

                            path = "wwwroot/" + imageProduct.path + imageProduct.ImageName;
                            if (System.IO.File.Exists(path))
                            {

                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(path);
                                img.ScaleAbsoluteWidth(50);
                                img.ScaleAbsoluteHeight(45);
                                cell_2 = new(img);
                            }
                        }
                        //document.Add(pic);


                        //// Creating an ImageData object 
                        //String imageFile = "C:/itextExamples/javafxLogo.jpg";
                        //ImageData data = ImageDataFactory.Create(imageFile);

                        //Image images = new Image(data);

                        //image.setWidth(pdfDocument.getDefaultPageSize().getWidth() - 50);
                        //image.setAutoScaleHeight(true);


                        //Image image3 = Image.GetInstance("wwwroot/img/logo/Aluro_logo-x-300_2.png");

                        //string paths =  item.Product.pathImageUrl250x250;
                        //Image image = Image.GetInstance("https://partneralluro.hostingasp.pl/" + paths+"");
                        //img.ScaleAbsoluteWidth(50);
                        //img.ScaleAbsoluteHeight(45);
                        //PdfPCell cell_2 = new(img);
                        PdfPCell cell_3 = new(new Phrase(item.Product.Name, regular));
                        PdfPCell cell_4 = new(new Phrase(item.Product.Symbol, regular));
                        PdfPCell cell_5 = new(new Phrase(item.Quantity.ToString(), regular));
                        PdfPCell cell_6 = new(new Phrase(order.RabatZamowienia.ToString(), regular));
                        PdfPCell cell_7 = new(new Phrase(cenaJednostkowa.ToString("C"), regular));
                        PdfPCell cell_8 = new(new Phrase(cenaJednostkowaIlosc.ToString("C"), regular));

                        cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_1.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_2.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_3.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_4.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_5.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_5.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_6.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_6.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_7.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_7.VerticalAlignment = Element.ALIGN_CENTER;
                        cell_8.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_8.VerticalAlignment = Element.ALIGN_CENTER;

                        table2.AddCell(cell_1);
                        table2.AddCell(cell_2);
                        table2.AddCell(cell_3);
                        table2.AddCell(cell_4);
                        table2.AddCell(cell_5);
                        table2.AddCell(cell_6);
                        table2.AddCell(cell_7);
                        table2.AddCell(cell_8);
                    }
                    document.Add(table2);




                    PdfPTable table3 = new(2);

                    table3.SpacingBefore = 30;

                    table3.TotalWidth = 500f;
                    table3.LockedWidth = true;
                    float[] widths3 = new float[] { 200f, 100f };
                    table3.SetWidths(widths3);


                    PdfPCell cell1_tab3 = new(new Phrase(""));
                    cell1_tab3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell1_tab3.BorderWidth = 0;
                    cell1_tab3.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1_tab3.VerticalAlignment = Element.ALIGN_LEFT;
                    table3.AddCell(cell1_tab3);

                    PdfPCell cell2_tab3 = new(new Phrase(""));
                    cell2_tab3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell2_tab3.BorderWidth = 0;
                    cell2_tab3.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell2_tab3.VerticalAlignment = Element.ALIGN_LEFT;
                    table3.AddCell(cell2_tab3);


                    for (int i = 0; i < 1; i++)
                    {
                        PdfPCell cell_1 = new(new Phrase());
                        cell_1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell_1.BorderWidth = 0;

                        PdfPCell cell_2 = new(new Phrase("Do zapłaty: " + order.OrderTotal.ToString("C"), bold)); //odstep
                        cell_2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        cell_2.BorderWidth = 1;

                        cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell_2.HorizontalAlignment = Element.ALIGN_CENTER;

                        table3.AddCell(cell_1);
                        table3.AddCell(cell_2);
                    }
                    document.Add(table3);

                    PdfPTable table4 = new(1);

                    table4.SpacingBefore = 30;

                    table4.TotalWidth = 500f;
                    table4.LockedWidth = true;

                    PdfPCell cell1_tab4 = new(new Phrase("Wiadomość: " + order.Komentarz, bold));
                    cell1_tab4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                    cell1_tab4.BorderWidth = 1;
                    cell1_tab4.PaddingTop = 10;
                    cell1_tab4.PaddingBottom = 10;
                    cell1_tab4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1_tab4.VerticalAlignment = Element.ALIGN_LEFT;
                    table4.AddCell(cell1_tab4);

                    document.Add(table4);

                    document.Close();
                    write.Close();
                    var constant = ms.ToArray();

                    return File(constant, "application/vnd", order.Id +"_"+ order.User.NazwaFirmy + ".pdf");
                }
                pdfBytes = ms.ToArray();
            }
            
        }

        public bool CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            order.ByloAnulowane = false;

            var cartItems = _cart.CartItems;

            foreach (var item in cartItems)
            {
                if(item.Quantity < item.Product.Ilosc)
                {

                _productService.ZmiejszIloscProductIdAsync(item.Product.ProductId, item.Quantity);

                item.Product.CenaProduktuDlaUzytkownika = item.Product.CenaProduktuBrutto * (1 - (Core.Constants.Rabat / 100));

                var Cena_brutto = (decimal)item.Product.CenaProduktuBrutto;
                if (item.Product.Promocja == true && item.Product.CenaPromocyja != 0)
                {
                    Cena_brutto = (decimal)item.Product.CenaPromocyja;
                }

                var CenaJednostkowa = Cena_brutto * (1 - (partner_aluro.Core.Constants.Rabat / 100));

                var ProductTotal = CenaJednostkowa * item.Quantity;

                var orderItem = new OrderItem()
                {
                    Quantity = item.Quantity,
                    ProductId = item.Product.ProductId,
                    OrderId = order.Id,
                    CenaJednProductuBrutto = item.Product.CenaProduktuBrutto,
                    CenaJednProductuNetto = item.Product.CenaProduktuNetto,
                    //CenaTotal = (int)(item.Product.CenaProduktuDlaUzytkownika * item.Quantity)
                    CenaTotal = ProductTotal
                    //CenaTotal = (int)(item.Product.CenaProduktuBrutto * item.Quantity)
                };

                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.CenaTotal;
                }
                else
                {
                    return false;
                }
            }
            order.UserID = _userManager.GetUserId(HttpContext.User);

            order.RabatZamowienia = Core.Constants.Rabat;
            order.StanZamowienia = StanZamowienia.Nowe;

            order.ByloAnulowane = false;
            _orderService.Add(order);
            return true;
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        public async Task<IActionResult> ListaZamowien() // To jest widok listy zamowien w panelu dashoboards
        {
            @ViewData["StanZamowienia"] = GetStanyZamowienia();
            //List<Order> orders = await _orderService.ListOrdersAll();
            ICollection<Order> orders = _context.Orders
                .Include(x=>x.User)
                .ThenInclude(x=>x.Adress1rozliczeniowy)
                .Include(x=>x.OrderItems)
                .ToList();
            return View(orders);
        }
        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.Klient}")]
        public IActionResult ListaZamowienZalogowanegoUzytkownika() // To jest widok listy zamowien w panelu dashoboards
        {
            @ViewData["StanZamowienia"] = GetStanyZamowienia();
            var UserID = _userManager.GetUserId(HttpContext.User);

            List<Order> orders = _orderService.ListOrdersUser(UserID);

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> ZmienStatusAsync(Order order)
        {

            //Wyslij e mail do klienta
            int id = order.Id;

            Order orders = await _unitOfWorkOrder.OrderService.GetOrder(id);
            orders.StanZamowienia = order.StanZamowienia;

            if (orders.StanZamowienia == StanZamowienia.Anulowane && orders.ByloAnulowane == false)
            {
                AnulowanieZamowieniaZmienStanyProduktow(orders.Id);
                orders.ByloAnulowane = true;
            }
            else if(orders.StanZamowienia != StanZamowienia.Anulowane && orders.ByloAnulowane == true)
            {
                PrzywrocenieZamowieniaZmienStanyProduktow(orders.Id);
                orders.ByloAnulowane = false;
            }

            _unitOfWorkOrder.OrderService.Update(orders);

            return RedirectToAction("Detail", new { id = id });
        }

        //Dodanie ilosc do stanu z anulowanego zamowienia
        public void AnulowanieZamowieniaZmienStanyProduktow(int OrderId)
        {
            Order orders = _context.Orders.Where(x=>x.Id == OrderId).Include(i=>i.OrderItems).FirstOrDefault();
            for(int i = 0; i < orders.OrderItems.Count(); i++) 
            { 
                Product product = _context.Products.Where(x=>x.ProductId == orders.OrderItems[i].ProductId).FirstOrDefault();
                product.Ilosc += orders.OrderItems[i].Quantity;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }

        //Dodanie ilosc do stanu z anulowanego zamowienia
        public void PrzywrocenieZamowieniaZmienStanyProduktow(int OrderId)
        {
            Order orders = _context.Orders.Where(x => x.Id == OrderId).Include(i => i.OrderItems).FirstOrDefault();
            for (int i = 0; i < orders.OrderItems.Count(); i++)
            {
                Product product = _context.Products.Where(x => x.ProductId == orders.OrderItems[i].ProductId).FirstOrDefault();
                product.Ilosc -= orders.OrderItems[i].Quantity;
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }

        [HttpPost]
        public void ChangeStanZamowienia(int Id, string stanZamowienia)
        {
            Order orders = _context.Orders.Where(x=>x.Id == Id).FirstOrDefault();
            //orders.StanZamowienia = order.StanZamowienia;

            foreach (StanZamowienia suit in (StanZamowienia[])Enum.GetValues(typeof(StanZamowienia)))
            {
                if (suit.ToString() == stanZamowienia)
                {
                    orders.StanZamowienia = suit;
                }
            }

            if (orders.StanZamowienia == StanZamowienia.Anulowane && orders.ByloAnulowane == false)
            {
                AnulowanieZamowieniaZmienStanyProduktow(orders.Id);
                orders.ByloAnulowane = true;
            }
            else if (orders.StanZamowienia != StanZamowienia.Anulowane && orders.ByloAnulowane == true)
            {
                PrzywrocenieZamowieniaZmienStanyProduktow(orders.Id);
                orders.ByloAnulowane = false;
            }

            _context.Orders.Update(orders);
            _context.SaveChanges();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            //var orderItems = await _orderService.ListAsync(id);
            //Order order = await _orderService.GetOrder(id);

            //order.OrderItems = orderItems;


            Order order = _context.Orders.Where(x => x.Id == id)
                .Include(x => x.OrderItems)
                .ThenInclude(x=>x.Product)
                .Include(x=>x.User)
                .ThenInclude(x=>x.Adress1rozliczeniowy)
                .Include(x=>x.AdressDostawy)
                .Include(x=>x.adresRozliczeniowy)
                .First();

            ViewData["StanyZamowienia"] = GetStanyZamowienia();

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> DetailOrder(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = await _orderService.ListAsync(id);
            Order order = await _orderService.GetOrder(id);

            order.OrderItems = orderItems;

            ViewData["StanyZamowienia"] = GetStanyZamowienia();

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
            return RedirectToAction("Detail", new { id = id });
        }

        private static List<SelectListItem> GetStanyZamowienia()
        {
            var lstStanZamowien = new List<SelectListItem>();

            foreach (StanZamowienia suit in (StanZamowienia[])Enum.GetValues(typeof(StanZamowienia)))
            {
                if (suit == StanZamowienia.Wrealizacji)
                {
                    var dmyItemA = new SelectListItem()
                    {
                        Value = suit.ToString(),
                        Text = "W realizacji"
                    };
                    lstStanZamowien.Insert(0, dmyItemA);
                }
                else
                {
                    var dmyItemA = new SelectListItem()
                    {
                        Value = suit.ToString(),
                        Text = suit.ToString()
                    };
                    lstStanZamowien.Insert(0, dmyItemA);
                }
            }

            return lstStanZamowien;
        }

        [HttpPost]
        public void ChangeIlosc(int ProduktId, int Ilosc, int OrderID)
        {
            Order order = _context.Orders.Where(x => x.Id == OrderID).Include(oi => oi.OrderItems).ThenInclude(p=>p.Product).FirstOrDefault();

            OrderItem oi = order.OrderItems.Where(p => p.ProductId == ProduktId).First();

            Product produkt = _context.Products.Where(x=>x.ProductId == ProduktId).First();

            if(Ilosc >= oi.Quantity && order.StanZamowienia != StanZamowienia.Anulowane)
            {
                var roznica = Ilosc - oi.Quantity;
                produkt.Ilosc -= roznica;
            }else if(Ilosc < oi.Quantity && order.StanZamowienia != StanZamowienia.Anulowane)
            {
                var roznica = oi.Quantity - Ilosc;
                produkt.Ilosc += roznica;
            }

            order.OrderTotal -= oi.CenaTotal;

            oi.Quantity = Ilosc;
            oi.CenaTotal = Ilosc * oi.CenaJednProductuBrutto;
            order.OrderTotal += oi.CenaTotal;

            _context.Orders.Update(order);
            _context.SaveChanges();

        }

    }


}
