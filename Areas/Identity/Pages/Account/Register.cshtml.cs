// Licensed to the .NET Foundation under one or more agreements.
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



            [Phone]
            [Required(ErrorMessage = "Telefon do kontaktu jest wymagany.")]
            [Display(Name = "Telefon:")]
            [DataType(DataType.PhoneNumber)]
            //[StringLength(10, ErrorMessage = "{0} Numer musi mieć zawierać liczby pomiędzy {2}, a {1} znaków.", MinimumLength = 6)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3,4})$", ErrorMessage = "Nie prawidłowy nr telefonu (komórkowy)")]
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
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
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
            [StringLength(10, ErrorMessage = "Proszę wprowadzic poprawy nr NIP składający się z 10-ciu znaków.", MinimumLength = 10)]
            [RegularExpression("^([0-9]{10})$", ErrorMessage = "Nieprawidłowy nr NIP.")]
            public string NIP { get; set; }
        }

        [HttpGet]
        public async Task OnGetAsync(string pass, string email, string returnUrl = null)
        {
            //TUTAJ
            ViewData["Profile"] = GetProfiles();
            ViewData["email"] = email;
            ViewData["pass"] = pass;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        }



        [HttpPost]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ViewData["Profile"] = GetProfiles();

            _model.Vat = Input.NIP;
            _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);


            if (_model.Errors.Count > 0)
            {
                komunikat = _model.Errors[0].ErrorMessagePl;
                return Page();
            }

            Input.NazwaFirmy = _model.Name;
            Input.Miasto = _model.Miejscowosc;
            Input.Ulica = _model.Ulica;
            Input.KodPocztowy1 = _model.KodPocztowy;

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
                adres1.NrNieruchomosci = _model.NrNieruchomosci;
                adres1.NrLokalu = _model.NrLokalu;
                adres1.Vat = _model.Vat;
                adres1.Miasto = _model.Miejscowosc;
                adres1.Ulica = _model.Ulica;
                adres1.KodPocztowy = _model.KodPocztowy;
                adres1.Wojewodztwo = _model.Wojewodztwo;
                adres1.Kraj = "Polska";
                adres1.Powiat = _model.Powiat;
                adres1.Gmina = _model.Gmina;
                adres1.StatusNip = _model.StatusNip;
                adres1.Regon = _model.Regon;
                adres1.Telefon = Input.Telefon1;
                adres1.Adres1UserID = user.Id;

                Adress2dostawy adres2 = new Adress2dostawy();
                adres2.Imie = Input.Imie;
                adres2.Nazwisko = Input.Nazwisko;
                adres2.Email = Input.Email;
                adres2.Miasto = _model.Miejscowosc;
                adres2.Ulica = _model.Ulica;
                adres2.KodPocztowy = _model.KodPocztowy;
                adres2.Telefon = Input.Telefon1;
                adres2.Kraj = "Polska";
                adres2.Adres2UserID = user.Id;

                user.Adress1rozliczeniowy = adres1;
                user.Adress2dostawy = adres2;

                Input.NazwaFirmy = _model.Name;
                user.NazwaFirmy = _model.Name;
                Input.Kraj = "Polska";
                Input.Miasto = _model.Miejscowosc;
                Input.Ulica = _model.Ulica;
                Input.KodPocztowy1 = _model.KodPocztowy;


                user.DataZałożenia = DateTime.Now;
                user.IdProfilDzialalnosci = Input.IdProfildzialalnosci;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("ApplicationUser created a new account with password.");
                    
                    var userId = await _userManager.GetUserIdAsync(user);

                    user.Adress1rozliczeniowy.UserID = userId;
                    user.Adress2dostawy.UserID = userId;

                    string text = $"Dziękujemy za rejestrację nowego konta w systemie platformy hurtowej B2B marki ALURO.<br><br>Po weryfikacji danych, otrzymają Państwo dostęp do platformy hurtowej<br>z możliwością zakupów w cenach hurtowych.<br><br>Zazwyczaj proces weryfikacji trwa od 1 do 12 godzin, <br>dziękujemy za cierpliwość.";

                    //text = text.Replace("@", "@" + System.Environment.NewLine);

                    EmailDto newClint = new EmailDto()
                    {
                    Subject = "Dziękujemy za rejestracje w Aluro",
                        To = Input.Email,
                        Body = text,
                    };

                    _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Ustaw swoj e-mail",
                        $"Ustaw swój e-mail: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj</a>.");

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
