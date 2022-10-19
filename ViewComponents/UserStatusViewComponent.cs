using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class UserStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfildzialalnosciService _profildzialalnosciService;
        public UserStatusViewComponent(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IProfildzialalnosciService profildzialalnosciService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _profildzialalnosciService = profildzialalnosciService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserStatusModel model = new UserStatusModel();
            model.User = _userManager.GetUserAsync(Request.HttpContext.User).Result;

            int idProfil = 0;
            if (model.User.IdProfilDzialalnosci != null)
            {
                idProfil = (int)model.User.IdProfilDzialalnosci;
            }

            model.User.ProfilDzialalnosci = _profildzialalnosciService.GetProfil(idProfil);
            
            return View(model);
        }
    }
}
