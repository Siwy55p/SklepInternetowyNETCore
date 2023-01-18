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

        private readonly IStringLocalizer<HomeController> _localizer;

        private readonly ISetting _setting;

        //Kontrolery odzoruwuja widoki , sluza do generowania róznych treści

        //        uzytkownik czy jest aktywny
        //Details
        //Tworzenie Excela z produktow

        //flaga


        public HomeController(ISliderService sliderService, IStringLocalizer<HomeController> localizer,ApplicationDbContext context,ILogger<HomeController> logger, RegonService regonService, ISetting setting)
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol
                                        | SecurityProtocolTypeExtensions.Tls12;
            _logger = logger;
            _context = context;
            RegonService = regonService;
            _sliderService = sliderService;
            _localizer = localizer;

            _setting = setting;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var nowosci = await _context.Products.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(9).ToListAsync();

            //var bestseller = await _context.Products.Where(a => !a.Ukryty).OrderBy(a => Guid.NewGuid()).Take(3).ToListAsync();
            ////Category category = new Category { Name = "Kategoria1", Description = "Opis kategoria", NazwaPlikuIkony = "ikona.png" };
            ////_context.Add(category);
            ////_context.SaveChanges();


            //if(Core.Constants.SliderHome1 == null || Core.Constants.SliderHome1 == 0)
            //{
            //    Core.Constants.SliderHome1 = 9;
            //}
            //if (Core.Constants.SliderHome2 == null || Core.Constants.SliderHome2 == 0)
            //{
            //    Core.Constants.SliderHome2 = 10;
            //}
            //if (Core.Constants.SliderHome3 == null || Core.Constants.SliderHome3 == 0)
            //{
            //    Core.Constants.SliderHome3 = 11;
            //}
            //try
            //{

            //try
            //{
            //    TerytWs1.CzyZalogowanyRequest request = new TerytWs1.CzyZalogowanyRequest();
            //    var proxy = new ChannelFactory<TerytWs1.ITerytWs1>("custom");
            //    proxy.Credentials.UserName.UserName = "szuminski.p";
            //    proxy.Credentials.UserName.Password = "sgj1EpTwz";
            //    var result = proxy.CreateChannel();
            //    var test = result.CzyZalogowany(request);
            //}
            //catch (Exception ex) { }


            int SliderHome1= _setting.GetSliderHome1(1);
                int SliderHome2 = _setting.GetSliderHome2(1);
                int SliderHome3 = _setting.GetSliderHome3(1);

                Slider sliderHomes1 = await _sliderService.GetAsync(SliderHome1); //Id slider
                Slider sliderHomes2 = await _sliderService.GetAsync(SliderHome2); //Id slider
                Slider sliderHomes3 = await _sliderService.GetAsync(SliderHome3); //Id slider

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