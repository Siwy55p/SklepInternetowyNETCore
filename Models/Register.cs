using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class Register
    {
        [Required(ErrorMessage ="Wprowadz nazwę użytkownika")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
