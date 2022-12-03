using Microsoft.EntityFrameworkCore;
using partner_aluro.Services.Interfaces;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace partner_aluro.Models
{

    [Index(nameof(Symbol),IsUnique =true)]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int ProductImagesId { get; set; }

        public string? EAN13 { get; set; }

        public string? Wymiar_wewnetrzny { get; set; }

        [Required(ErrorMessage = "Pole Nazwa musi być wypełnione")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Symbol jest wymagany np: A01532 i musi być unikatowy")]
        [StringLength(13)]
        public string Symbol { get; set; }
        public string? KrotkiOpis { get; set; }

        public string? Description { get; set; }
        public DateTime? DataDodania { get; set; } = DateTime.Now;
        public string? NazwaPlikuObrazka { get; set; }

        [Required(ErrorMessage = "Cena Produktu jest wymagana")]
        public decimal CenaProduktu { get; set; }

        [NotMapped]
        public decimal CenaProduktuDlaUzytkownika { get; set; }

        public decimal CenaPromocyja { get; set; } //Cena promocyjna
        public bool Promocja { get; set; } = false;

        public string? Pakowanie { get; set; }

        public string? Materiał { get; set; }
        public int? Ilosc { get; set; }

        public decimal CenaProduktuDetal { get; set; }
        public decimal? WagaProduktu { get; set; }
        public decimal? SzerokoscProduktu { get; set; }
        public decimal? WysokoscProduktu { get; set; }
        public decimal? GlebokoscProduktu { get; set; }

        public decimal? SzerokoscWewnetrznaProduktu { get; set; }
        public decimal? WysokoscWewnetrznaProduktu { get; set; }
        public decimal? GlebokoscWewnetrznaProduktu { get; set; }

        public bool Bestseller { get; set; }
        public bool Ukryty { get; set; } = false;

        public string? ImageUrl { get; set; }

        //To jest obrazek glowny ktory usuwam
        //[Display(Name = "Obrazek główny")]
        //[NotMapped]
        //public virtual ImageModel? product_Image { get; set; } = new ImageModel();
        ////public IFormFile? FrontImage { get; set; }

        public virtual List<ProductCategory> Kategorie { get; set; } = new List<ProductCategory>();

        [ForeignKey(nameof(CategoryId))]
        [Display(Name="Kategoria")]
        [InverseProperty("Produkty")]
        public virtual Category? CategoryNavigation { get; set; }

        [ForeignKey(nameof(ProductImagesId))]
        public virtual List<ImageModel>? Product_Images { get; set; } = new List<ImageModel>();


    }
}
