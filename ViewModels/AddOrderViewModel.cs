using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Models;

namespace partner_aluro.ViewModels
{
    public class AddOrderViewModel
    {
        public Order order { get; set; }

        public string SelectUser { get; set; }
        public List<SelectListItem> ListaUzytkownik { get; set; }
        public List<Product> ListaProduktow { get; set; }

    }
}


