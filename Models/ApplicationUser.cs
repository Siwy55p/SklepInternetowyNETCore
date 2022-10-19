using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace partner_aluro.Models;

public class ApplicationUser : IdentityUser
{
    public int? IdProfilDzialalnosci { get; set; }
    public string Imie { get; set; }
    public string Nazwisko { get; set; }
    public string? NazwaFirmy { get; set; }
    public DateTime? DataZałożenia { get; set; }

    public int? Adres1rozliczeniowyId { get; set; }
    public int? Adres2dostawyId { get; set; }

    public virtual Adress1rozliczeniowy Adres1 { get; set; }
    public virtual Adress2dostawy? Adres2 { get; set; }

    public string? NotatkaOsobista { get; set; }



    [Display(Name = "Profil działalności")]
    [ForeignKey(nameof(IdProfilDzialalnosci))]
    public virtual ProfilDzialalnosci? ProfilDzialalnosci { get; set; }
}
public class ApplicationRole : IdentityRole
{

}
