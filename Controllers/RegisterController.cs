using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RegonService RegonService;

        public RegisterController(RegonService regonService)
        {
            RegonService = regonService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<List<string>> SprawdzNIPAsync(string Vat = null)
        {

            CompanyModel _model = new CompanyModel();

            //ViewData["Profile"] = GetProfiles();
            var a = 0;
            _model.Vat = Vat;
            _model = await RegonService.GetCompanyDataByNipAsync(_model.Vat);

            var komunikat = "Brak danych";
            var nazwa_firmy = "Nie znaleziona takiej firmy";
            var adres = "";
            var miasto = "";
            var kod_pocztowy = "";

            if (_model.Errors.Count > 0)
            {
                komunikat = _model.Errors[0].ErrorMessagePl;

            }
            else
            {
                nazwa_firmy = _model.Name;
                adres = _model.Address;
                miasto = _model.Miejscowosc;
                kod_pocztowy = _model.KodPocztowy;
            }

            List<string> list = new List<string>();

            list.Add(komunikat);
            list.Add(nazwa_firmy);
            list.Add(adres);
            list.Add(miasto);
            list.Add(kod_pocztowy);


            return list;
        }
    }
}
