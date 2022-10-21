﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Core;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using SmartBreadcrumbs.Nodes;

using X.PagedList;
using X.PagedList.Mvc.Core;

namespace partner_aluro.Controllers
{

    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.User}")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkCategory _iUnitOfWorkCategory;

        private readonly ApplicationDbContext _context;

        private readonly Cart _cart;

        public CategoryController(Cart cart, ICategoryService categoryDB, IUnitOfWorkCategory iUnitOfWorkCategory, ApplicationDbContext context)
        {
            _cart = cart;
            _categoryService = categoryDB;
            _iUnitOfWorkCategory = iUnitOfWorkCategory;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            //logika zapisania kategorii do bazy.
            var id = _categoryService.AddSave(category);

            return RedirectToAction("List");

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kategoria = _categoryService.Get(id);
            return View(kategoria);
        }

        [HttpPost]
        public IActionResult Edit(Category data)
        {
            var kategoria = _iUnitOfWorkCategory.Category.Get(data.CategoryId);

            if (kategoria == null)
            {
                return NotFound();
            }

            kategoria.Name = data.Name;
            kategoria.Description = data.Description;

            _iUnitOfWorkCategory.Category.Update(kategoria);

            return RedirectToAction("List");
        }


        [HttpGet]        
        public IActionResult Details(int id)
        {
            var category = _categoryService.Get(id);
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _context.Category.ToListAsync());
        }

        //wyswietlane produkty na bierzaca nie zalezne ktora metoda jest uzyta


        //TUTAJ WYSWIETLAM STRONE PODSTAWOWĄ DLA WYSWIETLENIA PRODUKTOW Z ID KATEGORIA szukanaNazwa
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista(int? page, string? szukanaNazwa, int? Sort) //Link do wyswietlania po wyborze kategorii
        {
            var products = _cart.GetAllCartItems();

            var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            ViewData["BreadcrumbNode"] = categoryPage;
            ViewData["Title"] = szukanaNazwa;
            ViewData["szukanaNazwa"] = szukanaNazwa;

            ViewData["Sort"] = Sort;

            //jesli jest cos w karcie przekaz do zmiennej, pokaz wartosc karty true
            if (products.Count > 0)
            {
                ViewData["Pokaz"] = "show";
            }

            if (szukanaNazwa == null || szukanaNazwa == "")
            {
                szukanaNazwa = "";
                List<Product> produkty = await _context.Products.ToListAsync();
                var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
                var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

                if (Sort == 1)
                {
                    onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                }
                if (Sort == 2)
                {
                    onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                }
                if (Sort == 3)
                {
                    onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                }
                if (Sort == 4)
                {
                    onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                }


                ViewBag.OnePageOfProducts = onePageOfProducts;

                return View();
            }
            else
            {

                List<Product> produkty2 = await _context.Products.Where(x => x.CategoryNavigation.Name == szukanaNazwa).ToListAsync();
                

                if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
                {
                    List<Product> produkty = (List<Product>)await szukanie(szukanaNazwa);

                    var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
                    
                    foreach (var produkt in produkty2)
                    {
                        produkty.Add(produkt);
                    }

                    var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

                    if (Sort == 1)
                    {
                        onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    }
                    if (Sort == 2)
                    {
                        onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    }
                    if (Sort == 3)
                    {
                        onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    }
                    if (Sort == 4)
                    {
                        onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    }
                    
                    ViewBag.OnePageOfProducts = onePageOfProducts;

                    return View(produkty);
                }
                else
                {
                    return View(produkty2);
                }


            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista2(int? page, string? szukanaNazwa, int? Sort) //Link do wyswietlania po wyborze kategorii TO JEST TYLKO KONTENER
        {
            List <Product> produkty = await szukanie(szukanaNazwa);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

            if (Sort == 1)
            {
                onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }
            if (Sort == 2)
            {
                onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }
            if (Sort == 3)
            {
                onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }
            if (Sort == 4)
            {
                onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }

            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<List<Product>>? szukanie(string szukanaNazwa)   // to jest wynik wyszukiwarki (zwraca produkty ktore zawieraja w nazwie szukana nazwe)
        {
            List<Product> WszystkieProdukty = await _context.Products.ToListAsync();

            //List<string> WyszukaneNazwyProdukow = await _context.Products.Select(p=> p.Name.ToLower()).ToListAsync(); // tworze liste nazw (puste)

            List<string> WyszukaneNazwyProdukow = await _context.Products.Select(p => p.Name.ToLower()).ToListAsync(); // tworze liste nazw (puste)

            List<string> WyszukaneSymbolowProdukow = await _context.Products.Select(p => p.Symbol.ToLower()).ToListAsync(); // tworze liste nazw Symbolów wszystkie tylko symbole

            
            WyszukaneNazwyProdukow.AddRange(WyszukaneSymbolowProdukow);

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                szukanaNazwa = szukanaNazwa.ToLower();

                //WyszukaneNazwyProdukow zawiera wszystkie nazwy wszystkich produków
                // oraz produkty ktore zawieraja ten symbol



                List<Product> WyszukaneProduktyLowerName = new List<Product>();

                foreach (string NazwaProduktu in WyszukaneNazwyProdukow)
                {
                    if (NazwaProduktu.Contains(szukanaNazwa)) // jesli jakas nazwa jest w liscie 
                    {
                        WyszukaneProduktyLowerName.Add(WszystkieProdukty.Find(x => x.Name.ToLower() == NazwaProduktu || x.Symbol.ToLower() == NazwaProduktu));
                    }

                }

                return WyszukaneProduktyLowerName;
            }
            else
            {
                return null;
            }
        }


    }
}
