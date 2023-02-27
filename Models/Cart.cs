using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewComponents;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Threading.Tasks.Sources;
using static iTextSharp.text.pdf.AcroFields;

namespace partner_aluro.Models
{
    public class Cart
    {
        private readonly ApplicationDbContext _context;

        public Cart()
        {

        }

        public Cart(ApplicationDbContext context)
        {
            _context = context;
        }



        [Key]
        public int CartID { get; set; }

        public int Id { get; set; }

        public string? CartaId { get; set; }
        public string? UserId { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? user { get; set; }

        public DateTime dataPowstania { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RazemNetto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RazemBrutto { get; set; }

        public bool? Zrealizowane { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartsId = session.GetString("CartsId") ?? Guid.NewGuid().ToString();

            session.SetString("CartsId", cartsId);


            var user = services.GetRequiredService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            session.SetString("User", user.ToString());

            Cart cart = new Cart();
            cart.dataPowstania = DateTime.Now;

            var existCart2 = context.Carts
                .Where(x => x.UserId == user)
                .OrderBy(x=>x.CartID)
                .LastOrDefault(); // istneije juz taka sesja wiec nie dodwaj jej do bazy tylko zrob update

            var existCart = context.Carts.Where(x => x.CartaId == cartsId).FirstOrDefault(); // istneije juz taka sesja wiec nie dodwaj jej do bazy tylko zrob update
            if(existCart2 != null)
            {
                //jesli jest zrealizowana to utworz nowa Carte
                if(existCart2.Zrealizowane == true)
                {
                    cartsId = Guid.NewGuid().ToString();
                    session.SetString("CartsId", cartsId);

                    cart = new Cart(context) { CartaId = cartsId, UserId = user };
                    cart.RazemBrutto = 0;
                    cart.RazemNetto = 0;
                    cart.dataPowstania = DateTime.Now;
                    cart.Zrealizowane = false;
                    context.Carts.Add(cart);
                    context.SaveChanges();
                }


                //jesli nie jest zrealizowana
                if (existCart2.Zrealizowane == false)
                {
                    //cart = context.Carts.Where(x => x.UserId == user).FirstOrDefault();
                    cart = existCart2;

                    //cart = context.Carts
                    //   .Include(x => x.user)
                    //   .Include(u => u.CartItems)
                    //   .ThenInclude(p => p.Product)
                    //   .Where(x => x.UserId == user)
                    //   .FirstOrDefault();

                    cart.dataPowstania = DateTime.Now;
                    cart.RazemBrutto = cart.GetCartTotalBrutto();
                    context.Carts.Update(cart);
                    context.SaveChanges();
                }

            }
            else //nie istnieje taki koszy utworz wpis do bazy z nowym koszykiem
            {
                cart = new Cart(context) { CartaId = cartsId, UserId = user };

                cart.RazemBrutto = 0;
                cart.RazemNetto = 0;
                cart.dataPowstania = DateTime.Now;
                cart.Zrealizowane = false;

                    context.Carts.Add(cart);
            }

            context.SaveChanges();

            return cart;
        }

        public CartItem GetCartItem(Product product)
        {
            return _context.CartItems.SingleOrDefault(ci =>
                ci.Product.ProductId == product.ProductId && ci.CartIds == CartaId);
        }

        public void AddToCart(Product product, int quantity)
        {

            var Rabat = Core.Constants.Rabat;

            product.CenaProduktuDlaUzytkownika = product.CenaProduktuBrutto * (1 - (Rabat / 100));

            ApplicationUser user = _context.Users.Where(x => x.Id == UserId).FirstOrDefault();

            var cartItem = GetCartItem(product);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    CartIds = CartaId,
                    Carts = _context.Carts.Where(x => x.CartaId == CartaId).FirstOrDefault()
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                var iloscProduktuNaStanie = _context.Products.Where(x => x.ProductId == product.ProductId).Select(x=>x.Ilosc).FirstOrDefault();
                if (cartItem.Quantity < iloscProduktuNaStanie)
                {
                    cartItem.Quantity += quantity;
                }
            }
            _context.SaveChanges();
        }
        public decimal ReduceQuantity(Product product)
        {
            var cartItem = GetCartItem(product);
            decimal remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    remainingQuantity = --cartItem.Quantity;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public decimal IncreaseQuantity(Product product)
        {
            var cartItem = GetCartItem(product);
            decimal remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 0 && cartItem.Quantity < product.Ilosc)
                {
                    remainingQuantity = ++cartItem.Quantity;
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public void RemoveFromCart(Product product)
        {
            var cartItem = GetCartItem(product);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }
        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartIds == CartaId);

            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync()
        {
            return (List<CartItem>)(CartItems ??= await _context.CartItems.Where(ci => ci.CartIds == CartaId)
                    .Include(ci => ci.Product)
                    .ToListAsync());
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync(string CartId)
        {
            List<CartItem> cartItem = new List<CartItem>();

            return cartItem ??= await _context.CartItems.Where(ci => ci.CartIds == CartId)
                    .AsNoTracking()
                    .Include(ci => ci.Product)
                    .ToListAsync();
        }

        public decimal GetCartTotalBrutto()
        {
            decimal CartTotal1 = _context.CartItems
                .Where(ci => ci.CartIds == CartaId && ci.Product.Promocja == false)
                .Select(ci => ci.Product.CenaProduktuBrutto * ci.Quantity)
                .Sum();

            decimal CartTotal2 = _context.CartItems
                .Where(ci => ci.CartIds == CartaId && ci.Product.Promocja == true)
                .Select(ci => ci.Product.CenaPromocyja * ci.Quantity)
                .Sum();

            decimal CartTotal = CartTotal1 + CartTotal2;

            CartTotal = CartTotal * (1 - (Core.Constants.Rabat / 100));

            //CartTotal = CartTotal * Core.Constants.Vat;

            return CartTotal;
        }
        public decimal GetCartTotalNetto()
        {
            decimal CartTotal1 = _context.CartItems
                .Where(ci => ci.CartIds == CartaId && ci.Product.Promocja == false)
                .Select(ci => ci.Product.CenaProduktuBrutto * ci.Quantity)
                .Sum();

            decimal CartTotal2 = _context.CartItems
                .Where(ci => ci.CartIds == CartaId && ci.Product.Promocja == true)
                .Select(ci => ci.Product.CenaPromocyja * ci.Quantity)
                .Sum();

            decimal CartTotal = CartTotal1 + CartTotal2;

            CartTotal = CartTotal * (1 - (Core.Constants.Rabat / 100));

            CartTotal = CartTotal / Core.Constants.Vat;

            return CartTotal;
        }

    }
}
