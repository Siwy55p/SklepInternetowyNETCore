using partner_aluro.Models;
using partner_aluro;

namespace partner_aluro.Data
{
    public class RegonService
    {
        IBIRSearchService _birSearchService;

        public RegonService(IBIRSearchService birSearchService)
        {
            _birSearchService = birSearchService;
        }

        public async Task<CompanyModel> GetCompanyDataByNipAsync(string vatId)
        {
            var search = await _birSearchService.GetCompanyDataByNipIdAsync(vatId);

            var separator = string.IsNullOrEmpty(search.NrLokalu) ? string.Empty : "/";

            CompanyModel model = new CompanyModel
            {
                Name = search.Nazwa,
                Vat = search.Nip,
                Address = $"{search.Ulica} {search.NrNieruchomosci}{separator}{search.NrLokalu} {search.KodPocztowy} {search.Miejscowosc}",
                Regon = search.Regon,
                Errors = search.Errors,
            };
            return model;
        }

        public async Task<CompanyModel> GetCompanyDataByRegonAsync(string regonId)
        {
            var search = await _birSearchService.GetCompanyDataByRegonAsync(regonId);

            var separator = string.IsNullOrEmpty(search.NrLokalu) ? string.Empty : "/";

            CompanyModel model = new CompanyModel
            {
                Name = search.Nazwa,
                Vat = search.Nip,
                Address = $"{search.Ulica} {search.NrNieruchomosci}{separator}{search.NrLokalu} {search.KodPocztowy} {search.Miejscowosc}",
                Regon = search.Regon,
                Errors = search.Errors,
            };
            return model;
        }
    }
}
