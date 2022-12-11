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
        public string CartId { get; set; }
        public string UserId { get; set; }

        public string? CartsId { get; set; }
        [ForeignKey(nameof(CartsId))]
        public virtual List<CartItem> CartItems { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser user { get; set; }

        public decimal RazemNetto { get; set; }

        public decimal RazemBrutto { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartsId") ?? Guid.NewGuid().ToString();

            session.SetString("CartsId", cartId);


            var user = services.GetRequiredService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            session.SetString("User", user.ToString());


            return new Cart(context) { CartsId = cartId, UserId = user };
        }

        public CartItem GetCartItem(Product product)
        {
            return _context.CartItems.SingleOrDefault(ci =>
                ci.Product.ProductId == product.ProductId && ci.CartId == CartsId);
        }

        public void AddToCart(Product product, int quantity)
        {

            var Rabat = Core.Constants.Rabat;

            product.CenaProduktuDlaUzytkownika = product.CenaProduktu * (1 - (Rabat / 100));

            ApplicationUser user = _context.Users.Where(x => x.Id == UserId).FirstOrDefault();

            var cartItem = GetCartItem(product);


            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    CartId = CartsId
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }
        public int ReduceQuantity(Product product)
        {
            var cartItem = GetCartItem(product);
            var remainingQuantity = 0;

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

        public int IncreaseQuantity(Product product)
        {
            var cartItem = GetCartItem(product);
            var remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 0)
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
            var cartItems = _context.CartItems.Where(ci => ci.CartId == CartsId);

            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync()
        {
            return CartItems ??= await _context.CartItems.Where(ci => ci.CartId == CartsId)
                    .Include(ci => ci.Product)
                    .ToListAsync();
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync(string CartId)
        {
            List<CartItem> cartItem = new List<CartItem>();

            return cartItem ??= await _context.CartItems.Where(ci => ci.CartId == CartId)
                    .Include(ci => ci.Product)
                    .ToListAsync();
        }

        public decimal GetCartTotalBrutto()
        {
            decimal CartTotal1 = _context.CartItems
                .Where(ci => ci.CartId == CartsId && ci.Product.Promocja == false)
                //.Where(ci=> ci.Product.Promocja == false)
                .Select(ci => ci.Product.CenaProduktu * ci.Quantity)
                .Sum();

            decimal CartTotal2 = _context.CartItems
                .Where(ci => ci.CartId == CartsId && ci.Product.Promocja == true)
                //.Where(ci => ci.Product.Promocja == true)
                .Select(ci => ci.Product.CenaPromocyja * ci.Quantity)
                .Sum();

            decimal CartTotal = CartTotal1 + CartTotal2;

            CartTotal = CartTotal * (1 - (Core.Constants.Rabat / 100));

            CartTotal = CartTotal * Core.Constants.Vat;

            return CartTotal;
        }
        public decimal GetCartTotalNetto()
        {
            decimal CartTotal1 = _context.CartItems
                .Where(ci => ci.CartId == CartsId && ci.Product.Promocja == false)
                .Select(ci => ci.Product.CenaProduktu * ci.Quantity)
                .Sum();

            decimal CartTotal2 = _context.CartItems
                .Where(ci => ci.CartId == CartsId && ci.Product.Promocja == true)
                .Select(ci => ci.Product.CenaPromocyja * ci.Quantity)
                .Sum();

            decimal CartTotal = CartTotal1 + CartTotal2;

            CartTotal = CartTotal * (1 - (Core.Constants.Rabat / 100));

            return CartTotal;
        }

    }
}
