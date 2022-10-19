using partner_aluro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace partner_aluro.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<SelectListItem> Roles { get; set; }
    }
}
