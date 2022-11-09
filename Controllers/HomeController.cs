﻿using Microsoft.AspNetCore.Authorization;
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

        //Kontrolery odzoruwuja widoki , sluza do generowania róznych treści
        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger, RegonService regonService)
        {
            _logger = logger;
            _context = context;
            RegonService = regonService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            //logika zalogowania
            if (User.Identity.IsAuthenticated)
            {
                ////Potrzebne do REGON
                //CompanyModel _model = new CompanyModel();

                //zawsze trzeba pobrac dane i wrzucic do widoku
                //var kategorie = _context.Category.ToList();

                //pobieramu produkty
                var nowosci = _context.Products.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();

                var bestseller = _context.Products.Where(a => !a.Ukryty).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
                //Category category = new Category { Name = "Kategoria1", Description = "Opis kategoria", NazwaPlikuIkony = "ikona.png" };
                //_context.Add(category);
                //_context.SaveChanges();

                //zainicjuj view model
                var vm = new HomeViewModel() { Nowosci = nowosci, Bestsellery = bestseller};

                return View(vm); //zapewnia renderowania widoków 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1)});

            return Redirect(Request.Headers["Referer"].ToString());
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

        [Route("testowy-route/{name}")]
        public IActionResult Product(string name)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel vm)
        {
            //zawsze trzeba pobrac dane i wrzucic do widoku
            var kategorie = _context.Category.ToList();
            var nowosci = _context.Products.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();
            var bestseller = _context.Products.Where(a => !a.Ukryty).OrderBy(a => Guid.NewGuid()).Take(3).ToList();

            var vm2 = new HomeViewModel() { Kategorie = kategorie, Nowosci = nowosci, Bestsellery = bestseller};

            return View(vm2);
        }

    }
}