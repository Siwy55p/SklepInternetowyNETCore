using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Core;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using SmartBreadcrumbs.Attributes;
using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ServiceReference2;
using NuGet.Protocol.Plugins;
using System.ServiceModel;
using System.Net;

namespace partner_aluro.Controllers
{
    [Authorize]
    [DefaultBreadcrumb("Home")]
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.Klient}")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        private readonly  RegonService RegonService;

        private readonly ISliderService _sliderService;

        //private readonly IStringLocalizer<HomeController> _localizer;

        private readonly ISetting _setting;

        //Kontrolery odzoruwuja widoki , sluza do generowania róznych treści

        //        uzytkownik czy jest aktywny
        //Details
        //Tworzenie Excela z produktow

        //flaga


        public HomeController(ISliderService sliderService,ApplicationDbContext context,ILogger<HomeController> logger, RegonService regonService, ISetting setting)
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol
                                        | SecurityProtocolTypeExtensions.Tls12;
            _logger = logger;
            _context = context;
            RegonService = regonService;
            _sliderService = sliderService;

            _setting = setting;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int Setting = 1;

            Slider sliderHomes1 = await _sliderService.GetAsync(_setting.GetSliderHome1(Setting)); //Id slider
            Slider sliderHomes2 = await _sliderService.GetAsync(_setting.GetSliderHome2(Setting)); //Id slider
            Slider sliderHomes3 = await _sliderService.GetAsync(_setting.GetSliderHome3(Setting)); //Id slider

            //zainicjuj view model
            var vm = new HomeViewModel() { SliderHome1 = sliderHomes1, SliderHome2 = sliderHomes2, SliderHome3 = sliderHomes3 };

            return View(vm); //zapewnia renderowania widoków 

        }

        [Route("Ogolne-warunki-sprzedazy")]
        [HttpGet]
        public IActionResult WarunkiSprzedazy()
        {
            Setting setting = _context.Setting.FirstOrDefault();
            return View(setting);
        }

        [Route("Platnosc-i-dostawa")]
        [HttpGet]
        public IActionResult PlatnoscDostawa()
        {
            Setting setting = _context.Setting.FirstOrDefault();
            return View(setting);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Redirect()
        {
            return RedirectToAction("Privacy");
        }

        public IActionResult IntegracjaXML()
        {
            return View();
        }

        [Route("testowy-route/{name}")]
        public IActionResult Product(string name)
        {
            return View();
        }


    }
}