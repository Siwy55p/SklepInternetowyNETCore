using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Permissions;

namespace partner_aluro.Models
{
    public class ProductPrestashop
    {
        [Key]
        public int Id { get; set; }

        public int? id_product { get; set; } //AUTOINCREMENT

        public int? id_supplier { get; set; } //FOREGING KEY
        public int? id_manufacturer { get; set; } //FOREGING KEY
        public int? id_category_default { get; set; } //FOREGING KEY
        public int? id_shop_default { get; set; }
        public int? id_tax_rules_group { get; set; }
        public byte? on_sale { get; set; }

        public byte? online_only { get; set; }

        [StringLength(13, ErrorMessage = "EAN Max Length is 13")]
        public string? ean13 { get; set; }

        [StringLength(12, ErrorMessage = "upc Max Length is 12")]
        public string? upc { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ecotax { get; set; }
        public int? quantity { get; set; }
        public int? minimal_quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? wholesale_price { get; set; }

        [StringLength(255, ErrorMessage = "unity Max Length is 255")]
        public string? unity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? unit_price_ratio { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? additional_shipping_cost { get; set; }
        [StringLength(32, ErrorMessage = "reference Max Length is 32")]
        public string? reference { get; set; }
        [StringLength(32, ErrorMessage = "supplier_reference Max Length is 32")]
        public string? supplier_reference { get; set; }
        [StringLength(64, ErrorMessage = "location Max Length is 64")]
        public string? location { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? width { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? height { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? depth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? weight { get; set; }

        public int? out_of_stock { get; set; }
        public byte? quantity_discount { get; set; }
        public byte? customizable { get; set; }
        public byte? uploadable_files { get; set; }
        public byte? text_fields { get; set; }
        public byte? active { get; set; }
        public int? id_product_redirected { get; set; }
        public byte? available_for_order { get; set; }
        public string? available_date { get; set; }
        public condit? condition { get; set; }

        public byte? show_price { get; set; }
        public byte? indexed { get; set; }
        //public visible? visibility { get; set; }
        public byte? visibility { get; set; }
        public byte? cache_is_pack { get; set; }
        public byte? cache_has_attachments { get; set; }
        public byte? is_virtual { get; set; }
        public int? cache_default_attribute { get; set; }
        public string? date_add { get; set; }
        public string? date_upd { get; set; }
        public byte? advanced_stock_management { get; set; }
        public int? pack_stock_type { get; set; }

        public enum condit
        {
            nowe,
            used,
            refurbished
        }
        public enum visible
        {
            both,
            catalog,
            search,
            none
        }
    }

}
