using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cena { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
