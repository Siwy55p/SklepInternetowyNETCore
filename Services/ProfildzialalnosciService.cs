using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Services
{
    public class ProfildzialalnosciService : IProfildzialalnosciService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfildzialalnosciService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ProfilDzialalnosci> GetListAllProfils()
        {
            List<ProfilDzialalnosci> Lista = _context.ProfileDzialalnosci.ToList();
            return Lista;
        }

        public ProfilDzialalnosci GetProfil(int Id)
        {
            ProfilDzialalnosci profil = _context.ProfileDzialalnosci.Find(Id);
            return profil;
        }

        public void Create(ProfilDzialalnosci profil)
        {
            _context.ProfileDzialalnosci.Add(profil);
            _context.SaveChanges();
        }

        public void Update(ProfilDzialalnosci profil)
        {
            _context.ProfileDzialalnosci.Update(profil);
            _context.SaveChanges();
        }

        public void Delete(ProfilDzialalnosci profil)
        {
            _context.ProfileDzialalnosci.Remove(profil);
            _context.SaveChanges();
        }

        public decimal GetRabat(string IdUser)
        {
            ApplicationUser User = _userManager.Users.FirstOrDefault(u => u.Id == IdUser);
            decimal Rabat = 0;
            if (User.IdProfilDzialalnosci != null)
            {
                Rabat = (decimal)_context.ProfileDzialalnosci.FirstOrDefault(x => x.Id == User.IdProfilDzialalnosci).Rabat;
            }else
            {
                ProfilDzialalnosci nowyProfil = new ProfilDzialalnosci();
                nowyProfil.Id = 0;
                nowyProfil.NazwaProfilu = "Sklep stacjonarny";
                nowyProfil.Rabat = 0;
            }


            return Rabat;
        }
    }
}