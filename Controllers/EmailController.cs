using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using MimeKit.Text;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Reflection.Metadata;
using partner_aluro.Core;
using partner_aluro.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmailFromConf(EmailViewModel sends)
        {
            if (sends is null)
            {
                throw new ArgumentNullException(nameof(sends));
            }

            _emailService.SendEmailAsync(sends.Send);
            return View();
        }

        [HttpPost]
        public IActionResult SaveChangeMessageToRegister(EmailViewModel MessageToRegister)
        {
            Constants.RegisterNewAccoutMessageEmailSubject = MessageToRegister.Register.Subject;
            Constants.RegisterNewAccoutMessageEmail = MessageToRegister.Register.Body;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Index()
        {

            EmailViewModel emailViewModel = new EmailViewModel();

            return View(emailViewModel);
        }

    }
}
