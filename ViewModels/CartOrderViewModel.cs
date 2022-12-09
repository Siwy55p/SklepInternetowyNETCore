using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.ViewModels
{
    public class CartOrderViewModel
    {

        public Cart? Carts { get; set; }
        public Order? Orders { get; set; }


        public List<CartItem>? cartItems { get; set; }

        public IEnumerable<IGrouping<string, CartItem>>? group { get; set; }
    }



}
