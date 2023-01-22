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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserStatusViewComponent(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            return View(model);
        }
    }
}
