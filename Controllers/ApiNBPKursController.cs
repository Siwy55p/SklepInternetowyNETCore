using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ApiNBPKursController : Controller
    {
        private readonly IApiServiceNBPKurs _apiServiceNBPKurs;
        public ApiNBPKursController(IApiServiceNBPKurs apiServiceNBPKurs)
        {
            _apiServiceNBPKurs = apiServiceNBPKurs;   
        }

        public IActionResult Index()
        {
            var response = _apiServiceNBPKurs.Get("EUR");
            return View();
        }
    }
}
