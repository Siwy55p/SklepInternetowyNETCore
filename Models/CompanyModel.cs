using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string Name { get; set; }

        [Display(Name = "Adres firmy")]
        public string Address { get; set; }

        [Display(Name = "Ulica")]
        public string Ulica { get; set; }

        [Display(Name = "Nr Nieruchomości")]
        public string NrNieruchomosci { get; set; }

        [Display(Name = "Nr Lokalu")]
        public string NrLokalu { get; set; }

        [Display(Name = "Kod Pocztowy")]
        public string KodPocztowy { get; set; }

        [Display(Name = "Miejscowosc")]
        public string Miejscowosc { get; set; }

        [Display(Name = "Nr Nip")]
        public string Vat { get; set; }

        [Display(Name = "Wojewodztwo")]
        public string Wojewodztwo { get; set; }

        [Display(Name = "Powiat")]
        public string Powiat { get; set; }

        [Display(Name = "Gmina")]
        public string Gmina { get; set; }

        [Display(Name = "StatusNip")]
        public string StatusNip { get; set; }

        [Display(Name = "DataZakonczeniaDzialalnosci")]
        public string DataZakonczeniaDzialalnosci { get; set; }

        [Display(Name = "Nr Regon")]
        public string Regon { get; set; }

        public List<ErrorModel> Errors { get; set; } = new();


    }
}
