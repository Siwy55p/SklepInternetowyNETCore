using Microsoft.EntityFrameworkCore.Diagnostics;
using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProfildzialalnosciService  
    {
        List<ProfilDzialalnosci> GetListAllProfils();

        void Create(ProfilDzialalnosci profil);

        void Update(ProfilDzialalnosci profil);
        void Delete(ProfilDzialalnosci profil);

        ProfilDzialalnosci GetProfil(int Id);

        decimal GetRabat(string UserId);

    }
}
