using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class SubCategory : Category
    {
        private ICollection<Product>? produkty;

        [Key] //Entity inkrementacja po ID
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Wprowadz nazwę kategorii")]
        public string SubName { get; set; }

        public string? SubDescription { get; set; }

        public string? SubNazwaPlikuIkony { get; set; }

        //Kategoria moze przechowywac produckty
        public virtual Category Category { get; set; }
        public virtual ICollection<Product>? Produkty { get => produkty; set => produkty = value; }


    }
}
