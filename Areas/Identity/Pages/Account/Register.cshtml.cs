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

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IProfildzialalnosciService profildzialalnosciService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _profildzialalnosciService = profildzialalnosciService;
        }

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
            [Required]
            [Display(Name = "Profil Dzialalnosci")]
            public int IdProfildzialalnosci { get; set; }

            [Required]
            [StringLength(255, ErrorMessage ="Twoje imie może mieć maksywalnie 255 znaków.")]
            [Display(Name ="Imie")]
            public string Imie { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Twoje Nazwisko może mieć maksywalnie 255 znaków.")]
            [Display(Name = "Nazwisko")]
            public string Nazwisko { get; set; }

            [Display(Name = "Firma")]
            public string NazwaFirmy { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Twoja ulica musi być podana.")]
            [Display(Name = "Adres")]
            public string Ulica { get; set; }

            [Required(ErrorMessage = "Wprowadz Kod Pocztowy")]
            [StringLength(7)]
            [Display(Name = "Kod Pocztowy")]
            public string KodPocztowy1 { get; set; }
            //public Adress1rozliczeniowy Adress { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Twoja miejscowosc musi być podana.")]
            [Display(Name = "Miasto")]
            public string Miasto { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Podaj Kraj")]
            [Display(Name = "Kraj")]
            public string Kraj { get; set; }



            [Display(Name = "Telefon")]
            public string Telefon1 { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            //TUTAJ
            ViewData["Profile"] = GetProfiles();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Imie = Input.Imie;
                user.Nazwisko = Input.Nazwisko;

                user.NazwaFirmy = Input.NazwaFirmy;

                Adress1rozliczeniowy adres1 = new Adress1rozliczeniowy();
                Adress2dostawy adres2 = new Adress2dostawy();
                user.Adres1 = adres1;
                user.Adres2 = adres2;

                user.Adres1.Miasto = Input.Miasto;
                user.Adres1.Ulica = Input.Ulica;
                user.Adres1.KodPocztowy = Input.KodPocztowy1;
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
