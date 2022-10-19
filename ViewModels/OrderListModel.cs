using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Models;

namespace partner_aluro.ViewModels
{
    public class OrderListModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<Order> Orders { get; set; }

    }
}


