using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Core;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using SmartBreadcrumbs.Attributes;
using System;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

using ServiceReference1;
using partner_aluro.Enums;
using System.Runtime.CompilerServices;
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

        private readonly IStringLocalizer<HomeController> _localizer;

        //Kontrolery odzoruwuja widoki , sluza do generowania róznych treści
        public HomeController(IStringLocalizer<HomeController> localizer,ApplicationDbContext context,ILogger<HomeController> logger, RegonService regonService)
        {
            _logger = logger;
            _context = context;
            RegonService = regonService;

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

                Slider slider = new Slider();

                ImageModel img1 = new ImageModel()
                {
                    ImageName = "slider1",
                    kolejnosc = 1,
                    path = $"../images/sliderhome/slider1.jpg",

                };
                ImageModel img2 = new ImageModel()
                {
                    ImageName = "slider2",
                    kolejnosc = 2,
                    path = $"../images/sliderhome/slider2.jpg",

                };
                ImageModel img3 = new ImageModel()
                {
                    ImageName = "slider3",
                    kolejnosc = 3,
                    path = "..\\images\\SliderHome\\slider3.jpg",

                };
                slider.ObrazkiDostepneWSliderze.Add(img1);
                slider.ObrazkiDostepneWSliderze.Add(img2);
                slider.ObrazkiDostepneWSliderze.Add(img3);

                List<Slider> sliders = new List<Slider>();
                Slider slider2 = new Slider();
                slider2.ObrazkiDostepneWSliderze.Add(img1);
                slider2.ObrazkiDostepneWSliderze.Add(img2);
                slider2.ObrazkiDostepneWSliderze.Add(img3);
                slider2.Name = "SliderHome";


                Slider sliderHomes = _context.Sliders.Where(x => x.ImageSliderID == 6).Include(x=>x.ObrazkiDostepneWSliderze).FirstOrDefault();

                sliders.Add(slider);
                sliders.Add(slider2);
                //zainicjuj view model
                var vm = new HomeViewModel() { Nowosci = nowosci, Bestsellery = bestseller, SliderHome = sliders ,SliderHome1 = sliderHomes };

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