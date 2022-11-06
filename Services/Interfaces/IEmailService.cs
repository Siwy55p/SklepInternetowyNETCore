using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request);
    }
}
