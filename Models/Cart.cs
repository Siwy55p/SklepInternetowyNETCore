using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewComponents;
using System.Security.Claims;
using System.Threading.Tasks.Sources;

namespace partner_aluro.Models
{
    public class Cart
    {
        private readonly ApplicationDbContext _context;
        public Cart(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cartId);



            return new Cart(context) { Id = cartId };
        }

        public CartItem GetCartItem(Product product)
        {
            return _context.CartItems.SingleOrDefault(ci =>
                ci.Product.ProductId == product.ProductId && ci.CartId == Id);
        }

        public void AddToCart(Product product, int quantity)
        {

            var Rabat = Core.Constants.Rabat;

            product.CenaProduktuDlaUzytkownika = product.CenaProduktu * (1 - (Rabat / 100));


            var cartItem = GetCartItem(product);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    CartId = Id
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
            var cartItems = _context.CartItems.Where(ci => ci.CartId == Id);

            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public List<CartItem> GetAllCartItems()
        {
            return CartItems ??
                (CartItems = _context.CartItems.Where(ci => ci.CartId == Id)
                    .Include(ci => ci.Product)
                    .ToList());
        }

        public int GetCartTotal()
        {
            int CartTotal = (int)_context.CartItems
                .Where(ci => ci.CartId == Id)
                .Select(ci => ci.Product.CenaProduktu * ci.Quantity)
                .Sum();

            CartTotal = (int)(CartTotal * (1 - (Core.Constants.Rabat / 100)));

            return CartTotal;
        }
    }
}
