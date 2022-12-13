﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Core;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Resources;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using SmartBreadcrumbs.Nodes;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace partner_aluro.Controllers
{

    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.Klient}")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkCategory _iUnitOfWorkCategory;

        private readonly ApplicationDbContext _context;


        private readonly LanguageService _language;

        private static int Pages = 12;

        public CategoryController(ICategoryService categoryDB, IUnitOfWorkCategory iUnitOfWorkCategory, ApplicationDbContext context, LanguageService language)
        {
            _categoryService = categoryDB;
            _iUnitOfWorkCategory = iUnitOfWorkCategory;
            _context = context;
            _language = language;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var Categorys = await _categoryService.List();
        //    return View(Categorys);
        //}

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["kategorie"] = GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(Category category)
        {
            ModelState.Remove("SubCategories");
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if(category.ParentId > 0)
            {
                category.ChildId = 2;
                Category cat = _context.Category.Where(c => c.CategoryId == category.ParentId).FirstOrDefault();

                Category kategoria = await _iUnitOfWorkCategory.Category.GetAsync(cat.CategoryId);

                if (kategoria == null)
                {
                    return NotFound();
                }

                kategoria.ChildId = 1;

                _iUnitOfWorkCategory.Category.Update(kategoria);

                _context.SaveChanges();

            }else
            {
                category.ChildId = 0;
            }
            //logika zapisania kategorii do bazy.
            _categoryService.AddSave(category);

            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var kategoria = await _categoryService.GetAsync(id);


            Category kategoria = _context.Category.Include(p => p.Produkty).Where(x=>x.CategoryId == id).FirstOrDefault();

            ViewData["Category"] = GetCategories();

            return View(kategoria);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category data)
        {
            Category kategoria = await _iUnitOfWorkCategory.Category.GetAsync(data.CategoryId);

            if (kategoria == null)
            {
                return NotFound();
            }


            List<Category> categories = _context.Category.ToList();

            for(int i = 0; i < categories.Count(); i++)
            {
                Category category = categories[i];
                if(category.ParentId == data.CategoryId && data.ParentId==0)
                {
                    kategoria.ChildId = 1;
                }else if(category.ParentId != 0)
                {
                    kategoria.ChildId = 2;
                }else
                {
                    kategoria.ChildId = 0;
                }
            }


            kategoria.Name = data.Name;
            kategoria.Description = data.Description;
            kategoria.kolejnosc = data.kolejnosc;
            kategoria.Aktywny = data.Aktywny;

            _iUnitOfWorkCategory.Category.Update(kategoria);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = _categoryService.GetAsync(id);
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
            var Category = await _categoryService.List();

            return View(Category);
        }

        //TUTAJ WYSWIETLAM STRONE PODSTAWOWĄ DLA WYSWIETLENIA PRODUKTOW Z ID KATEGORIA
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista1(int KategoriaId, int? page, string? szukanaNazwa, int? Sort) //Link do wyswietlania po wyborze kategorii
        {


            //KategoriaId = 3;

            List<Product> produkty = await _context.ProductCategory.Where(x => x.CategoryID == KategoriaId).Include(x=>x.Product).Where(x => x.Product.Ukryty == false).Select(p=>p.Product).ToListAsync();
            
            //var products = _cart.GetAllCartItems();

            //List<Product> produkty = await _context.Products.Where(x => x.Ukryty == false).Where(x => x.CategoryId == KategoriaId).ToListAsync();

            //List<Product> produkty = await _context.Products.Where(x => x.Ukryty == false).Where(x => x.CategoryId == KategoriaId).ToListAsync();

            szukanaNazwa = _categoryService.GetName(KategoriaId);
            var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            ViewData["BreadcrumbNode"] = categoryPage;
            ViewData["Title"] = szukanaNazwa;
            ViewData["szukanaNazwa"] = szukanaNazwa;

            ViewData["Sort"] = Sort;

            //jesli jest cos w karcie przekaz do zmiennej, pokaz wartosc karty true
            //if (products.Count > 0)
            //{
            //    ViewData["Pokaz"] = "show";
            //}

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = produkty.ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize

            switch (Sort)
            {
                case 1:
                    onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                case 2:
                    onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                case 3:
                    onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                case 4:
                    onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                default:
                    onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, Pages);
                    break;
            }

            //ViewData["OnePageOfProducts"] = onePageOfProducts;

            //ViewBag.OnePageOfProducts = onePageOfProducts;
            ViewData["OnePageOfProducts"] = onePageOfProducts;

            return View(produkty);


        }



        //wyswietlane produkty na bierzaca nie zalezne ktora metoda jest uzyta


        //TUTAJ WYSWIETLAM STRONE PODSTAWOWĄ DLA WYSWIETLENIA PRODUKTOW Z ID KATEGORIA szukanaNazwa
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista(int? page, string? szukanaNazwa, int? Sort) //Link do wyswietlania po wyborze kategorii
        {
            //var products = _cart.GetAllCartItems();

            var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            ViewData["BreadcrumbNode"] = categoryPage;
            ViewData["Title"] = szukanaNazwa;
            ViewData["szukanaNazwa"] = szukanaNazwa;

            ViewData["Sort"] = Sort;


            if (szukanaNazwa == null || szukanaNazwa == "")
            {
                szukanaNazwa = "";
                List<Product> produkty = await _context.Products.Where(x => x.Ukryty == false).ToListAsync();
                var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
                var onePageOfProducts = produkty.ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize

                
                switch(Sort)
                {
                    case 1:
                        onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                        break;
                    case 2:
                        onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                        break;
                    case 3:
                        onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                        break;
                    case 4:
                        onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                        break;
                    default:
                        break;
                }


                ViewData["OnePageOfProducts"] = onePageOfProducts;

                return View();
            }
            else
            {
                List<Product> produkty2 = new List<Product>();
                Category a = _context.Category.Where(x => x.Name == szukanaNazwa).FirstOrDefault();
                if (a == null)
                {
                    produkty2 = await _context.Products.Where(x => x.Ukryty == false).Where(x => x.CategoryNavigation.Name == szukanaNazwa).ToListAsync();

                    if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
                    {
                        List<Product> produkty = (List<Product>)await Szukanie(szukanaNazwa);

                        var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

                        foreach (var produkt in produkty2)
                        {
                            produkty.Add(produkt);
                        }

                        var onePageOfProducts = produkty.ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize

                        switch (Sort)
                        {
                            case 1:
                                onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                                break;
                            case 2:
                                onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                                break;
                            case 3:
                                onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                                break;
                            case 4:
                                onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                                break;
                            default:
                                break;
                        }

                        //ViewBag.OnePageOfProducts = onePageOfProducts;
                        ViewData["OnePageOfProducts"] = onePageOfProducts;

                        return View(produkty);
                    }
                    else
                    {
                        return View(produkty2);
                    }
                }
                else
                {
                    produkty2 = await _context.ProductCategory.Where(x => x.CategoryID == a.CategoryId).Select(x => x.Product).ToListAsync();


                    var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

                    var onePageOfProducts = produkty2.ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    
                    switch (Sort)
                    {
                        case 1:
                            onePageOfProducts = produkty2.OrderBy(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                            break;
                        case 2:
                            onePageOfProducts = produkty2.OrderByDescending(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                            break;
                        case 3:
                            onePageOfProducts = produkty2.OrderBy(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                            break;
                        case 4:
                            onePageOfProducts = produkty2.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                            break;
                        default:
                            break;
                    }
                    ViewData["OnePageOfProducts"] = onePageOfProducts;

                    return View(produkty2);
                }

            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista2(int? page, string? szukanaNazwa, int? Sort) //Link do wyswietlania po wyborze kategorii TO JEST TYLKO KONTENER
        {
            //var products = _cart.GetAllCartItems();

            var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            ViewData["BreadcrumbNode"] = categoryPage;
            ViewData["Title"] = szukanaNazwa;
            ViewData["szukanaNazwa"] = szukanaNazwa;

            List<Product> produkty = await Szukanie(szukanaNazwa);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = produkty.ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize

            switch (Sort)
            {
                case 1:
                    onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                case 2:
                    onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                case 3:
                    onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                case 4:
                    onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, Pages); // will only contain 25 products max because of the pageSize
                    break;
                default:
                    break;
            }

            //ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewData["OnePageOfProducts"] = onePageOfProducts;

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<List<Product>>? Szukanie(string szukanaNazwa)   // to jest wynik wyszukiwarki (zwraca produkty ktore zawieraja w nazwie szukana nazwe)
        {
            List<Product> WszystkieProdukty = await _context.Products.Where(x => x.Ukryty == false).ToListAsync();



            List<PInfo> WyszukaneNazwyProdukow = await _context.Products.Where(x => x.Ukryty== false).Select(p => new PInfo{ Name = p.Name.ToLower(), Symbol = p.Symbol.ToString().ToLower() }).ToListAsync(); // tworze liste nazw (puste)

            //List<string> WyszukaneSymbolowProdukow = await _context.Products.Where(x => x.Ukryty == false).Select(p => p.Symbol.ToLower()).ToListAsync(); // tworze liste nazw Symbolów wszystkie tylko symbole

            
            //WyszukaneNazwyProdukow.AddRange(WyszukaneSymbolowProdukow);

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                szukanaNazwa = szukanaNazwa.ToLower();

                //WyszukaneNazwyProdukow zawiera wszystkie nazwy wszystkich produków
                // oraz produkty ktore zawieraja ten symbol



                List<Product> WyszukaneProduktyLowerName = new();

                foreach (PInfo NazwaProduktu in WyszukaneNazwyProdukow)
                {
                    if (NazwaProduktu.Name.Contains(szukanaNazwa) || NazwaProduktu.Symbol.Contains(szukanaNazwa)) // jesli jakas nazwa jest w liscie 
                    {
                        
                        WyszukaneProduktyLowerName.Add(WszystkieProdukty.Find(x => x.Name.ToLower() == NazwaProduktu.Name ));
                    }

                }

                return WyszukaneProduktyLowerName;
            }
            else
            {
                return null;
            }
        }

        private List<SelectListItem> GetCategories()
        {
            var lstCategories = new List<SelectListItem>();

            lstCategories = _categoryService.GetList().Where(x=>x.ParentId == 0).OrderBy(x => x.CategoryId).Select(ct => new SelectListItem()
            {
                Value = ct.CategoryId.ToString(),
                Text = ct.Name
            }).ToList();

            //var dmyItem = new SelectListItem()
            //{
            //    Value = null,
            //    Text = "--- Wybierz Kategorie ---"
            //};

            //lstCategories.Insert(0, dmyItem);
            return lstCategories;
        }
    }
}
