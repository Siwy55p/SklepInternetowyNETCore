using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }

        public int? ProductID { get; set; }

        public int? CategoryID { get; set; }

        virtual public Product? Product { get; set; }

        virtual public Category? Category { get; set; }

    }
}
