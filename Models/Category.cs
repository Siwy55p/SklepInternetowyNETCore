using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Category
    {
        [Key] //Entity inkrementacja po ID
        public int CategoryId { get; set; }

        public int? ParentId { get; set; }

        public int? ChildId { get; set; }
        [InverseProperty(nameof(SubCategory.Categories))]
        public ICollection<SubCategory>? SubCategories { get; set; } = new List<SubCategory>();


        [Required(ErrorMessage = "Wprowadz nazwę kategorii")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int? kolejnosc { get; set; }

        public string? NazwaPlikuIkony { get; set; }

        public bool? Aktywny { get; set; }




        //Kategoria przechowuje produkty
        public virtual ICollection<Product>? Produkty { get; set; }

    }
    public class SubCategory : Category
    {

        [NotMapped]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign key 
        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        public bool? Aktywny { get; set; }

        [ForeignKey("CategoryId")]
        public int CatID { get; set; }

        public virtual Category Categories { get; set; }

        public virtual ICollection<Product>? Produkty { get; set; }


    }


}
