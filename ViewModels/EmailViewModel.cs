using partner_aluro.Models;
using System.Reflection.Metadata;
using partner_aluro.Core;

namespace partner_aluro.ViewModels
{
    public class EmailViewModel
    {
        public EmailDto Register = new EmailDto()
        {
            Body = Constants.RegisterNewAccoutMessageEmail,
            Subject = Constants.RegisterNewAccoutMessageEmailSubject
        };

        public EmailDto? Active { get; set; }

        public EmailDto? Send { get; set; }

    }
}
