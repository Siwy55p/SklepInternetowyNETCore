using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System;
using static iTextSharp.text.pdf.AcroFields;
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
        public async Task<IActionResult> PDFOrderAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = _orderService.List(id);
            Order order = await _orderService.GetOrder(id);

            order.OrderItems = orderItems;


            using (MemoryStream ms = new MemoryStream())
            {

                Font bold = new Font(BaseFont.CreateFont(@"wwwroot\css\font\arial.ttf", BaseFont.CP1250, true), 10, Font.BOLD);
                Font regular = new Font(BaseFont.CreateFont(@"wwwroot\css\font\arial.ttf", BaseFont.CP1250, true), 10);




                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter write = PdfWriter.GetInstance(document, ms);
                write.PageEvent = new PageHeaderFooter();
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
                //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

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

                Paragraph para3 = new Paragraph("NIP: " + order.adresRozliczeniowy.Vat, bold);
                para3.Alignment = Element.ALIGN_RIGHT;
                para3.SpacingAfter = 10;
                document.Add(para3);


                string text2_1 = order.User.NazwaFirmy + "\n";
                string text2_2 = order.User.Imie + " " + order.User.Nazwisko +"\n";
                string text2_3 = order.adresRozliczeniowy.Ulica + "\n";
                string text2_4 = order.adresRozliczeniowy.KodPocztowy + " " + order.adresRozliczeniowy.Miasto + "\n";
                string text2_5 = order.adresRozliczeniowy.Telefon + "\n";
                string text2_6 = order.adresRozliczeniowy.Vat + "\n";
                string text2_7 = order.User.Email + "\n";
                //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                Chunk c2_1 = new Chunk(text2_1, bold);
                Chunk c2_2 = new Chunk(text2_2, regular);
                Chunk c2_3 = new Chunk(text2_3, regular);
                Chunk c2_4 = new Chunk(text2_4, regular);
                Chunk c2_5 = new Chunk(text2_5, regular);
                Chunk c2_6 = new Chunk(text2_6, bold);
                Chunk c2_7 = new Chunk(text2_7, regular);

                Paragraph para4 = new Paragraph();
                para4.Add(c2_1);
                para4.Add(c2_2);
                para4.Add(c2_3);
                para4.Add(c2_4);
                para4.Add(c2_5);
                para4.Alignment = Element.ALIGN_LEFT;
                //document.Add(para4);
                


                string text3_1 = order.AdressDostawy.Imie + " " + order.AdressDostawy.Nazwisko + "\n";
                string text4_2 = order.User.Imie + " " + order.User.Nazwisko + "\n";
                string text4_3 = order.AdressDostawy.Ulica + "\n";
                string text4_4 = order.AdressDostawy.KodPocztowy + " " + order.adresRozliczeniowy.Miasto + "\n";
                string text4_5 = order.AdressDostawy.Telefon + "\n";

                string text4_7 = order.User.Email + "\n";
                //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                Chunk c4_1 = new Chunk(text2_1, bold);
                Chunk c4_2 = new Chunk(text2_2, regular);
                Chunk c4_3 = new Chunk(text2_3, regular);
                Chunk c4_4 = new Chunk(text2_4, regular);
                Chunk c4_5 = new Chunk(text2_5, regular);
                Chunk c4_6 = new Chunk(text2_6, bold);
                Chunk c4_7 = new Chunk(text2_7, regular);

                Paragraph para5 = new Paragraph();
                para5.Add(c4_1);
                para5.Add(c4_2);
                para5.Add(c4_3);
                para5.Add(c4_4);
                para5.Add(c4_5);
                para5.Alignment = Element.ALIGN_LEFT;
                //document.Add(para4);




                PdfPTable table1 = new PdfPTable(5);

                table1.TotalWidth = 500f;
                table1.LockedWidth = true;
                float[] widths = new float[] { 200f, 5f, 240f, 10f, 90f };
                table1.SetWidths(widths);


                PdfPCell cell1_tab1 = new PdfPCell(new Phrase("Faktura dla", bold));
                cell1_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell1_tab1.BorderWidth = 0;
                cell1_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1_tab1.VerticalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(cell1_tab1);


                PdfPCell cell2_tab1 = new PdfPCell(new Phrase("", regular));
                cell2_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell2_tab1.BorderWidth = 0;
                cell2_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_tab1.VerticalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(cell2_tab1);

                PdfPCell cell3_tab1 = new PdfPCell(new Phrase("Dostawa do", bold));
                cell3_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell3_tab1.BorderWidth = 0;
                cell3_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell3_tab1.VerticalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(cell3_tab1);

                PdfPCell cell4_tab1 = new PdfPCell(new Phrase("", regular));
                cell4_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell4_tab1.BorderWidth = 0;
                cell4_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell4_tab1.VerticalAlignment = Element.ALIGN_LEFT;
                table1.AddCell(cell4_tab1);

                PdfPCell cell5_tab1 = new PdfPCell(new Phrase("", regular));
                //cell5_tab1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell5_tab1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell5_tab1.BorderWidth = 0;
                cell5_tab1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell5_tab1.VerticalAlignment = Element.ALIGN_LEFT;

                table1.AddCell(cell5_tab1);


                for (int i = 0; i < 1; i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(para4));
                    cell_1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell_1.BorderWidth = 0;

                    PdfPCell cell_2 = new PdfPCell(); //odstep
                    cell_2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell_2.BorderWidth = 0;

                    PdfPCell cell_3 = new PdfPCell(new Phrase(para5));

                    PdfPCell cell_4 = new PdfPCell();//odstep
                    cell_4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell_4.BorderWidth = 0;

                    PdfPCell cell_5 = new PdfPCell(new Phrase("Pakowal"));

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



                string text6_1 = order.AdressDostawy.Imie + " " + order.AdressDostawy.Nazwisko + "\n";
                string text6_3 = order.AdressDostawy.Ulica + "\n";
                //Paragraph para1 = new Paragraph("Data zamówienia: " + order.OrderPlaced, bold);

                Chunk c6_1 = new Chunk(text6_1, bold);
                Chunk c6_3 = new Chunk(text6_3, regular);

                Paragraph para6 = new Paragraph();
                para6.Add(c6_1);
                para6.Add(c6_3);
                para6.Alignment = Element.ALIGN_LEFT;
                para6.SpacingAfter = 15;
                document.Add(para6);

                //Paragraph para90 = new Paragraph("To jest paragraf3", regular);
                //para90.Alignment = Element.ALIGN_CENTER;
                //para90.SpacingAfter = 10;
                //document.Add(para90);

                PdfPTable table2 = new PdfPTable(8);

                table2.TotalWidth = 500f;
                table2.LockedWidth = true;
                float[] widths2 = new float[] { 10f, 55f, 200f, 50f, 40f, 40f,40f, 40f };
                table2.SetWidths(widths2);

                PdfPCell cell1_tab2 = new PdfPCell(new Phrase("#", regular));
                cell1_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell1_tab2.BorderWidth = 1;
                cell1_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell1_tab2);

                PdfPCell cell2_tab2 = new PdfPCell(new Phrase("Obrazek", regular));
                cell2_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell2_tab2.BorderWidth = 1;
                cell2_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell2_tab2);

                PdfPCell cell3_tab2 = new PdfPCell(new Phrase("Nazwa / Kod kreskowy", regular));
                cell3_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell3_tab2.BorderWidth = 1;
                cell3_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell3_tab2);

                PdfPCell cell4_tab2 = new PdfPCell(new Phrase("Symbol", regular));
                cell4_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell4_tab2.BorderWidth = 1;
                cell4_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell4_tab2);

                PdfPCell cell5_tab2 = new PdfPCell(new Phrase("Ilość", regular));
                cell5_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell5_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell5_tab2.BorderWidth = 1;
                cell5_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell5_tab2);

                PdfPCell cell6_tab2 = new PdfPCell(new Phrase("Rabat", regular));
                cell6_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell6_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell6_tab2.BorderWidth = 1;
                cell6_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell6_tab2);

                PdfPCell cell7_tab2 = new PdfPCell(new Phrase("Cena (brutto)", regular));
                cell7_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell7_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell7_tab2.BorderWidth = 1;
                cell7_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell7_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell7_tab2);

                PdfPCell cell8_tab2 = new PdfPCell(new Phrase("Wartość (brutto)", regular));
                cell8_tab2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell8_tab2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell8_tab2.BorderWidth = 1;
                cell8_tab2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8_tab2.VerticalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(cell8_tab2);


                foreach(var item in order.OrderItems)
                {
                    decimal cenaJednostkowa = item.Product.CenaProduktu * (1 - ((decimal)order.RabatZamowienia / 100));
                    decimal cenaJednostkowaIlosc = cenaJednostkowa * @item.Quantity;

                    PdfPCell cell_1 = new PdfPCell(new Phrase(item.Id));

                    Image image = Image.GetInstance("wwwroot/images/produkty/"+@item.Product.Symbol+"/"+@item.Product.ImageUrl);
                    image.ScaleAbsoluteWidth(50);
                    image.ScaleAbsoluteHeight(45);
                    PdfPCell cell_2 = new PdfPCell(image);
                    PdfPCell cell_3 = new PdfPCell(new Phrase(item.Product.Name,regular));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(item.Product.Symbol, regular));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(item.Quantity.ToString(), regular));
                    PdfPCell cell_6 = new PdfPCell(new Phrase(order.RabatZamowienia.ToString(), regular));
                    PdfPCell cell_7 = new PdfPCell(new Phrase(cenaJednostkowa.ToString("C"), regular));
                    PdfPCell cell_8 = new PdfPCell(new Phrase(cenaJednostkowaIlosc.ToString("C"), regular));

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




                PdfPTable table3 = new PdfPTable(2);

                table3.SpacingBefore = 30;

                table3.TotalWidth = 500f;
                table3.LockedWidth = true;
                float[] widths3 = new float[] { 200f, 100f };
                table3.SetWidths(widths3);


                PdfPCell cell1_tab3 = new PdfPCell(new Phrase(""));
                cell1_tab3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell1_tab3.BorderWidth = 0;
                cell1_tab3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1_tab3.VerticalAlignment = Element.ALIGN_LEFT;
                table3.AddCell(cell1_tab3);

                PdfPCell cell2_tab3 = new PdfPCell(new Phrase(""));
                cell2_tab3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell2_tab3.BorderWidth = 0;
                cell2_tab3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2_tab3.VerticalAlignment = Element.ALIGN_LEFT;
                table3.AddCell(cell2_tab3);


                for (int i = 0; i < 1; i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase());
                    cell_1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell_1.BorderWidth = 0;

                    PdfPCell cell_2 = new PdfPCell(new Phrase("Do zapłaty: " + order.OrderTotal.ToString("C"),bold)); //odstep
                    cell_2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell_2.BorderWidth = 1;

                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;

                    table3.AddCell(cell_1);
                    table3.AddCell(cell_2);
                }
                document.Add(table3);



                PdfPTable table4 = new PdfPTable(1);

                table4.SpacingBefore = 30;

                table4.TotalWidth = 500f;
                table4.LockedWidth = true;



                PdfPCell cell1_tab4 = new PdfPCell(new Phrase("Wiadomość: " + order.Komentarz, bold));
                cell1_tab4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER ;
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
        public async Task<IActionResult> ListaZamowienAsync() // To jest widok listy zamowien w panelu dashoboards
        {
            List<Order> orders = await _orderService.ListOrdersAll();

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
        public async Task<IActionResult> ZmienStatusAsync(Order order)
        {
            //Wyslij e mail do klienta
            int id = order.Id;

            Order orders = await _unitOfWorkOrder.OrderService.GetOrder(id);
            orders.StanZamowienia = order.StanZamowienia;
            _unitOfWorkOrder.OrderService.Update(orders);

            return RedirectToAction("Detail", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = _orderService.List(id);
            Order order = await _orderService.GetOrder(id);
            
            order.OrderItems = orderItems;

            ViewBag.StanyZamowienia = GetStanyZamowienia();

            //var adres1 = _orderService.GetUserAdress1(order.UserID);
            //var adres2 = _orderService.GetUserAdress2(order.UserID);
            //order.User.Adress1rozliczeniowy = adres1;
            //order.User.Adress2dostawy = adres2;

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
