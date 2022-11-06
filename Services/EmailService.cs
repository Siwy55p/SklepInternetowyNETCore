using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using MailKit.Net.Smtp;

namespace partner_aluro.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 465, SecureSocketOptions.StartTls);  //smtp.gmail.com
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Dispose();  //To moze jest nie potrzebne
            smtp.Disconnect(true);

        }
    }
}
