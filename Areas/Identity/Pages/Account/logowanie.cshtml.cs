// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using partner_aluro.Models;
using System.Security.Claims;
using partner_aluro.Services.Interfaces;
using Microsoft.Extensions.Localization;
using System.Reflection;
using partner_aluro.Resources;

namespace partner_aluro.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IProfildzialalnosciService _profildzialalnosciService;

        private readonly IStringLocalizer _identityLocalizer;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IProfildzialalnosciService profildzialalnosciService , IStringLocalizerFactory factory)
        {
            _signInManager = signInManager;
            _logger = logger;

            _profildzialalnosciService = profildzialalnosciService;

            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _identityLocalizer = factory.Create("ShareResource", assemblyName.Name);

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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        public string email2 { get; set; }

        public string pass2 { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }


            [Display(Name = "Email")]
            [EmailAddress]
            public string Email2 { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            public string Password2 { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");


            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                var user = await _signInManager.UserManager.FindByNameAsync(Input.Email);
                if(user == null)
                {
                    ModelState.TryAddModelError(string.Empty, "Nieprawidłowy e-mail");
                    return Page();
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
                if(result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("arm","pwd"),
                        //new Claim("EmployeeNumber", "1")
                    };

                    var roles = await _signInManager.UserManager.GetRolesAsync(user);

                    if(roles.Any())
                    {
                        //Manager
                        var roleClaim = string.Join(",", roles);
                        claims.Add(new Claim("Roles", roleClaim));
                    }


                    await _signInManager.SignInWithClaimsAsync(user, Input.RememberMe, claims );

                    Core.Constants.UserId = user.Id; //Pobierz uzytkownika
                    Core.Constants.Rabat = _profildzialalnosciService.GetRabat(Core.Constants.UserId);

                    _logger.LogInformation("ApplicationUser zalogował się");
                    return Redirect("/Home/Index");
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {

                    _logger.LogWarning("Konto zablokowane.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _identityLocalizer["Nieprawidłowe hasło."]);
                    //ModelState.AddModelError(string.Empty, _identityLocalizer["INVALID_LOGIN_ATTEMPT"]);
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
