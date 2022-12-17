using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
        public string CartIds { get; set; }

        //[ForeignKey("CartId")]
        public virtual Cart? Carts { get; set;  }
    }
}
