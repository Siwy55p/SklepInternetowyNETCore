
//using partner_aluro.Models;
//using ServiceReference2;

using partner_aluro.Models;

namespace partner_aluro.ViewModels
{
    public class UserStatusModel
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Email { get; set; }

        public virtual ProfilDzialalnosci Profil { get; set; }
        //public int idProfil { get; set; }

        //public ApplicationUser? User { get; set; }
    }
}
