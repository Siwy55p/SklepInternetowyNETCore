using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace partner_aluro.Models
{
    public class Adress1rozliczeniowy
    {
        

        [Required]
        [Key] //Entity inkrementacja po ID
        public int Adres1rozliczeniowyId { get; set; }

        [Required(ErrorMessage = "Pole Miasto jest wymagane")]
        public string Miasto { get; set; }

        [Required(ErrorMessage = "Pole Kraj jest wymagane")]
        public string Kraj { get; set; }

        [Required(ErrorMessage = "Pole Ulica jest wymagane")]
        public string Ulica { get; set; }

        [Display(Name = "Nr Nieruchomości")]
        public string? NrNieruchomosci { get; set; }

        [Display(Name = "Nr Lokalu")]
        public string? NrLokalu { get; set; }


        [Required(ErrorMessage = "Pole kod-pocztowy jest wymagane")]
        [StringLength(7)]
        public string? KodPocztowy { get; set; }


        [StringLength(9)]
        public string Telefon { get; set; }

        [Display(Name = "Nr Nip/VAT")]
        public string Vat { get; set; }

        public string? Wojewodztwo { get; set; }

        public string? Powiat { get; set; }

        public string? Gmina { get; set; }

        [Display(Name = "StatusNip")]
        public string? StatusNip { get; set; }

        [Display(Name = "DataZakonczeniaDzialalnosci")]
        public string? DataZakonczeniaDzialalnosci { get; set; }

        [Display(Name = "Nr Regon")]
        public string? Regon { get; set; }

        public string? UserID { get; set; }

        public string? Adres1UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? ApplicationUser { get; set; }





    }

}
