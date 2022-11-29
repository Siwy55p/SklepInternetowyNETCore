using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace partner_aluro.Models
{
    public class SMS
    {
        [Key]
        public int SMSId { get; set; }
        public string apiKey { get; set; }
        public string numbers { get; set; }

        public string message { get; set; }
        public string sender { get; set; }


    }
}
