using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class AddresPrestashop
    {
        [Key]
        public int Id { get; set; }

        public int? id_address { get; set; }

        public int? id_country { get; set; }
        public int? id_state { get; set; }
        public int? id_customer { get; set; }
        public int? id_manufacturer { get; set; }
        public int? id_supplier { get; set; }
        public int? id_warehouse { get; set; }

        public string? alias { get; set; }

        public string? company { get; set; }

        public string? lastname { get; set; }
        public string? firstname { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? postcode { get; set; }
        public string? city { get; set;}
        public string? other { get; set; }
        public string? phone { get; set; }
        public string? phone_mobile { get; set;}

        public string? vat_number { get; set; }
        public string? dni { get; set; }

        public DateTime? date_add { get; set; }

        public DateTime? date_upd { get; set; }

        public int? active { get; set; }

        public int? deleted { get; set;}


    }
}
