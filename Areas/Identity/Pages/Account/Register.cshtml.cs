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

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IProfildzialalnosciService profildzialalnosciService,
            RegonService regonService
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
            [Required(ErrorMessage ="Profil działalności jest wymagany")]
            [Display(Name = "Wybierz profil działalności.")]
            public int IdProfildzialalnosci { get; set; }

            [Required(ErrorMessage = "Imię jest wymagane")]
            [StringLength(255, ErrorMessage ="Twoje imie może mieć maksywalnie 255 znaków.")]
            [Display(Name ="Imię")]
            public string Imie { get; set; }

            [Required(ErrorMessage = "Twoje Nazwisko jest wymagane")]
            [StringLength(255, ErrorMessage = "Twoje Nazwisko może mieć maksywalnie 255 znaków.")]
            [Display(Name = "Nazwisko")]
            public string Nazwisko { get; set; }

            [Display(Name = "Firma")]
            public string NazwaFirmy { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Ulica musi zostać podana.")]
            [Display(Name = "Adres")]
            public string Ulica { get; set; }

            [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
            [StringLength(7)]
            [Display(Name = "Kod Pocztowy")]
            public string KodPocztowy1 { get; set; }
            //public Adress1rozliczeniowy Adress { get; set; }

            [Required(ErrorMessage = "Miejscowość jest wymagana.")]
            [StringLength(255, ErrorMessage = "Miejscowość jest wymagana.")]
            [Display(Name = "Miasto")]
            public string Miasto { get; set; }


            [StringLength(255, ErrorMessage = "Podaj Kraj")]
            [Display(Name = "Kraj")]
            public string Kraj { get; set; }



            [Required(ErrorMessage = "Telefon do kontaktu jest wymagany.")]
            [Display(Name = "Telefon")]
            public string Telefon1 { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Twój adres e-mail jest wymagany.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Pole hasło jest wymagane.")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Powtórz hasło")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [StringLength(10, ErrorMessage = "Proszę wprowadzic poprawy nr NIP", MinimumLength = 10)]
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
            }

            Input.NazwaFirmy = _model.Name;
            Input.Miasto = _model.Miejscowosc;
            Input.Ulica = _model.Ulica;
            Input.KodPocztowy1 = _model.KodPocztowy;

            ModelState.Remove("Input.KodPocztowy1");
            returnUrl ??= Url.Content("~/");
            ModelState.Remove("Input.Miasto");
            ModelState.Remove("Input.Ulica");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Imie = Input.Imie;
                user.Nazwisko = Input.Nazwisko;

                Adress1rozliczeniowy adres1 = new Adress1rozliczeniowy();
                Adress2dostawy adres2 = new Adress2dostawy();
                user.Adres1 = adres1;
                user.Adres2 = adres2;

                Input.NazwaFirmy = _model.Name;
                user.NazwaFirmy = _model.Name;
                Input.Kraj = "Polska";
                Input.Miasto = _model.Miejscowosc;
                Input.Ulica = _model.Ulica;
                Input.KodPocztowy1 = _model.KodPocztowy;
                user.Adres1.NrNieruchomosci = _model.NrNieruchomosci;
                user.Adres1.NrLokalu = _model.NrLokalu;
                user.Adres1.Vat = _model.Vat;
                user.Adres1.Miasto = _model.Miejscowosc;
                user.Adres1.Ulica = _model.Ulica;
                user.Adres1.KodPocztowy = _model.KodPocztowy;
                user.Adres1.Wojewodztwo = _model.Wojewodztwo;
                user.Adres1.Kraj = "Polska";
                user.Adres1.Powiat = _model.Powiat;
                user.Adres1.Gmina = _model.Gmina;
                user.Adres1.StatusNip = _model.StatusNip;
                user.Adres1.DataZakonczeniaDzialalnosci = _model.DataZakonczeniaDzialalnosci;
                user.Adres1.Regon = _model.Regon;

                user.Adres1.Telefon = Input.Telefon1;
                user.Adres1.Kraj = Input.Kraj;

                user.Adres2.Miasto = user.Adres1.Miasto;
                user.Adres2.Ulica = user.Adres1.Ulica;
                user.Adres2.KodPocztowy = user.Adres1.KodPocztowy;
                user.Adres2.Telefon = user.Adres1.Telefon;
                user.Adres2.Kraj = user.Adres1.Kraj;


                user.DataZałożenia = DateTime.Now;
                user.IdProfilDzialalnosci = Input.IdProfildzialalnosci;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("ApplicationUser created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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
