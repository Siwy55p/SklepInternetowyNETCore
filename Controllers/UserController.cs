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
using partner_aluro.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace partner_aluro.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkAdress1rozliczeniowy _unitOfWorkAdress1Rozliczeniowy;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProfildzialalnosciService _profildzialalnosciService;


        private readonly IAdress1rozliczeniowyService _adress1RozliczeniowyService;
        private readonly IAdress2dostawyService _adress2DostawyService;

        private readonly IEmailService _emailService;

        public UserController(IEmailService emailService, IUnitOfWorkAdress1rozliczeniowy unitOfWorkAdress1Rozliczeniowy, IAdress2dostawyService adress2DostawyServicee, IAdress1rozliczeniowyService adress1RozliczeniowyService, IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, IProfildzialalnosciService profildzialalnosciService)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _profildzialalnosciService = profildzialalnosciService;
            _adress1RozliczeniowyService = adress1RozliczeniowyService;
            _adress2DostawyService = adress2DostawyServicee;

            _unitOfWorkAdress1Rozliczeniowy = unitOfWorkAdress1Rozliczeniowy;

            _emailService = emailService;
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

        //[HttpPost]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    ApplicationUser user = await _signInManager.UserManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        //_adress1RozliczeniowyService.Delete(user.Adress1rozliczeniowyId);
        //        //_adress2DostawyService.DeleteUserId(user.Id);
        //        IdentityResult result = await _signInManager.UserManager.DeleteAsync(user);
        //            if (result.Succeeded)
        //            return RedirectToAction("Index");
        //        else
        //            Errors(result);
        //    }
        //    else
        //        ModelState.AddModelError("", "User Not Found");
        //    return View("Index");
        //}


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


            var Adres1roz = _unitOfWorkAdress1Rozliczeniowy.adress1Rozliczeniowy.Get(data.User.Id);

            //_unitOfWorkAdress1Rozliczeniowy.adress1Rozliczeniowy.Update(data.User.Adress1rozliczeniowy);

            //_adress1RozliczeniowyService.Update(data.User.Adress1rozliczeniowy);
            //_adress2DostawyService.Update(data.User.Adress2dostawy);

            //return RedirectToAction("Update", new { id = user.Adres1rozliczeniowyId });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Add(EditUserViewModel data) //Zapis uzytkownika do bazy
        {

            return View();
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
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public async Task<IActionResult> ResetPasswordUser(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                if (user == null)
                {

                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                EmailDto newClint = new EmailDto()
                {
                    Subject = "Reset hasła",
                    To = email,
                    Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                };

                _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                return RedirectToAction("Edit", new { id = user.Id });
            }
            return RedirectToAction("Index");
        }

    }

}
