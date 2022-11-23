using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Data;
using Microsoft.EntityFrameworkCore;
using DeepL;
using System.Resources.NetStandard;
using System.Collections;

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

       
        public ProductController(IProductCategoryService productCategoryService, ApplicationDbContext applicationDbContext, IProductService productService, IUnitOfWorkProduct unitOfWorkProduct, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _ProductService = productService;
            _unitOfWorkProduct = unitOfWorkProduct;
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;
            _imageService = imageService;

            _productCategoryService = productCategoryService;


            
        }

        public async Task<IActionResult> Index()
        {
            return View(await _ProductService.GetProductList());
        }



        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public async Task <IActionResult> Edit(int id)
        {
            ViewBag.Category = GetCategories();
            Product produkt = await _ProductService.GetProductId(id);
            produkt.Kategorie = _context.ProductCategory.Where(x => x.ProductID == id).ToList();
            return View(produkt);
        }


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.Category = GetCategories();

            if (id != product.ProductId)
            {
                return NotFound();
            }

            ImageModel front = _context.Images.FirstOrDefault(x => x.ImageName == "Front_" + product.Symbol + ".jpg");
            if (front != null)
            {
                product.ImageUrl = front.ImageName;
            }

            int i = 0;
            i++;

            if(product.product_Image.ImageFile != null)
            {
                product.ImageUrl = await _imageService.DeleteFrontImage(product);

                product.ImageUrl = await _imageService.CreateImageAddAsync(product);
            }

            UploadFile2Async(product);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(List));
            }
            ViewData["CategoryId"] = GetCategories();
            return View(product);
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Category = GetCategories();

            Product product = new();

            //product.Cats = _ProductService.GetListCategory();

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
            }else
            {
                _productCategoryService.DeleteProductCategoryMultiple(ProduktId, KategoriaID);
            }

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
            ViewBag.Category = GetCategories();

            //string NameEn = "Angielska Nazwa";



            var authKey = $"bbc4aaae-78af-4f5e-37dd-34e29f91a480:fx"; // Replace with your key
            var translator = new Translator(authKey);

            string NameEn = product.Name.ToString();
            string NameDE = product.Name.ToString();

            var translatedText1 = await translator.TranslateTextAsync(
              NameEn,
              "PL",
              "en-US");
            NameEn = translatedText1.Text;

            var translatedText2 = await translator.TranslateTextAsync(
              NameDE,
              "PL",
              "DE");
            NameDE = translatedText2.Text;

            //Dodanie do pliku resx tlumaczenia nazwy produktu
            string webRootPath = _webHostEnvironment.ContentRootPath;
            string resxFile1 = webRootPath+ "\\Resources\\SharedResource.pl-PL.resx";

            Dictionary<string, string> dict1 = new Dictionary<string, string>();
            dict1.Add(product.Name, product.Name);
            Hashtable data1 = new Hashtable(dict1);
            UpdateResourceFile(data1, resxFile1);
            // KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu
            //Dodanie do pliku resx tlumaczenia nazwy produktu
            string resxFile2 = webRootPath + "\\Resources\\SharedResource.en-US.resx";

            Dictionary<string, string> dict2 = new Dictionary<string, string>();
            dict2.Add(product.Name, NameDE);
            Hashtable data2 = new Hashtable(dict2);
            UpdateResourceFile(data2, resxFile2);
            // KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu

            //Dodanie do pliku resx tlumaczenia nazwy produktu
            string resxFile3 = webRootPath + "\\Resources\\SharedResource.de-DE.resx";

            Dictionary<string, string> dict3 = new Dictionary<string, string>();
            dict3.Add(product.Name, NameDE);
            Hashtable data3 = new Hashtable(dict3);
            UpdateResourceFile(data3, resxFile3);
            // KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu Niemiecki

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


            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var id = _ProductService.AddProduct(product);//wazne aby przypisac

            product.ImageUrl = await _imageService.CreateImageAddAsync(product);

            product.ProductImagesId = product.product_Image.ImageId;

            UploadFile2Async(product);

            //var id = _ProductService.AddProduct(product);//wazne aby przypisac
            //var produkt = _unitOfWorkProduct.Product.GetProductId(product.ProductId);
            //produkt.ProductImagesId = product.ProductId;

            await _context.SaveChangesAsync();

            ProductCategory productCategory = new ProductCategory()
            {
                ProductID = product.ProductId,
                CategoryID = product.CategoryId
            };
            _productCategoryService.AddProductCategoryMultiple(productCategory);


            return RedirectToAction(nameof(List));


        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            ViewBag.Category = GetCategories();
            return View(await _ProductService.GetProductList());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _ProductService.GetProductId(id));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _ProductService.DeleteProductId(id);
            return RedirectToAction("List");
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        private async Task UploadFile2Async(Product product)
        {
            var files = HttpContext.Request.Form.Files;

            if (product.ImageUrl != "" && files.Count > 1) //To oznacza ze frontowy obrazek został dodany
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                for (int i = 1; i <= files.Count; i++)
                {
                    //Save image to wwwroot/image
                    string path0 = "images\\produkty\\";
                    var uploadsFolder = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = product.Symbol + "_"+ i + "_" + extension;

                    using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
                    {
                        files[i].CopyTo(filesStream);
                    }

                    //add product Image for new product
                    ImageModel imgModel = new()
                    {
                        path = path0 + product.Symbol + "\\",
                        fullPath = path0 + product.Symbol + "\\" + dynamicFileName,
                        kolejnosc = i,
                        Tytul = product.Name,
                        ImageName = dynamicFileName,
                        ProductId = product.ProductId
                    };

                    product.Product_Images.Add(imgModel);

                    _imageService.Add(imgModel);

                }
            }
            else if (files.Count > 0 && product.ImageUrl == "") //Fronyowy obrazek nie został dodany zacznij dodawac od 0
            {

                string webRootPath = _webHostEnvironment.WebRootPath;

                for (int i = 0; i <= files.Count; i++)
                {
                    string path0 = "images\\produkty\\";

                    var uploads = Path.Combine(webRootPath, path0 + product.Symbol);

                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = product.Symbol + "_" + i + "_" + extension;
                    //var dynamicFileName = DateTime.Now.ToString("yymmssfff") + "_" + extension;

                        using (var filesStream = new FileStream(Path.Combine(uploads, dynamicFileName), FileMode.Create))
                        {
                            files[i].CopyTo(filesStream);
                        }


                    //add product Image for new product
                    ImageModel imgModel = new()
                    {
                        path = path0 + product.Symbol +"\\",
                        fullPath = path0 + product.Symbol +"\\"+ dynamicFileName,
                        kolejnosc = i,
                        Tytul = product.Name,
                        ImageName = dynamicFileName,
                        ProductId = product.ProductId
                    };

                    product.Product_Images.Add(imgModel);


                    _imageService.Add(imgModel);


                }
            }
        }

        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //private string UploadFile(Product product)
        //{
        //    string uniqueFileName = null;

        //    var files = HttpContext.Request.Form.Files;

        //    if (files.Count > 0)
        //    {
        //        if (HttpContext.Request.Form.Files[0] != null)
        //        {
        //            var file = HttpContext.Request.Form.Files[0];

        //            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\produkty\\" + product.Symbol);

        //            if (!Directory.Exists(uploadsFolder))
        //            {
        //                Directory.CreateDirectory(uploadsFolder);
        //            }
        //            //uniqueFileName = "Front" + DateTime.Now.ToString("yymmssfff") + "_" + product.FrontImage.FileName;

        //            var extension = Path.GetExtension(files[0].FileName);

        //            uniqueFileName = "Front_" + product.Symbol + extension;
        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //        }
        //    }

        //    if (uniqueFileName != null)
        //    {
        //        return uniqueFileName;
        //    }
        //    else
        //    {

        //        return uniqueFileName = "Front_" + product.Symbol +".jpg";

        //    }

        //}

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