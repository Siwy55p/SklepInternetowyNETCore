using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class ProfilDzialalnosci
    {
        [Key]
        public int Id { get; set; }

        public string? IdUser { get; set; }

        public string NazwaProfilu { get; set; }

        public double? Rabat { get; set; }

        [ForeignKey(nameof(IdUser))]
        [Display(Name = "Twój profil działalności")]
        public virtual List<ApplicationUser> UserProfilDzialalnosci { get; set; }
    }
}
