using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Newsletter
    {
        [Key]
        public int NewsletterID { get; set; }

        public string? Nazwa { get; set; }

        [NotMapped]
        public List<string>? listaEmail { get; set; }

        public string? MessagerBody { get; set; }




    }
}
