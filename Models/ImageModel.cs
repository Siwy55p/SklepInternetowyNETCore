using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageId { get; set; }

        public string? path { get; set; }
        public string? fullPath { get; set; }

        public int? kolejnosc { get; set; }
        public int? ProductId { get; set; }

        public int? SliderIds { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        public string? Tytul { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string? ImageName { get; set; }

        [DisplayName("Opis obrazka")]
        public string? Opis { get; set; }
        public int? ProductImagesId { get; set; } //dodatkowy id produktu

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }  //pojedynczy plik

        public virtual Product? Product { get; set; }
    }
}
