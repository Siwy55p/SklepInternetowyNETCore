using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using partner_aluro.Data;
using partner_aluro.ViewModels;
using System.IO;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Services;

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

        public ProductController(ApplicationDbContext applicationDbContext, IProductService productService, IUnitOfWorkProduct unitOfWorkProduct, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _ProductService = productService;
            _unitOfWorkProduct = unitOfWorkProduct;
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;
            _imageService = imageService;
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
            ViewBag.SubCategory = GetSubCategories();
            Product produkt = await _ProductService.GetProductId(id);
            return View(produkt);
        }


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.Category = GetCategories();
            ViewBag.SubCategory = GetSubCategories();
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if(product.product_Image.ImageFile != null)
            {
                //_imageService.Edit()
            }

            product.ImageUrl = await _imageService.CreateImageAddAsync(product);

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
            ViewBag.SubCategory = GetSubCategories();
            Product product = new();


            return View(product);
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            ViewBag.Category = GetCategories();
            ViewBag.SubCategory = GetSubCategories();

            product.DataDodania = DateTime.Now;


            product.Bestseller = true;
            product.ImageUrl = "";


            //product.ImageUrl = await _imageService.CreateImageAddAsync(product.product_Image);

            //string uniqueFileName = UploadFile(product);
            //product.ImageUrl = uniqueFileName;

            ModelState.Remove("CategoryNavigation");
            ModelState.Remove("CategorySubNavigation");
            ModelState.Remove("product_Images");

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

            return RedirectToAction(nameof(List));


        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
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

            if (product.ImageUrl != "" && files.Count > 1)
            {
                if (HttpContext.Request.Form.Files[1] != null)
                {

                    string webRootPath = _webHostEnvironment.WebRootPath;

                    for(int i = 1; i <= files.Count; i++)
                    {
                        string path0 = "\\images\\produkty\\";
                        var uploads = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);

                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        var extension = Path.GetExtension(files[i].FileName);
                        var dynamicFileName = product.Symbol + "_"+ i + "_" + extension;

                        using (var filesStream = new FileStream(Path.Combine(uploads, dynamicFileName), FileMode.Create))
                        {
                            files[i].CopyTo(filesStream);
                        }

                        //add product Image for new product
                        ImageModel imgModel = new()
                        {
                            path = path0 + product.Symbol,
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
            else if (files.Count > 0 && product.ImageUrl == "")
            {
                    if (HttpContext.Request.Form.Files[0] != null)
                    {

                        string webRootPath = _webHostEnvironment.WebRootPath;

                    for (int i = 0; i <= files.Count; i++)
                    {
                        string path0 = "\\images\\produkty\\";

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
                            path = path0 + product.Symbol,
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

        private List<SelectListItem> GetSubCategories()
        {
            //var lstCategories = new List<SelectListItem>();

            //lstCategories = _ProductService.GetListCategory().Select(ct => new SelectListItem()
            //{
            //    Value = ct.CategoryId.ToString(),
            //    Text = ct.Name
            //}).ToList();

            //var dmyItem = new List<SelectListItem>();

            var lstCategories = new List<SelectListItem>();

            lstCategories = _context.SubCategory.ToList().OrderBy(x=> x.SubCategoryId).Select(di => new SelectListItem()
            {
                Value = di.SubCategoryId.ToString(),
                Text = di.Name
            }).ToList();


            //lstCategories.Insert(0, dmyItem );

            return lstCategories;
        }


    }
}