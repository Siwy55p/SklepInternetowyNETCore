using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class ProductNazwyPrestashop
    {
        [Key]
        public int Id { get; set; } 

        public int? id_product { get; set; }
        public int? id_shop { get; set; }
        public int? id_lang { get; set; }
        public string? description { get; set; }
        public string? description_short { get; set; }
        public string? link_rewrite { get; set; }
        public string? meta_description { get; set; }
        public string? meta_keywords { get; set; }
        public string? meta_title { get; set; }
        public string? name { get; set; }
        public string? available_now { get; set; }
        public string? available_later { get; set; }

    }
}
