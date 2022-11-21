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

namespace partner_aluro.Controllers
{
    [Authorize]
    [DefaultBreadcrumb("Home")]
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.User}")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        private readonly  RegonService RegonService;

        private readonly ISliderService _sliderService;

        private readonly IStringLocalizer<HomeController> _localizer;

        //Kontrolery odzoruwuja widoki , sluza do generowania róznych treści
        public HomeController(ISliderService sliderService, IStringLocalizer<HomeController> localizer,ApplicationDbContext context,ILogger<HomeController> logger, RegonService regonService)
        {
            _logger = logger;
            _context = context;
            RegonService = regonService;
            _sliderService = sliderService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //logika zalogowania
            if (User.Identity.IsAuthenticated)
            {
                var nowosci = await _context.Products.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(9).ToListAsync();

                var bestseller = await _context.Products.Where(a => !a.Ukryty).OrderBy(a => Guid.NewGuid()).Take(3).ToListAsync();
                //Category category = new Category { Name = "Kategoria1", Description = "Opis kategoria", NazwaPlikuIkony = "ikona.png" };
                //_context.Add(category);
                //_context.SaveChanges();


                Slider sliderHomes = await _sliderService.GetAsync(9); //Id slider

                //zainicjuj view model
                var vm = new HomeViewModel() { Nowosci = nowosci, Bestsellery = bestseller, SliderHome1 = sliderHomes };

                return View(vm); //zapewnia renderowania widoków 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [Route("Ogolne-warunki-sprzedazy")]
        [HttpGet]
        public IActionResult WarunkiSprzedazy()
        {
            return View();
        }

        [Route("Platnosc-i-dostawa")]
        [HttpGet]
        public IActionResult PlatnoscDostawa()
        {
            return View();
        }
        [Route("polityka-prywatnosci")]
        public IActionResult Privacy()
        {
            return View();
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