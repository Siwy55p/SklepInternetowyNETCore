
using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class ProductQuantityPrestashop
    {
        [Key]
        public int id { get; set; }

        public int id_stock_available { get; set; }

        public int id_product { get; set; }

        public int id_product_attribute { get; set; }
        public int id_shop { get; set; }
        public int quantity { get; set; }
        public int depends_on_stock { get; set; }
        public int out_of_stock { get; set; }

    }
}
