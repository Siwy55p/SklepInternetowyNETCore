// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace partner_aluro.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    
                    return RedirectToPage("./ForgotPasswordUserDontExist");
                }
                //if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed

                //    return RedirectToPage("./ForgotPasswordUserDontExist");
                //}

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
                    To = Input.Email,
                    Body = $"<table style=\"background-color: #ececec; font-family: Arial, sans-serif; width: 100%;\">\r\n<tbody>\r\n<tr>\r\n<td>\r\n<table class=\"newsletter-pro-content\" style=\"margin: 1% auto; width: 646px; background-color: rgb(255, 255, 255); height: 549.25px;\" align=\"center\">\r\n<tbody>\r\n<tr style=\"height: 549.25px;\">\r\n<td style=\"height: 549.25px;\">\r\n<table style=\"border-collapse: collapse; width: 100%; height: 402.719px;\" border=\"0\">\r\n<tbody>\r\n<tr style=\"height: 181.391px;\">\r\n<td style=\"text-align: center; height: 181.391px;\">\r\n<p><span style=\"font-family: Arial, Helvetica, sans-serif;\"><img src=\"http://www.partner.aluro.pl/img/cms/log-png.png\" alt=\"\" width=\"358\" height=\"211\"></span></p>\r\n</td>\r\n</tr>\r\n<tr style=\"height: 159.953px;\">\r\n<td style=\"text-align: left; height: 159.953px;\" align=\"center\">\r\n<p style=\"text-align: center;\"></p>\r\n<p style=\"text-align: center;\"><span style=\"font-family: Arial, Helvetica, sans-serif;\">Proszę zresetuj swoje hasło: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> klikacjąc tutaj.</a></p>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>"
                };

                _emailService.SendEmailAsync(newClint); //Bardzo specjalnie tak jest jak jest zrobione. Musi tak zostać.

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
