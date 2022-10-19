using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Models;
using partner_aluro.Core.Repositories;
using partner_aluro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using static partner_aluro.Core.Constants;
using partner_aluro.Core;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProfildzialalnosciService _profildzialalnosciService;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, IProfildzialalnosciService profildzialalnosciService)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _profildzialalnosciService = profildzialalnosciService;
        }

        public IActionResult Index()
        {
            ICollection<ApplicationUser> users = _unitOfWork.User.GetUsers();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Profile"] = GetProfiles();


            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name)))).ToList();

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = roleItems
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel data) //Zapis uzytkownika do bazy
        {
            var user = _unitOfWork.User.GetUser(data.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            //Loop through the roles in ViewModel
            //Check if the Role is Assigned In DB
            //If Assigned -> Do Nothing
            //If Not Assigned -> Add Role

            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();

            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    //Add role
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    //Remove Role
                    if (assignedInDb != null)
                    {
                        rolesToDelete.Add(role.Text);
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.Imie = data.User.Imie;
            user.Nazwisko = data.User.Nazwisko;
            user.Email = data.User.Email;
            user.UserName = data.User.UserName;
            user.NotatkaOsobista = data.User.NotatkaOsobista;
            user.IdProfilDzialalnosci = data.User.IdProfilDzialalnosci;
            user.NazwaFirmy = data.User.NazwaFirmy;
            _unitOfWork.User.UpdateUser(user);

            //return RedirectToAction("Update", new { id = user.Adres1rozliczeniowyId });
            return RedirectToAction("Index");
        }


        private List<SelectListItem> GetProfiles() //Pobierz profile dzialalnbosci rabaty w zaleznosci jakiprofil dzialalnosci (sklep internetowy, stacjonarny)
        {
            var lstProfiles = new List<SelectListItem>();

            lstProfiles = _profildzialalnosciService.GetListAllProfils().Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.NazwaProfilu
            }).ToList();

            return lstProfiles;
        }
    }
}
