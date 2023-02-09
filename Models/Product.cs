using Microsoft.AspNetCore.Mvc.Rendering;
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

        public string SzukanaNazwa { get; set; }

        [Required(ErrorMessage = "Cena Produktu jest wymagana")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CenaProduktuBrutto { get; set; }  // Cena Producktu (wczesniej netto) nazwa CenaProduktu
        [Column(TypeName = "decimal(18,2)")]
        public decimal CenaProduktuNetto { get; set; }

        [NotMapped]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CenaProduktuDlaUzytkownika { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CenaPromocyja { get; set; } //Cena promocyjna
        public bool Promocja { get; set; } = false;

        public string? Pakowanie { get; set; }
        public string? NazwaPromocyjna { get; set; }

        public string? Materiał { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Ilosc { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CenaProduktuDetal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WagaProduktu { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SzerokoscProduktu { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WysokoscProduktu { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GlebokoscProduktu { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SzerokoscWewnetrznaProduktu { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WysokoscWewnetrznaProduktu { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GlebokoscWewnetrznaProduktu { get; set; }

        public bool Bestseller { get; set; }
        public bool Ukryty { get; set; } = false;

        public string? ImageUrl { get; set; }
        public string? pathImageUrl250x250 { get; set; }

        [NotMapped]
        public IList<SelectListItem>? categories { get; set; }

        public virtual List<ProductCategory> Kategorie { get; set; } = new List<ProductCategory>();

        [ForeignKey(nameof(CategoryId))]
        [Display(Name="Kategoria")]
        [InverseProperty("Produkty")]
        public virtual Category? CategoryNavigation { get; set; }

        [ForeignKey(nameof(ProductImagesId))]
        public virtual List<ImageModel>? Product_Images { get; set; } = new List<ImageModel>();

    }
}
