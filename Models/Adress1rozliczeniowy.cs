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

        [Required(ErrorMessage = "Pole KodPocztowy jest wymagane")]
        [StringLength(7)]
        public string? KodPocztowy { get; set; }


        [StringLength(9)]
        public string Telefon { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }




    }

}
