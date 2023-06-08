using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace partner_aluro.Models;

public class ApplicationUser : IdentityUser
{
    public int IdProfilDzialalnosci { get; set; }
    [Required]
    public string Imie { get; set; }

    [Required]
    public string Nazwisko { get; set; }
    public string? NazwaFirmy { get; set; }
    public DateTime DataZałożenia { get; set; }
    public bool? Aktywny { get; set; }
    public bool? Usuniety { get; set; }
    public bool? Nowy { get; set; }
    public int? Adress1rozliczeniowyId { get; set; }
    public int? Adress2dostawyId { get; set; }

    [ForeignKey(nameof(Adress1rozliczeniowyId))]
    public virtual Adress1rozliczeniowy? Adress1rozliczeniowy { get; set; }

    [ForeignKey(nameof(Adress2dostawyId))]
    public virtual Adress2dostawy? Adress2dostawy { get; set; }

    public string? NotatkaOsobista { get; set; }
    public bool? PolitykaPrywatnosci { get; set; }
    public bool? Newsletter { get; set; }

    [InverseProperty(nameof(Order.User))]
    public virtual ICollection<Order>? Orders { get; set; }



    [Display(Name = "Profil działalności")]
    //[ForeignKey(nameof(IdProfilDzialalnosci))]
    public virtual ProfilDzialalnosci? ProfilDzialalnosci { get; set; }
}
public class ApplicationRole : IdentityRole
{

}
