﻿using Microsoft.AspNetCore.Identity;
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
using partner_aluro.Data;
using static DeepL.Model.Usage;
using Microsoft.EntityFrameworkCore;

namespace partner_aluro.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProfildzialalnosciService _profildzialalnosciService;

        private readonly IUserRepository _userRepository;

        private readonly RegonService RegonService;


        private readonly IUnitOfWorkAdress1rozliczeniowy _unitOfWorkAdress1rozliczeniowy;
        private readonly IUnitOfWorkAdress2dostawy _unitOfWorkAdress2dostawy;

        private readonly ApplicationDbContext _context;

        private readonly IAdress1rozliczeniowyService _adress1RozliczeniowyService;
        private readonly IAdress2dostawyService _adress2DostawyService;

        private readonly IEmailService _emailService;

        public UserController(ApplicationDbContext context ,RegonService regonService, IEmailService emailService, IUnitOfWorkAdress2dostawy unitOfWorkAdress2dostawy, IUnitOfWorkAdress1rozliczeniowy unitOfWorkAdress1Rozliczeniowy, IAdress2dostawyService adress2DostawyServicee, IAdress1rozliczeniowyService adress1RozliczeniowyService, IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, IProfildzialalnosciService profildzialalnosciService, IUserRepository userRepository)
        {
            _context = context;

            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _profildzialalnosciService = profildzialalnosciService;
            _adress1RozliczeniowyService = adress1RozliczeniowyService;
            _adress2DostawyService = adress2DostawyServicee;

            _unitOfWorkAdress1rozliczeniowy = unitOfWorkAdress1Rozliczeniowy;
            _unitOfWorkAdress2dostawy = unitOfWorkAdress2dostawy;

            _emailService = emailService;

            RegonService = regonService;
            _userRepository = userRepository;

        }

        public IActionResult Index()
        {
            //ICollection<ApplicationUser> users = _unitOfWork.User.GetUsers();

            var users = _context.Users.Include(x => x.Adress1rozliczeniowy)
                .Include(x => x.Adress2dostawy)
                .Where(x=>x.Usuniety != true)
                .ToList();

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


        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await _signInManager.UserManager.FindByIdAsync(id);
            
            if (user != null)
            {
                //_adress1RozliczeniowyService.Delete(user.Adress1rozliczeniowyId);
                //_adress2DostawyService.DeleteUserId(user.Id);
                if (user.Adress1rozliczeniowyId != null)
                {
                    Adress1rozliczeniowy adress1 = _context.Adress1rozliczeniowy.Where(x => x.Adres1rozliczeniowyId == user.Adress1rozliczeniowyId).FirstOrDefault();
                    _context.Adress1rozliczeniowy.Remove(adress1);
                    _context.SaveChanges();
                }
                var existAdress1 = _context.Adress1rozliczeniowy.Where(x => x.UserID == id).FirstOrDefault();
                if(existAdress1 !=null)
                {
                    //Adress1rozliczeniowy adress1Remove = _context.Adress1rozliczeniowy.Where(x => x.UserID == id).FirstOrDefault();
                    _context.Adress1rozliczeniowy.Remove(existAdress1);
                    _context.SaveChanges();
                }

                if (user.Adress2dostawyId != null)
                {
                    Adress2dostawy adress2 = _context.Adress2dostawy.Where(x => x.Adres2dostawyId == user.Adress2dostawyId).FirstOrDefault();
                    _context.Adress2dostawy.Remove(adress2);
                    _context.SaveChanges();
                }

                var existAdress2dostawt = _context.Adress2dostawy.Where(x => x.UserID == id).FirstOrDefault();
                if (existAdress2dostawt != null)
                {
                    //Adress2dostawy adres2Dostawy = _context.Adress2dostawy.Where(x => x.UserID == id).FirstOrDefault();
                    _context.Adress2dostawy.Remove(existAdress2dostawt);
                    _context.SaveChanges();
                }

                //var existCart1 = _context.Carts.Where(x => x.UserId == id).ToList();
                //if(existCart1.Any())
                //{ 
                //    var existCart = _context.Carts.Where(x => x.UserId == id).OrderBy(x => x.Id).Last();
                //    if (existCart != null)
                //    {
                //        _context.Carts.Remove(existCart);
                //        _context.SaveChanges();
                //    }
                //}

                user.Aktywny = false;
                user.Usuniety = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                return View("Index");
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index");
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

            user.Adress1rozliczeniowy = data.User.Adress1rozliczeniowy;
            user.Adress2dostawy = data.User.Adress2dostawy;
            user.Nowy = false;

            for (int i = 0; i < rolesToAdd.Count(); i++)
            {
                if (rolesToAdd[i] == "Klient")
                {
                    user.Aktywny = true;
                    //powiadomienie ze konto zostalo aktywowane wyslanie emaila do uzytkownika
                    var code = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    var callbackUrlHome = Url.Page(
                        "/Account/Login",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    EmailDto newClint = new EmailDto()
                    {
                        Subject = "Twoje konto zostało aktywowane.",
                        To = user.Email,
                        Body = $"<table class=\"newsletter-pro-container\" style=\"background-color: #ececec; font-family: Arial, sans-serif; width: 100%;\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table class=\"newsletter-pro-content\" style=\"margin: 1% auto; width: 646px; background-color: rgb(255, 255, 255); height: 549.25px;\" align=\"center\">\r\n<tbody>\r\n<tr style=\"height: 549.25px;\">\r\n<td style=\"height: 549.25px;\">\r\n<table style=\"border-collapse: collapse; width: 100%; height: 402.719px;\" border=\"0\">\r\n<tbody>\r\n<tr style=\"height: 181.391px;\">\r\n<td style=\"text-align: center; height: 181.391px;\">\r\n<p><span style=\"font-family: Arial, Helvetica, sans-serif;\"><img src=\"http://www.partner.aluro.pl/img/cms/log-png.png\" alt=\"\" width=\"358\" height=\"211\"></span></p>\r\n</td>\r\n</tr>\r\n<tr style=\"height: 159.953px;\">\r\n<td style=\"text-align: left; height: 159.953px;\" align=\"center\">\r\n<p style=\"text-align: center;\"><strong><span style=\"font-family: Arial, Helvetica, sans-serif;\">Twoje konto zostało aktywnowane.</span></strong></p>\r\n<p style=\"text-align: center;\"><span style=\"font-family: Arial, Helvetica, sans-serif;\">Możesz zalogować się do portalu <a href='{HtmlEncoder.Default.Encode(callbackUrlHome)}'>klikając tutaj</a>,&nbsp;</span></p>\r\n<p style=\"text-align: center;\"><span style=\"font-family: Arial, Helvetica, sans-serif;\">lub jeśli nie pamiętasz hasła to <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj aby je zresetować.</a></span></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>"
                    };

                    _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.
                }
            }

            for (int i = 0; i < rolesToDelete.Count(); i++)
            {
                if (rolesToDelete[i] == "Klient")
                {
                    user.Aktywny = false;
                    //powiadomienie ze konto uzytkownika zostalo zablokowane wiec wysylam email do uzytkownika z informacja o tym
                    var code = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    var callbackUrlHome = Url.Page(
                        "/Account/Login",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    EmailDto newClint = new EmailDto()
                    {
                        Subject = "Twoje konto zostało zablokowane.",
                        To = user.Email,
                        Body = $"<table class=\"newsletter-pro-container\" style=\"background-color: #ececec; font-family: Arial, sans-serif; width: 100%;\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table class=\"newsletter-pro-content\" style=\"margin: 1% auto; width: 646px; background-color: rgb(255, 255, 255); height: 549.25px;\" align=\"center\">\r\n<tbody>\r\n<tr style=\"height: 549.25px;\">\r\n<td style=\"height: 549.25px;\">\r\n<table style=\"border-collapse: collapse; width: 100%; height: 402.719px;\" border=\"0\">\r\n<tbody>\r\n<tr style=\"height: 181.391px;\">\r\n<td style=\"text-align: center; height: 181.391px;\">\r\n<p><span style=\"font-family: Arial, Helvetica, sans-serif;\"><img src=\"http://www.partner.aluro.pl/img/cms/log-png.png\" alt=\"\" width=\"358\" height=\"211\"></span></p>\r\n</td>\r\n</tr>\r\n<tr style=\"height: 159.953px;\">\r\n<td style=\"text-align: left; height: 159.953px;\" align=\"center\">\r\n<p style=\"text-align: center;\"><strong><span style=\"font-family: Arial, Helvetica, sans-serif;\">Twoje konto zostało zablokowane.</span></strong></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>"
                    };

                    _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.
                }
            }

            _unitOfWork.User.UpdateUser(user);

            //_userRepository.UpdateUser(user);

            //var Adres1roz = _unitOfWorkAdress1Rozliczeniowy.adress1Rozliczeniowy.Get(data.User.Id);

            //_unitOfWorkAdress1Rozliczeniowy.adress1Rozliczeniowy.Update(data.User.Adress1rozliczeniowy);

            //_adress1RozliczeniowyService.Update(data.User.Adress1rozliczeniowy);
            //_adress2DostawyService.Update(data.User.Adress2dostawy);

            //return RedirectToAction("Update", new { id = user.Adres1rozliczeniowyId });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Add() //Zapis uzytkownika do bazy
        {
            ViewData["Profile"] = GetProfiles();

            var user = CreateUser();


            Adress1rozliczeniowy adres1 = new Adress1rozliczeniowy();

            Adress2dostawy adres2 = new Adress2dostawy();

            user.Adress1rozliczeniowy = adres1;
            user.Adress2dostawy = adres2;
            user.Nowy = true;
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
        public async Task<IActionResult> Add(EditUserViewModel data) //Zapis uzytkownika do bazy
        {
            //utworz uzytkownika i zapisz do bazy na podstawie zmiennej data

            CompanyModel _model = new CompanyModel();

            _model.Vat = data.User.Adress1rozliczeniowy.Vat;
            _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);

            data.User.Adress1rozliczeniowy.Regon = _model.Regon;

            //if (data.User.Adress1rozliczeniowy.Wojewodztwo != null)
            //{
            //    data.User.Adress1rozliczeniowy.Wojewodztwo = _model.Wojewodztwo;
            //}

            //if (data.User.NazwaFirmy != null)
            //{
            //    data.User.NazwaFirmy = _model.Name;
            //}

            //if (data.User.Adress1rozliczeniowy.Wojewodztwo != null)
            //{
            //    data.User.Adress1rozliczeniowy.Wojewodztwo = _model.Wojewodztwo;
            //}
            //if (data.User.Adress1rozliczeniowy.Powiat != null)
            //{
            //    data.User.Adress1rozliczeniowy.Powiat = _model.Powiat;
            //}

            //if (data.User.Adress1rozliczeniowy.Ulica != null)
            //{
            //    data.User.Adress1rozliczeniowy.Ulica = _model.Ulica;
            //}

            //if (data.User.Adress1rozliczeniowy.Miasto != null)
            //{
            //    data.User.Adress1rozliczeniowy.Miasto = _model.Miejscowosc;
            //}

            //if (data.User.Adress1rozliczeniowy.KodPocztowy != null)
            //{
            //    data.User.Adress1rozliczeniowy.KodPocztowy = _model.KodPocztowy;
            //}

            //if (data.User.Adress2dostawy.Miasto != null)
            //{
            //    data.User.Adress2dostawy.Miasto = data.User.Adress1rozliczeniowy.Miasto;
            //}

            //if (data.User.Adress2dostawy.Ulica != null)
            //{
            //    data.User.Adress2dostawy.Ulica = data.User.Adress1rozliczeniowy.Ulica;
            //}


            //if (data.User.Adress2dostawy.KodPocztowy != null)
            //{ 
            //    data.User.Adress2dostawy.KodPocztowy = data.User.Adress1rozliczeniowy.KodPocztowy;
            //}

            var user = CreateUser();


            ViewData["Profile"] = GetProfiles();


            user = data.User;
            if (user == null)
            {
                return View(data);
                //return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);


            if (!ModelState.IsValid == true)
            {
                return View(data);
            }


            user.Imie = data.User.Imie;
            user.Nazwisko = data.User.Nazwisko;
            user.Email = data.User.Email;
            user.UserName = data.User.UserName;
            user.NotatkaOsobista = data.User.NotatkaOsobista;
            user.IdProfilDzialalnosci = data.User.IdProfilDzialalnosci;
            user.NazwaFirmy = data.User.NazwaFirmy;

            user.Newsletter = true;

            user.Nowy = true;
            user.DataZałożenia = DateTime.Now;

            user.Adress1rozliczeniowy = data.User.Adress1rozliczeniowy;
            user.Adress2dostawy = data.User.Adress2dostawy;
            //_unitOfWork.User.UpdateUser(user);



            _unitOfWork.User.UpdateUser(user);

            var nowyAdres1rozliczeniowy = _unitOfWorkAdress1rozliczeniowy.adress1Rozliczeniowy.Get(user.Id);

            if (nowyAdres1rozliczeniowy == null)
            {
                nowyAdres1rozliczeniowy = new Adress1rozliczeniowy();
                nowyAdres1rozliczeniowy.UserID = user.Id;
                nowyAdres1rozliczeniowy.Adres1UserID = user.Id;
                nowyAdres1rozliczeniowy.Ulica = user.Adress1rozliczeniowy.Ulica;
                nowyAdres1rozliczeniowy.Kraj = user.Adress1rozliczeniowy.Kraj;
                nowyAdres1rozliczeniowy.Miasto = user.Adress1rozliczeniowy.Miasto;
                nowyAdres1rozliczeniowy.KodPocztowy = user.Adress1rozliczeniowy.KodPocztowy;
                nowyAdres1rozliczeniowy.Telefon = user.Adress1rozliczeniowy.Telefon;
                nowyAdres1rozliczeniowy.Vat = user.Adress1rozliczeniowy.Vat;
                nowyAdres1rozliczeniowy.Regon = user.Adress1rozliczeniowy.Regon;

                _context.Adress1rozliczeniowy.Add(nowyAdres1rozliczeniowy);
                _context.SaveChanges();
            }

            var nowyAdres2dostawy = _unitOfWorkAdress2dostawy.adress2dostawy.Get(user.Id);

            if (nowyAdres2dostawy == null)
            {
                nowyAdres2dostawy = new Adress2dostawy();
                nowyAdres2dostawy.UserID = user.Id;
                nowyAdres2dostawy.Adres2UserID = user.Id;
                nowyAdres2dostawy.Ulica = user.Adress2dostawy.Ulica;
                nowyAdres2dostawy.Kraj = user.Adress2dostawy.Kraj;
                nowyAdres2dostawy.Miasto = user.Adress2dostawy.Miasto;
                nowyAdres2dostawy.KodPocztowy = user.Adress2dostawy.KodPocztowy;
                nowyAdres2dostawy.Email = user.Adress2dostawy.Email;
                nowyAdres2dostawy.Imie = user.Adress2dostawy.Imie;
                nowyAdres2dostawy.Nazwisko = user.Adress2dostawy.Nazwisko;
                _context.Adress2dostawy.Add(nowyAdres2dostawy);
                _context.SaveChanges();
            }





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

            return RedirectToAction("Index");

            //var Adres1roz = _unitOfWorkAdress1Rozliczeniowy.adress1Rozliczeniowy.Get(data.User.Id);


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
                    Body = $"<table style=\"background-color: #ececec; font-family: Arial, sans-serif; width: 100%;\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table class=\"newsletter-pro-content\" style=\"margin: 1% auto; width: 646px; background-color: rgb(255, 255, 255); height: 549.25px;\" align=\"center\">\r\n<tbody>\r\n<tr style=\"height: 549.25px;\">\r\n<td style=\"height: 549.25px;\">\r\n<table style=\"border-collapse: collapse; width: 100%; height: 402.719px;\" border=\"0\">\r\n<tbody>\r\n<tr style=\"height: 181.391px;\">\r\n<td style=\"text-align: center; height: 181.391px;\">\r\n<p><span style=\"font-family: Arial, Helvetica, sans-serif;\"><img src=\"http://www.partner.aluro.pl/img/cms/log-png.png\" alt=\"\" width=\"358\" height=\"211\"></span></p>\r\n</td>\r\n</tr>\r\n<tr style=\"height: 159.953px;\">\r\n<td style=\"text-align: left; height: 159.953px;\" align=\"center\">\r\n<p style=\"text-align: center;\"></p>\r\n<p style=\"text-align: center;\"><span style=\"font-family: Arial, Helvetica, sans-serif;\">Proszę zresetuj swoje hasło: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> klikacjąc tutaj.</a></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>"
                };

                _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                return View(user);
                //return RedirectToAction("Edit", new { id = user.Id });
            }

            return RedirectToAction("Index");
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


        public async Task AddToNewsletter(string UserName, bool check)
        {

            var user = await _signInManager.UserManager.FindByEmailAsync(UserName);
            user.Newsletter = check;
            _context.Users.Update(user);
            _context.SaveChanges();

        }

    }

}
