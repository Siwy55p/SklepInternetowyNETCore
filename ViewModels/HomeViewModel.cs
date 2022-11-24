using partner_aluro.Data;
using partner_aluro.Enums;
using partner_aluro.Models;

namespace partner_aluro.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category>? Kategorie { get; set; }
        public IEnumerable<Product>? Nowosci { get; set; }
        public IEnumerable<Product>? Bestsellery { get; set; }


        public Slider? SliderHome1 { get; set; }
        public Slider? SliderHome2 { get; set; }
        public Slider? SliderHome3 { get; set; }

        //public CompanyModel _model {get; set;}

    }
}
