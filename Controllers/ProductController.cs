﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Data;
using Microsoft.EntityFrameworkCore;
//using DeepL;
using System.Resources.NetStandard;
using System.Collections;
using System.Globalization;
using javax.jws;

namespace partner_aluro.Controllers
{

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IUnitOfWorkProduct _unitOfWorkProduct;
        private readonly IProductService _ProductService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IImageService _imageService;

        private readonly IProductCategoryService _productCategoryService;


        public ProductController(IProductCategoryService productCategoryService, ApplicationDbContext applicationDbContext,
            IProductService productService, IUnitOfWorkProduct unitOfWorkProduct,
            IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _ProductService = productService;
            _unitOfWorkProduct = unitOfWorkProduct;
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;
            _imageService = imageService;

            _productCategoryService = productCategoryService;



        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _ProductService.GetProductList());
        //}

        [HttpPost]
        public string CenaNetto(decimal CenaBrutto)
        {
            decimal Cena_netto = CenaBrutto / partner_aluro.Core.Constants.Vat;
            Cena_netto = decimal.Round(Cena_netto, 2, MidpointRounding.AwayFromZero);

            Cena_netto = Convert.ToDecimal(Cena_netto, CultureInfo.GetCultureInfo("pl-PL"));
            Cena_netto.ToString("#.##");
            return Cena_netto.ToString("#.##");
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Category"] = GetCategories();

            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();


            //ViewBag.Category = GetCategories();
            Product produkt = await _ProductService.GetProductId(id);
            produkt.Kategorie = _context.ProductCategory.Where(x => x.ProductID == id).ToList();
            return View(produkt);
        }

        //public void UpdateRow(Product model, int Id, int fromPosition, int toPosition)
        //{

        //        var ProductImageList = model.Product_Images.ToList();
        //        model.Product_Images.First(c => c.kolejnosc == Id).kolejnosc = toPosition;

        //}

        [HttpPost]
        public void UpdateRow(int ImageId, int kolejnosc, int Id, int oldPosition, int newPosition)
        {
            ImageModel image = _context.Images.Where(x => x.ImageId == ImageId).FirstOrDefault();

            image.kolejnosc = newPosition;
            _context.Images.Update(image);
            _context.SaveChanges();


        }

        bool translate = false;

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewData["Category"] = GetCategories();
            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();
            //ViewBag.Category = GetCategories();

            product.Product_Images = _context.Images.Where(x => x.ProductImagesId == product.ProductId).ToList();

            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (product.NameEn == null || product.NameDe == null && translate==true)
            {
                //var authKey = $"bbc4aaae-78af-4f5e-37dd-34e29f91a480:fx"; // Replace with your key
                //var translator = new Translator(authKey);

                //string NameEn = product.Name.ToString();
                //string NameDe = product.Name.ToString();

                //var translatedText1 = await translator.TranslateTextAsync(
                //  NameEn,
                //  "PL",
                //  "en-US");
                //NameEn = translatedText1.Text;

                //var translatedText2 = await translator.TranslateTextAsync(
                //  NameDe,
                //  "PL",
                //  "DE");
                //NameDe = translatedText2.Text;

                //product.NameEn = NameEn;
                //product.NameDe = NameDe;
            }

            if (product.SzukanaNazwa == null)
            {
                product.SzukanaNazwa = product.Name + " - [" + product.Symbol + "]";
            }


            //string imageName = "Front_" + product.Symbol + ".jpg";
            //ImageModel front = _imageService.Get(imageName);
            //if (front != null)
            //{
            //    product.ImageUrl = front.ImageName;
            //}

            //int i = 0;
            //i++;

            //if(product.product_Image.ImageFile != null)
            //{
            //    product.ImageUrl = await _imageService.DeleteFrontImage(product);

            //    product.ImageUrl = await _imageService.CreateImageAddAsync(product);
            //}

            var files = HttpContext.Request.Form.Files;
            _imageService.UploadFiles(files, product);
            ////UploadNewFilePicture
            //ImageController.Initialize(_webHostEnvironment);
            //ImageModel imgModel = new ImageModel();
            //var files = HttpContext.Request.Form.Files;
            //imgModel = ImageController.UploadFiles(files, product);
            //_imageService.AddAsync(imgModel);
            ////UploadNewFilePicture


            if ((product.ImageUrl == null || product.ImageUrl == "") && product.Product_Images != null)
            //Jesli nie ma obrazka glownego a jest obrazk [0] jako dodatkowy do wysietlenia , to wybierze ten obrazek i ustaw jako glowny.
            {

                if (product.Product_Images.Count >= 1)
                {
                    if (product.Product_Images[0].fullPath != "" && product.Product_Images[0] != null)
                    {
                        //product.product_Image = product.Product_Images[0];
                        product.ImageUrl = product.Product_Images[0].ImageName;
                        product.pathImageUrl250x250 = product.Product_Images[0].pathImageCompress250x250;
                        product.ProductImagesId = product.Product_Images[0].ImageId;
                    }
                }
            }

            //ustaw obrazek glowny ten ktory jest jako pierwszy w tabelce imagePictures

            if (product.Product_Images.FirstOrDefault() != null)
            {
                product.ImageUrl = product.Product_Images.OrderBy(x => x.kolejnosc).FirstOrDefault().ImageName;

                product.Product_Images = product.Product_Images.OrderBy(x => x.kolejnosc).ToList();

                product.pathImageUrl250x250 = product.Product_Images[0].pathImageCompress250x250;
                product.ProductImagesId = product.Product_Images[0].ImageId;
            }

            ModelState.Remove("product_Image.path");
            ModelState.Remove("product_Image.ImageName");
            ModelState.Remove("product_Image.fullPath");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }

            product.Kategorie = _context.ProductCategory.Where(x => x.ProductID == id).ToList();

            ViewData["Category"] = GetCategories();
            return View(product);
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Add()
        {
            //ViewBag.Category = GetCategories();

            ViewData["Category"] = GetCategories();
            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();

            Product product = new Product();
            //product.Cats = _ProductService.GetListCategory();

            var item = _context.Category.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.CategoryId.ToString()
            }).ToList();

            product.categories = item;

            return View(product);
        }

        public void AddToKategoria(int ProduktId, int KategoriaID, bool check)
        {

            if (check)
            {
                ProductCategory productCategory = new ProductCategory()
                {
                    ProductID = ProduktId,
                    CategoryID = KategoriaID
                };
                _productCategoryService.AddProductCategoryMultiple(productCategory);
            } else
            {
                _productCategoryService.DeleteProductCategoryMultiple(ProduktId, KategoriaID);
            }

        }

        [HttpPost]
        [WebMethod]
        public void ChangeCategory(int ProduktId, int KategoriaID)
        {
            Product product = _context.Products.Where(x => x.ProductId == ProduktId).FirstOrDefault();

            product.CategoryId = KategoriaID;
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        [HttpPost]
        [WebMethod]
        public void ChangeIlosc(int ProduktId, int Ilosc)
        {
            Product product = _context.Products.Where(x => x.ProductId == ProduktId).FirstOrDefault();

            product.Ilosc = Ilosc;
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        [HttpPost]
        [WebMethod]
        public void TakeProduct(int ProduktId, bool check)
        {
            ExcelController.Initialize();

            Product product = _context.Products.Where(x => x.ProductId == ProduktId).FirstOrDefault();

            Product find = ExcelController.produkty.Where(x => x.ProductId == ProduktId).FirstOrDefault();
            //logika check or uncheck
            if (check)
            {
                if (find != null)
                {
                    //produktu jest jest dodany
                }
                else
                {
                    ExcelController.produkty.Add(product);
                }

            }
            else
            {
                ExcelController.produkty.Remove(product);
                if (find != null)
                {
                    ExcelController.produkty.Remove(find);
                }
            }
        }

        [HttpPost]
        [WebMethod]
        public  void ChangeCheckUkryty(int ProduktId)
        {
            Product product = _context.Products.Where(x => x.ProductId == ProduktId).FirstOrDefault();

            if(product.Ukryty == true)
            {
                product.Ukryty = false;
                _context.Update(product);
                _context.SaveChanges();
            }else if (product.Ukryty == false)
            {
                product.Ukryty = true;
                _context.Update(product);
                _context.SaveChanges();
            }

        }

        [HttpPost]
        public void TakeProductId(int ProduktId)
        {
            ExcelController.Initialize();

            Product product = _context.Products.Where(x => x.ProductId == ProduktId).FirstOrDefault();

            Product find = ExcelController.produkty.Where(x => x.ProductId == ProduktId).FirstOrDefault();
            //logika check or uncheck
            if (find != null)
            {
                //produktu jest jest dodany
            }
            else
            {
                ExcelController.produkty.Add(product);
            }

            //if (find != null)
            //{
            //    ExcelController.produkty.Remove(product);
            //}

        }


        public static void UpdateResourceFile(Hashtable data, String path)
        {
            Hashtable resourceEntries = new Hashtable();

            //Get existing resources
            ResXResourceReader reader = new ResXResourceReader(path);
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            //Modify resources here...
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {

                    String value = data[key].ToString();
                    if (value == null) value = "";

                    resourceEntries.Add(key, value);
                }
            }

            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);

            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();

        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            //ViewBag.Category = GetCategories();

            var courseIds = product.categories.Where(x => x.Selected).Select(y => y.Value);

            ViewData["Category"] = GetCategories();

            //string NameEn = "Angielska Nazwa";

            if (translate == true)
            {
                //var authKey = $"bbc4aaae-78af-4f5e-37dd-34e29f91a480:fx"; // Replace with your key
                //var translator = new Translator(authKey);

                //string NameEn = product.Name.ToString();
                //string NameDe = product.Name.ToString();

                //var translatedText1 = await translator.TranslateTextAsync(
                //  NameEn,
                //  "PL",
                //  "en-US");
                //NameEn = translatedText1.Text;

                //var translatedText2 = await translator.TranslateTextAsync(
                //  NameDe,
                //  "PL",
                //  "DE");
                //NameDe = translatedText2.Text;

                //product.NameEn = NameEn;
                //product.NameDe = NameDe;
            }

            ////Dodanie do pliku resx tlumaczenia nazwy produktu
            //string webRootPath = _webHostEnvironment.ContentRootPath;
            //string resxFile1 = webRootPath + "\\wwwroot\\Resources\\SharedResource.pl-PL.resx";

            //Dictionary<string, string> dict1 = new Dictionary<string, string>();
            //dict1.Add(product.Name, product.Name);
            //Hashtable data1 = new Hashtable(dict1);
            //UpdateResourceFile(data1, resxFile1);
            //// KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu
            ////Dodanie do pliku resx tlumaczenia nazwy produktu
            //string resxFile2 = webRootPath + "\\wwwroot\\Resources\\SharedResource.en-US.resx";

            //Dictionary<string, string> dict2 = new Dictionary<string, string>();
            //dict2.Add(product.Name, NameDE);
            //Hashtable data2 = new Hashtable(dict2);
            //UpdateResourceFile(data2, resxFile2);
            //// KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu

            ////Dodanie do pliku resx tlumaczenia nazwy produktu
            //string resxFile3 = webRootPath + "\\wwwroot\\Resources\\SharedResource.de-DE.resx";

            //Dictionary<string, string> dict3 = new Dictionary<string, string>();
            //dict3.Add(product.Name, NameDE);
            //Hashtable data3 = new Hashtable(dict3);
            //UpdateResourceFile(data3, resxFile3);
            //// KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu Niemiecki

            product.Bestseller = true;
            product.ImageUrl = "";

            ModelState.Remove("CategoryNavigation");
            ModelState.Remove("CategorySubNavigation");
            ModelState.Remove("product_Images");

            Product produktTest = _context.Products.Where(x => x.Symbol == product.Symbol).FirstOrDefault();
            if (produktTest != null)
            {
                var mod = ModelState.First(c => c.Key == "Symbol");  // this
                mod.Value.Errors.Add("Symbol musi być unikatowy, ten Symbol już występuje.");    // this
                return View(product);
            }

            ModelState.Remove("product_Image.fullPath");
            ModelState.Remove("product_Image.ImageName");
            ModelState.Remove("product_Image.path");
            if (!ModelState.IsValid)
            {
                product.categories = null;

                var item = _context.Category.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.CategoryId.ToString()
                }).ToList();

                product.categories = item;

                return View(product);

            }

            var id = _ProductService.AddProduct(product);//wazne aby przypisac

            //product.ImageUrl = await _imageService.CreateImageAddAsync(product);

            //product.ProductImagesId = product.product_Image.ImageId;

            //await UploadFile2Async(product);

            var files = HttpContext.Request.Form.Files;


            _imageService.UploadFiles(files, product);


            if ((product.Product_Images != null) && (product.Product_Images.FirstOrDefault() != null))
            {
                product.ImageUrl = product.Product_Images.OrderBy(x => x.kolejnosc).FirstOrDefault().ImageName;

                product.Product_Images = product.Product_Images.OrderBy(x => x.kolejnosc).ToList();

                product.ProductImagesId = product.Product_Images.FirstOrDefault().ImageId;


                product.pathImageUrl250x250 = product.Product_Images.FirstOrDefault().pathImageCompress250x250;
            }

            product.SzukanaNazwa = product.Name + " - [" + product.Symbol + "]";

            _context.Update(product);
            _context.SaveChanges();

            //UploadNewFilePicture
            //ImageController.Initialize(_webHostEnvironment);

            //ImageModel imgModel = new ImageModel();

            //imgModel = ImageController.UploadFiles(files,product);
            //_imageService.AddAsync(imgModel);
            //UploadNewFilePicture

            //var id = _ProductService.AddProduct(product);//wazne aby przypisac
            //var produkt = _unitOfWorkProduct.Product.GetProductId(product.ProductId);
            //produkt.ProductImagesId = product.ProductId;

            //await _context.SaveChangesAsync();
            ProductCategory productCategory = new ProductCategory()
            {
                ProductID = product.ProductId,
                CategoryID = product.CategoryId
            };


            //_productCategoryService.AddProductCategoryMultiple(productCategory);

            foreach (var ids in courseIds)
            {

                productCategory = new ProductCategory()
                {
                    ProductID = product.ProductId,
                    CategoryID = Int32.Parse(ids)
                };
                _productCategoryService.AddProductCategoryMultiple(productCategory);
            }
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            ExcelController.Initialize(false);
            ExcelController.Initialize();

            ViewData["Category"] = GetCategories();
            List<ProductsList> ProductsList = await _context.Products
                .AsNoTracking()
                .Include(p => p.CategoryNavigation)
                //.Where(p => p.CategoryNavigation.Aktywny == true)
                .Select(p=> new ProductsList{ 
                    ProductId = p.ProductId, 
                    Name = p.Name,
                    pathImageUrl250x250 = p.pathImageUrl250x250, 
                    Symbol = p.Symbol,
                    Ukryty = p.Ukryty,
                    Ilosc = (int)p.Ilosc,
                    CenaProduktuBrutto = p.CenaProduktuBrutto,
                    CenaProduktuDetal = p.CenaProduktuDetal,
                    CategoryNavigation = p.CategoryNavigation,
                    CategoryId = p.CategoryId,
                })
                .OrderBy(p=> p.ProductId)
                .OrderByDescending(p=> p.ProductId)
                .ToListAsync();

            //(p => new PInfo { Name = p.NameDe.ToLower(), NameBezZnako

            return View(ProductsList);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Product product = await _ProductService.GetProductId(id);
            product.Product_Images = product.Product_Images.OrderBy(x=>x.kolejnosc).ToList();

            return View(product);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _context.Products.Find(id);

            ////delete image from wwwroot/images
            //var PathProduct = Path.Combine(_webHostEnvironment.WebRootPath + "\\img\\p\\" + product.Symbol);

            //if (System.IO.Directory.Exists(PathProduct))
            //    System.IO.Directory.Delete(PathProduct, true);
            ////delete tge record

            _ProductService.DeleteProductId(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) //View
        {
            ViewData["Category"] = GetCategories();

            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();


            //ViewBag.Category = GetCategories();
            Product produkt = await _ProductService.GetProductId(id);
            produkt.Kategorie = _context.ProductCategory.Where(x => x.ProductID == id).ToList();
            return View(produkt);
        }


        public int DeletePermanentlyProductId(int id)
        {
            var product = _context.Products.Find(id);


            List<ProductCategory> productCategor = _context.ProductCategory.Where(x => x.ProductID == id).ToList();
            for (int i = 0; i < productCategor.Count; i++)
            {
                _context.ProductCategory.Remove(productCategor[i]);
                _context.SaveChanges();
            }
            product.Ilosc = 0;
            product.CategoryId = 28;

            _context.Products.Remove(product);
            _context.SaveChanges();

            return id;
        }

        private List<SelectListItem> GetCategories()
        {
            var lstCategories = new List<SelectListItem>();


            lstCategories = _ProductService.GetListCategory().Select(ct => new SelectListItem()
            {
                Value = ct.CategoryId.ToString(),
                Text = ct.Name
            }).ToList();

            //var dmyItem = new List<SelectListItem>();

            //dmyItem = _context.Category.Where(x => x.Name == "Bez kategori").ToList().Select(di => new SelectListItem()
            //{
            //    Value = di.CategoryId.ToString(),
            //    Text = di.Name
            //}).ToList();


            //lstCategories.Insert(0, dmyItem );

            return lstCategories;
        }


    }
}