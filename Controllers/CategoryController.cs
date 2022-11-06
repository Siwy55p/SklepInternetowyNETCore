using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using partner_aluro.Core;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using SmartBreadcrumbs.Nodes;
using System.Reflection;
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


        public CategoryController(ICategoryService categoryDB, IUnitOfWorkCategory iUnitOfWorkCategory, ApplicationDbContext context)
        {
            _categoryService = categoryDB;
            _iUnitOfWorkCategory = iUnitOfWorkCategory;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Categorys = await _categoryService.List();
            return View(Categorys);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            ModelState.Remove("SubCategories");
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            //logika zapisania kategorii do bazy.
            _categoryService.AddSave(category);

            return RedirectToAction("List");

        }

        [HttpGet]
        public IActionResult AddSubCategory()
        {
            ViewData["kategorie"] = GetCategories();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSubCategoryAsync(SubCategory data)
        {
            ModelState.Remove("SubCategories");
            data.Category = await _categoryService.GetAsync(data.SubCategoryId);

            ModelState.Remove("Categories");
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            //logika zapisania kategorii do bazy.
            _categoryService.AddSave(data);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var kategoria = await _categoryService.GetAsync(id);
            return View(kategoria);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(Category data)
        {
            Category kategoria = await _iUnitOfWorkCategory.Category.GetAsync(data.CategoryId);



            if (kategoria == null)
            {
                return NotFound();
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
            //var products = _cart.GetAllCartItems();

            List<Product> produkty = await _context.Products.Where(x => x.Ukryty == false).Where(x => x.CategoryId == KategoriaId).ToListAsync();

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
            var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

            if (Sort == 1)
            {
                onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }
            else if (Sort == 2)
            {
                onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }
            else if (Sort == 3)
            {
                onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }
            else if (Sort == 4)
            {
                onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }


            ViewBag.OnePageOfProducts = onePageOfProducts;

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

            //jesli jest cos w karcie przekaz do zmiennej, pokaz wartosc karty true
            //if (products.Count > 0)
            //{
            //    ViewData["Pokaz"] = "show";
            //}

            if (szukanaNazwa == null || szukanaNazwa == "")
            {
                szukanaNazwa = "";
                List<Product> produkty = await _context.Products.Where(x => x.Ukryty == false).ToListAsync();
                var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
                var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

                if (Sort == 1)
                {
                    onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                } else if (Sort == 2)
                {
                    onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                } else if (Sort == 3)
                {
                    onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                } else if (Sort == 4)
                {
                    onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                }


                ViewBag.OnePageOfProducts = onePageOfProducts;

                return View();
            }
            else
            {

                List<Product> produkty2 = await _context.Products.Where(x => x.Ukryty == false).Where(x => x.CategoryNavigation.Name == szukanaNazwa || x.CategorySubNavigation.Name == szukanaNazwa).ToListAsync();


                if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
                {
                    List<Product> produkty = (List<Product>)await Szukanie(szukanaNazwa);

                    var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

                    foreach (var produkt in produkty2)
                    {
                        produkty.Add(produkt);
                    }

                    var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

                    if (Sort == 1)
                    {
                        onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    } else if (Sort == 2)
                    {
                        onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    } else if (Sort == 3)
                    {
                        onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
                    } else if (Sort == 4)
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
            //var products = _cart.GetAllCartItems();

            var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            ViewData["BreadcrumbNode"] = categoryPage;
            ViewData["Title"] = szukanaNazwa;
            ViewData["szukanaNazwa"] = szukanaNazwa;

            List<Product> produkty = await Szukanie(szukanaNazwa);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = produkty.ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize

            if (Sort == 1)
            {
                onePageOfProducts = produkty.OrderBy(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            } else if (Sort == 2)
            {
                onePageOfProducts = produkty.OrderByDescending(p => p.Name).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            } else if (Sort == 3)
            {
                onePageOfProducts = produkty.OrderBy(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            } else if (Sort == 4)
            {
                onePageOfProducts = produkty.OrderByDescending(p => p.Symbol).ToPagedList(pageNumber, 9); // will only contain 25 products max because of the pageSize
            }

            ViewBag.OnePageOfProducts = onePageOfProducts;

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

            lstCategories = _categoryService.GetList().Select(ct => new SelectListItem()
            {
                Value = ct.CategoryId.ToString(),
                Text = ct.Name
            }).ToList();

            var dmyItem = new SelectListItem()
            {
                Value = null,
                Text = "--- Wybierz Kategorie ---"
            };

            lstCategories.Insert(0, dmyItem);
            return lstCategories;
        }

    }
}
