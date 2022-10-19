using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageId { get; set; }


        public int? ProductId { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        public string Tytul { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string? ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public virtual Product Product { get; set; }
    }
}
