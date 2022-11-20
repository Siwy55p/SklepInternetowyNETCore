using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace partner_aluro.Models
{
    public class ProductPrestashop
    {
        [Key]
        public int Id { get; set; }

        public int id_product { get; set; }

        public int id_supplier { get; set; }
        public int id_manufacturer { get; set; }
        public int id_category_default { get; set; }
        public int id_shop_default { get; set; }
        public int id_tax_rules_group { get; set; }
        public byte on_sale { get; set; }

        public byte online_only { get; set; }
        public char ean13 { get; set; }

        public char upc { get; set; }
        public decimal ecotax { get; set; }
        public int quantity { get; set; }
        public int minimal_quantity { get; set; }
        public decimal price { get; set; }
        public decimal wholesale_price { get; set; }
        public char unity { get; set; }
        public decimal unit_price_ratio { get; set; }
        public decimal additional_shipping_cost { get; set; }
        public char reference { get; set; }
        public char supplier_reference { get; set; }
        public char location { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal dept { get; set; }
        public decimal weight { get; set; }

        public int out_of_stock {get; set;}
        public byte quantity_discount { get; set; }
        public byte customizable { get; set; }
        public byte uploadable_files { get; set; }
        public byte text_fields { get; set; }
        public byte active { get; set; }
        public int id_product_redirected { get; set; }
        public byte available_for_order { get; set; }
        public DateTime available_date { get; set; }
        public condit condition { get; set; }

        public byte show_price { get; set; }
        public byte indexed { get; set; }
        public visible visibility { get; set; }
        public byte cache_is_pack { get; set; }
        public byte cache_has_attachments { get; set; }
        public byte is_virtual { get; set; }
        public int cache_default_attribute { get; set; }
        public DateTime date_add { get; set; }
        public DateTime date_upd { get; set; }
        public byte advanced_stock_management { get; set; }
        public int pack_stock_type { get; set; }

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
