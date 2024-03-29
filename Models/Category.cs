﻿using Microsoft.EntityFrameworkCore;
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

        [Required(ErrorMessage = "Wprowadz nazwę kategorii")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int? kolejnosc { get; set; }

        public string? NazwaPlikuIkony { get; set; }

        public bool? Aktywny { get; set; }




        //Kategoria przechowuje produkty
        public virtual ICollection<Product>? Produkty { get; set; }

    }


    //public class SubCategory 
    //{

    //    [Key]
    //    public int SubCategoryId { get; set; }
    //    public string Name { get; set; }

    //    public bool? Aktywny { get; set; }


    //    public string? Description { get; set; }

    //    public int? kolejnosc { get; set; }

    //    public int CatID { get; set; }

    //    // Foreign key 
    //    [Display(Name = "Category")]
    //    public int SubCatId { get; set; }
    //    [ForeignKey("SubCatId")]
    //    public virtual Category Category { get; set; }

    //    public virtual ICollection<Product>? Produkty { get; set; }


    //}


}
