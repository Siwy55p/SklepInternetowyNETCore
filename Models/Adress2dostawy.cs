using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Adress2dostawy
    {
        [Key] //Entity inkrementacja po ID
        public int Adres2dostawyId { get; set; }

        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? NazwaFirmy { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Pole Ulica jest wymagane, lub nazwa ulicy jest za krótka.")]
        [StringLength(255, MinimumLength = 2)]
        public string? Ulica { get; set; }

        [Required(ErrorMessage = "Pole Miasto jest wymagane")]
        public string? Miasto { get; set; }
        public string? Kraj { get; set; }

        [Required(ErrorMessage = "Kod-pocztowy jest wymagany")]
        public string? KodPocztowy { get; set; }

        public string? Telefon { get; set; }


        public string? UserID { get; set; }
        public string? Adres2UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
