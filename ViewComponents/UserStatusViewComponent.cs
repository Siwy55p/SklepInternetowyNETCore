using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class UserStatusViewComponent : ViewComponent
    {
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfildzialalnosciService _profildzialalnosciService;
        private readonly ApplicationDbContext _context;

        public UserStatusViewComponent(ApplicationDbContext context,UserManager<ApplicationUser> userManager, IProfildzialalnosciService profildzialalnosciService)
        {
            _context = context;
            //_signInManager = signInManager;
            _userManager = userManager;
            _profildzialalnosciService = profildzialalnosciService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(Request.HttpContext.User);
            UserStatusModel model = new UserStatusModel()
            {
                Email = user.Email,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                Profil = await _context.ProfileDzialalnosci.FindAsync(user.IdProfilDzialalnosci)
            };
            //model.Imie = _userManager.GetUserAsync(Request.HttpContext.User);

            //UserStatusModel model = await _context.Users.Where(x=>x.UserName == Request.HttpContext.User).FirstOrDefaultAsync();
            //await _userManager.GetUserAsync(Request.HttpContext.User);

            //if (user != null)
            //{

            //    int idProfil = 0;
            //    if (model.User.IdProfilDzialalnosci != null)
            //    {
            //        idProfil = (int)model.User.IdProfilDzialalnosci;
            //    }

            //    model.User.ProfilDzialalnosci = _profildzialalnosciService.GetProfil(idProfil);
            //}

            return View(model);
        }
    }
}
