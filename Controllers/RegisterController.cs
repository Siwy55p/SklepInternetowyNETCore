using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Migrations;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;

namespace partner_aluro.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RegonService RegonService;

        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;

        private readonly ApplicationDbContext _context;

        public RegisterController(RegonService regonService, IEmailService emailService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            RegonService = regonService;
            _userManager = userManager;
            _emailService = emailService;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult ChangeValue(string culture)
        {

            if (culture == "zl")
            {
                NumberFormatInfo myNumberFormatInfo1 = new CultureInfo("pl-PL", false).NumberFormat;
                Core.Constants.myNumberFormatInfo = myNumberFormatInfo1;
            }
            else
            {
                NumberFormatInfo myNumberFormatInfo2 = new CultureInfo("de-DE", false).NumberFormat;
                Core.Constants.myNumberFormatInfo = myNumberFormatInfo2;
            }


            return Redirect(Request.Headers["Referer"].ToString());
        }

        //https://localhost:44315/Register/unsubscribe

        //https://localhost:44315/Register/unsubscribe/?Email=marcin@aluro.pl
        public async Task<IActionResult> Unsubscribe(string Email)
        {
            //var user = await _userManager.FindByEmailAsync(Email);

            var user = _context.Users.Where(x => x.UserName == Email).FirstOrDefault();
            user.Newsletter = false;
            _context.Users.Update(user);
            _context.SaveChanges();

            return View();
        }

        public async Task<IActionResult> ResetPassSend(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                if (user == null )
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("/Identity/Account/ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);



                EmailDto newClint = new EmailDto()
                {
                    Subject = "Reset hasła",
                    To = Email,
                    Body = $@"<div>
<p>Proszę zresetuj twoje hasło: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> naciskając tutaj.</a>.<p>
<p>Dziękujemy Zespół Aluro.</p> </div>"
                };

                _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("/Identity/Account/ForgotPasswordConfirmation");
            }

            return RedirectToPage("/Identity/Account/ForgotPasswordConfirmation");
        }


        [HttpPost]
        public async Task<List<string>> SprawdzNIPAsync(string? Vat = null)
        {
            var komunikat = "Brak danych";
            var nazwa_firmy = "Nie znaleziono takiej firmy";
            var adres = "";
            var miasto = "";
            var kod_pocztowy = "";

            EuropeanVatInformation companyEuropa = EuropeanVatInformation.Get(Vat);

            if(companyEuropa != null)
            {
                komunikat = "Aktywne";
                nazwa_firmy = companyEuropa.Name;
                adres = companyEuropa.Address;
                miasto = companyEuropa.CountryCode;
                kod_pocztowy = "";


                List<string> list1 = new List<string>();

                list1.Add(komunikat);
                list1.Add(nazwa_firmy);
                list1.Add(adres);
                list1.Add(miasto);
                list1.Add(kod_pocztowy);


                return list1;
            }

            CompanyModel _model = new CompanyModel();

            _model.Vat = Vat;
            _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);

            komunikat = "Brak danych";
            nazwa_firmy = "Nie znaleziono takiej firmy";
            adres = "";
            miasto = "";
            kod_pocztowy = "";

            if (_model.Errors.Count > 0)
            {
                komunikat = _model.Errors[0].ErrorMessagePl;

            }
            else
            {
                nazwa_firmy = _model.Name;
                adres = _model.Address;
                miasto = _model.Miejscowosc;
                kod_pocztowy = _model.KodPocztowy;
            }

            List<string> list = new List<string>();

            list.Add(komunikat);
            list.Add(nazwa_firmy);
            list.Add(adres);
            list.Add(miasto);
            list.Add(kod_pocztowy);


            return list;
        }
        [HttpPost]
        public async Task<List<string>> SprawdzNIPAdminAddUser(string? Vat = null)
        {

            CompanyModel _model = new CompanyModel();

            _model.Vat = Vat;
            _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);

            var komunikat = "Brak danych";
            var NazwaFirmy = "Nie znaleziono takiej firmy";

            var Regon = "";
            var Wojewodztwo1 = "";
            var Powiat1 = "";
            var Ulica1 = "";
            var Kraj1 = "";
            var Miasto1 = "";
            var KodPocztowy1 = "";

            var Ulica2 = "";
            var Kraj2 = "";
            var Miasto2 = "";
            var KodPocztowy2 = "";

            if (_model.Errors.Count > 0)
            {
                komunikat = _model.Errors[0].ErrorMessagePl;

            }
            else
            {
                Regon = _model.Regon;
                Wojewodztwo1 = _model.Wojewodztwo;

                NazwaFirmy = _model.Name;
                Regon = _model.Regon;
                Wojewodztwo1 = _model.Wojewodztwo;
                Powiat1 = _model.Powiat;
                Ulica1 = _model.Ulica;

                Miasto1 = _model.Miejscowosc;
                KodPocztowy1 = _model.KodPocztowy;
            }

            List<string> list = new List<string>();

            list.Add(komunikat);
            list.Add(NazwaFirmy);
            list.Add(Regon);
            list.Add(Wojewodztwo1);
            list.Add(Powiat1);
            list.Add(Ulica1);
            list.Add(Kraj1);
            list.Add(Miasto1);
            list.Add(KodPocztowy1);


            return list;
        }
    }
}
