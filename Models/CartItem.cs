using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string CartId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime Data { get; set; }
    }
}
