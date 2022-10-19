using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        private readonly IApiService _apiService;
        public ApiController(IApiService apiService)
        {
            _apiService = apiService;   
        }

        public IActionResult Index()
        {
            var response = _apiService.Get("London");
            return View(response);
        }
    }
}
