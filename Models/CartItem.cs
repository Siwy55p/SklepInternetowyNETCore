using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public Product Product { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public decimal CenaProduktuBrutto { get; set; }
        public decimal CenaProduktuNetto { get; set; }

        public string CartIds { get; set; }

        //[ForeignKey("CartId")]
        public virtual Cart? Carts { get; set;  }
    }
}
