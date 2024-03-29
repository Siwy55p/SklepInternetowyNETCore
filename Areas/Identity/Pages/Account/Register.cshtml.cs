﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using System.Text.RegularExpressions;
using System.Drawing;


namespace partner_aluro.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IProfildzialalnosciService _profildzialalnosciService;
        private readonly RegonService RegonService;
        private readonly IEmailService _emailService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IProfildzialalnosciService profildzialalnosciService,
            RegonService regonService,
            IEmailService emailService
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _profildzialalnosciService = profildzialalnosciService;
            RegonService = regonService;
            _emailService = emailService;
        }


        public string komunikat { get; set; }

        public CompanyModel _model { get; set; } = new CompanyModel();

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage ="Profil działalności jest wymagany.")]
            [Display(Name = "Wybierz profil działalności.")]
            public int IdProfildzialalnosci { get; set; }

            [Required(ErrorMessage = "Pole Imię jest wymagane.")]
            [StringLength(90, ErrorMessage ="Twoje imie może mieć maksywalnie 90 znaków.")]
            [Display(Name ="Imię:")]
            public string Imie { get; set; }

            [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
            [StringLength(120, ErrorMessage = "Twoje Nazwisko może mieć maksywalnie 120 znaków.")]
            [Display(Name = "Nazwisko:")]
            public string Nazwisko { get; set; }

            [Display(Name = "Firma:")]
            public string NazwaFirmy { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Pole Ulica jest wymagane.")]
            [Display(Name = "Adres:")]
            public string Ulica { get; set; }

            [Required(ErrorMessage = "Pole Kod pocztowy jest wymagane.")]
            [StringLength(8)]
            [Display(Name = "Kod Pocztowy:")]
            public string KodPocztowy1 { get; set; }
            //public Adress1rozliczeniowy Adress { get; set; }

            [Required(ErrorMessage = "Pole Miejscowość jest wymagana.")]
            [StringLength(255, ErrorMessage = "Nazwa miejscowosci nie może być tak długa.")]
            [Display(Name = "Miasto:")]
            public string Miasto { get; set; }


            [StringLength(255, ErrorMessage = "Podaj Kraj")]
            [Display(Name = "Kraj:")]
            public string Kraj { get; set; }



            //[Phone]
            [Required(ErrorMessage = "Telefon do kontaktu jest wymagany.")]
            [Display(Name = "Telefon:")]
            //[DataType(DataType.PhoneNumber)]
            //[StringLength(10, ErrorMessage = "{0} Numer musi mieć zawierać liczby pomiędzy {2}, a {1} znaków.", MinimumLength = 6)]
            //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3,4})$", ErrorMessage = "Nie prawidłowy nr telefonu (komórkowy)")]
            public string Telefon1 { get; set; }



            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Twój adres e-mail jest wymagany.")]
            [EmailAddress]
            [Display(Name = "Email:")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Pole hasło jest wymagane.")]
            [StringLength(100, ErrorMessage = "{0} musi być od {2} do maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło:")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Powtórz hasło:")]
            [Compare("Password", ErrorMessage = "Hasło i powtórzone hasło nie pasują do siebie.")]
            public string ConfirmPassword { get; set; }


            [Required(ErrorMessage = "NIP jest wymagany.")]
            [Display(Name = "NIP:")]
            [StringLength(13, ErrorMessage = "Proszę wprowadzic poprawy nr NIP składający się z 10-ciu znaków.", MinimumLength = 9)]
            public string NIP { get; set; }

            public bool Newsletter { get; set; }

            [CheckBoxRequired(ErrorMessage = "Proszę zaakceptować Politykę prywatności.")]
            public bool PolitykaPrywatnosci { get; set; }
        }

        //[HttpGet]
        public async Task OnGetAsync(string pass, string email, string returnUrl = null)
        {
            //TUTAJ
            ViewData["Profile"] = GetProfiles();
            ViewData["email"] = email;
            ViewData["pass"] = pass;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        }

        public class CheckBoxRequired : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                //get the entered value
                var student = (InputModel)validationContext.ObjectInstance;
                //Check whether the IsAccepted is selected or not.
                if (student.PolitykaPrywatnosci == false)
                {
                    //if not checked the checkbox, return the error message.
                    return new ValidationResult(ErrorMessage == null ? "Proszę zaznaczyc checkbox" : ErrorMessage);
                }
                return ValidationResult.Success;
            }
        }

        //[HttpPost]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ViewData["Profile"] = GetProfiles();

            _model.Vat = Input.NIP;



            //_model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);


            //if (_model.Errors.Count > 0)
            //{
            //    komunikat = _model.Errors[0].ErrorMessagePl;
            //    return Page();
            ////}

            //Input.NazwaFirmy = _model.Name;
            //Input.Miasto = _model.Miejscowosc;
            //Input.Ulica = _model.Ulica;
            //Input.KodPocztowy1 = _model.KodPocztowy;

            ModelState.Remove("Input.KodPocztowy1");
            ModelState.Remove("Input.Miasto");
            ModelState.Remove("Input.Ulica"); 
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();


                user.Imie = Input.Imie;
                user.Nazwisko = Input.Nazwisko;

                Adress1rozliczeniowy adres1 = new Adress1rozliczeniowy();

                //if (_model != null && _model.Errors.Count() < 1)
                //{
                //    adres1.NrNieruchomosci = _model.NrNieruchomosci;
                //    adres1.NrLokalu = _model.NrLokalu;
                //    adres1.Vat = _model.Vat;
                //    adres1.Miasto = _model.Miejscowosc;
                //    adres1.Ulica = _model.Ulica;
                //    adres1.KodPocztowy = _model.KodPocztowy;
                //    adres1.Wojewodztwo = _model.Wojewodztwo;
                //    adres1.Kraj = "Polska";
                //    adres1.Powiat = _model.Powiat;
                //    adres1.Gmina = _model.Gmina;
                //    adres1.StatusNip = _model.StatusNip;
                //    adres1.Regon = _model.Regon;
                //    adres1.Telefon = Input.Telefon1;
                //    adres1.Adres1UserID = user.Id;
                //}


                Adress2dostawy adres2 = new Adress2dostawy();
                //if (_model != null && _model.Errors.Count() < 1)
                //{
                //    adres2.Imie = Input.Imie;
                //    adres2.Nazwisko = Input.Nazwisko;
                //    adres2.Email = Input.Email;
                //    adres2.Miasto = _model.Miejscowosc;
                //    adres2.Ulica = _model.Ulica;
                //    adres2.KodPocztowy = _model.KodPocztowy;
                //    adres2.Telefon = Input.Telefon1;
                //    adres2.Kraj = "Polska";
                //    adres2.Adres2UserID = user.Id;
                //}


                //EuropeanVatInformation companyEuropa = EuropeanVatInformation.Get(Input.NIP);

                //if (companyEuropa != null)
                //{
                //    string SplitAdres = companyEuropa.Address;

                //    string[] AdresSplit = SplitAdres.Split("\n");
                //    string ulicaS = AdresSplit[0].ToString();
                //    string kod_pocztowyS = AdresSplit[1].ToString();
                //    string krajE = AdresSplit[2].ToString();

                //    string KodPocztowyZagraniczny = new String(kod_pocztowyS.Where(Char.IsDigit).ToArray());
                //    var MiastoS = new String(kod_pocztowyS.Where(Char.IsLetter).ToArray());

                //    adres1.NrNieruchomosci = "";
                //    adres1.NrLokalu = "";
                //    adres1.Vat = Input.NIP;
                //    adres1.Miasto = MiastoS;
                //    adres1.Ulica = ulicaS;
                //    adres1.KodPocztowy = KodPocztowyZagraniczny;
                //    adres1.Wojewodztwo = companyEuropa.CountryCode;
                //    adres1.Kraj = krajE;
                //    adres1.Powiat = companyEuropa.CountryCode;
                //    adres1.Gmina = companyEuropa.CountryCode;
                //    adres1.StatusNip = Input.NIP;
                //    adres1.Regon = Input.NIP;
                //    adres1.Telefon = Input.Telefon1;
                //    adres1.Adres1UserID = user.Id;

                //    //Regon = companyEuropa.VatNumber;
                //    //nazwa_firmy = companyEuropa.Name;
                //    //adres = ulicaS;
                //    //Kraj1 = companyEuropa.CountryCode + " " + krajE;
                //    //Miasto1 = MiastoS.ToString();
                //    //KodPocztowy1 = KodPocztowyZagraniczny;

                //    adres2.Imie = Input.Imie;
                //    adres2.Nazwisko = Input.Nazwisko;
                //    adres2.Email = Input.Email;
                //    adres2.Miasto = MiastoS;
                //    adres2.Ulica = ulicaS;
                //    adres2.KodPocztowy = KodPocztowyZagraniczny;
                //    adres2.Telefon = Input.Telefon1;
                //    adres2.Kraj = krajE;
                //    adres2.Adres2UserID = user.Id;
                //}


                //if (companyEuropa == null && (_model != null && _model.Errors.Count() < 1))
                //{ 
                //    var accessKey1 = "384438237950f46ed363afd151757d85";
                //    var accessKey = "38dfee3f31277dcf10adcabddb33e249";
                //    var url = $"http://apilayer.net/api/validate?access_key={accessKey}&vat_number={Input.NIP}";


                //    //using (var httpClient = new HttpClient())
                //    //{
                //    //    var response = await httpClient.GetAsync(url);
                //    //    var content = await response.Content.ReadAsStringAsync();

                //    //    if (response.IsSuccessStatusCode)
                //    //    {
                //    //        dynamic resultQ = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                //    //        if (resultQ != null && resultQ.success == true)
                //    //        {
                //    //            string name = resultQ.company_name;
                //    //            string adres = resultQ.company_address;
                //    //            string vat = resultQ.vat_number;

                //    //            string SplitAdres = adres;

                //    //            string[] AdresSplit = SplitAdres.Split(",");
                //    //            string KodPocztowyMiasto = AdresSplit[0].ToString();

                //    //            string[] KodPocztowyiMiasto = KodPocztowyMiasto.Split(" ");
                //    //            string KodPocztowy = KodPocztowyiMiasto[0].ToString();

                //    //            string Miasto = "";
                //    //            for (int i = 1; i < KodPocztowyiMiasto.Length; i++)
                //    //            {
                //    //                Miasto += KodPocztowyiMiasto[i].ToString() + " ";
                //    //            }

                //    //            string UlicaNrDomu = AdresSplit[1].ToString();
                //    //            string krajE = AdresSplit[2].ToString();

                //    //            adres1.NrNieruchomosci = "";
                //    //            adres1.NrLokalu = "";
                //    //            adres1.Vat = Input.NIP;
                //    //            adres1.Miasto = Miasto;
                //    //            adres1.Ulica = UlicaNrDomu;
                //    //            adres1.KodPocztowy = KodPocztowy;
                //    //            adres1.Wojewodztwo = krajE;
                //    //            adres1.Kraj = krajE;
                //    //            adres1.Powiat = krajE;
                //    //            adres1.Gmina = krajE;
                //    //            adres1.StatusNip = Input.NIP;
                //    //            adres1.Regon = Input.NIP;
                //    //            adres1.Telefon = Input.Telefon1;
                //    //            adres1.Adres1UserID = user.Id;

                //    //            //Regon = companyEuropa.VatNumber;
                //    //            //nazwa_firmy = companyEuropa.Name;
                //    //            //adres = ulicaS;
                //    //            //Kraj1 = companyEuropa.CountryCode + " " + krajE;
                //    //            //Miasto1 = MiastoS.ToString();
                //    //            //KodPocztowy1 = KodPocztowyZagraniczny;

                //    //            adres2.Imie = Input.Imie;
                //    //            adres2.Nazwisko = Input.Nazwisko;
                //    //            adres2.Email = Input.Email;
                //    //            adres2.Miasto = Miasto;
                //    //            adres2.Ulica = UlicaNrDomu;
                //    //            adres2.KodPocztowy = KodPocztowy;
                //    //            adres2.Telefon = Input.Telefon1;
                //    //            adres2.Kraj = krajE;
                //    //            adres2.Adres2UserID = user.Id;
                //    //        }
                //    //    }
                //    //}
                //}

                adres1.NrNieruchomosci = "";
                adres1.NrLokalu = "";
                adres1.Vat = Input.NIP;
                adres1.Miasto = Input.Miasto;
                adres1.Ulica = Input.Ulica;
                adres1.KodPocztowy = Input.KodPocztowy1;
                adres1.Wojewodztwo = Input.Kraj;
                adres1.Kraj = Input.Kraj;
                adres1.Powiat = Input.Kraj;
                adres1.Gmina = Input.Kraj;
                adres1.StatusNip = Input.NIP;
                adres1.Regon = Input.NIP;
                adres1.Telefon = Input.Telefon1;
                adres1.Adres1UserID = user.Id;

                adres2.Imie = Input.Imie;
                adres2.Nazwisko = Input.Nazwisko;
                adres2.Email = Input.Email;
                adres2.Miasto = Input.Miasto;
                adres2.Ulica = Input.Ulica;
                adres2.KodPocztowy = Input.KodPocztowy1;
                adres2.Telefon = Input.Telefon1;
                adres2.Kraj = Input.Kraj;
                adres2.Adres2UserID = user.Id;

                user.Adress1rozliczeniowy = adres1;
                user.Adress2dostawy = adres2;

                //Input.NazwaFirmy = _model.Name;
                //Input.NazwaFirmy = Input.NazwaFirmy;
                user.NazwaFirmy = Input.NazwaFirmy;

                //brak sprawdzania w gus
                //if (_model != null && _model.Errors.Count() < 1)
                //{
                //    user.NazwaFirmy = _model.Name;
                //    Input.Kraj = "Polska";
                //}
                //if (companyEuropa != null)
                //{
                //    user.NazwaFirmy = companyEuropa.Name;
                //    Input.Kraj = companyEuropa.CountryCode;
                //}

                //Input.Miasto = _model.Miejscowosc;
                //Input.Ulica = _model.Ulica;
                //Input.KodPocztowy1 = _model.KodPocztowy;


                user.DataZałożenia = DateTime.Now;
                user.Nowy = true;
                user.IdProfilDzialalnosci = Input.IdProfildzialalnosci;

                user.Newsletter = true;


                user.PolitykaPrywatnosci = Input.PolitykaPrywatnosci;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("ApplicationUser created a new account with password.");
                    
                    var userId = await _userManager.GetUserIdAsync(user);

                    user.Adress1rozliczeniowy.UserID = userId;
                    user.Adress2dostawy.UserID = userId;

                    string text1 = $"Dziękujemy za rejestrację nowego konta w systemie platformy hurtowej B2B marki ALURO.<br><br>Po weryfikacji danych, otrzymają Państwo dostęp do platformy hurtowej<br>z możliwością zakupów w cenach hurtowych.<br><br>Zazwyczaj proces weryfikacji trwa od 1 do 12 godzin, <br>dziękujemy za cierpliwość.";

                    //text = text.Replace("@", "@" + System.Environment.NewLine);

                    EmailDto newClint = new EmailDto()
                    {
                    Subject = "Dziękujemy za rejestracje w Aluro",
                        To = Input.Email,
                        Body = text1,
                    };

                    await _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.


                    string text2 = $"Nowy uzytkownik zarejestrował się do platformy Aluro i czeka za weryfikacją." +
                        "<br>Pan/Pani: " + @user.Imie + " " + user.Nazwisko + "<br>" +
                        "<br>Email: " + @user.Email + "<br>" +
                    "<br>Telefon: " + adres1.Telefon + "<br>" +
                        "<br> DANE ROZLICZENIOWE FIRMY <br>" +
                        "<br>Nazwa firmy: " + @user.NazwaFirmy + "<br>" +
                    "<br>NIP: " + @adres1.Vat + "<br>" +
                    "<br>Regon: " + @adres1.Regon + "<br>" +
                    "<br>Ulica: " + @adres1.Ulica + "<br>" +
                    "<br>Miasto: " + @adres1.Miasto + "<br>" +
                    "<br>Kod pocztowy: " + @adres1.KodPocztowy + "<br>";

                    EmailDto newClintDzialTechniczny1 = new EmailDto()
                    {
                        Subject = "Nowy użytkownik, do werfikacji",
                        To = "aluro@aluro.pl",
                        Body = text2,
                    };
                    EmailDto newClintDzialTechniczny2 = new EmailDto()
                    {
                        Subject = "Nowy użytkownik, do werfikacji",
                        To = "marcin@aluro.pl",
                        Body = text2,
                    };
                    EmailDto newClintDzialTechniczny3 = new EmailDto()
                    {
                        Subject = "Nowy użytkownik, do werfikacji",
                        To = "mariusz@aluro.pl",
                        Body = text2,
                    };
                    await _emailService.SendEmailAsync(newClintDzialTechniczny1); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.
                    await _emailService.SendEmailAsync(newClintDzialTechniczny2);
                    await _emailService.SendEmailAsync(newClintDzialTechniczny3);

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Ustaw swoj e-mail",
                    //    $"Ustaw swój e-mail: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
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

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            //ViewData["profile"] = GetProfiles();

            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        private List<SelectListItem> GetProfiles()
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
