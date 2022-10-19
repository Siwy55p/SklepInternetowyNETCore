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
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;


        private readonly IUnitOfWorkProduct _unitOfWorkProduct;
        private readonly IProductService _ProductService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext applicationDbContext, IProductService productService, IUnitOfWorkProduct unitOfWorkProduct, IWebHostEnvironment webHostEnvironment)
        {
            _ProductService = productService;
            _unitOfWorkProduct = unitOfWorkProduct;
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _ProductService.GetProductList());
        }

        [HttpGet]
        public async Task <IActionResult> Edit(int id)
        {
            ViewBag.Category = GetCategories();
            return View(await _ProductService.GetProductId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {

            if (id != product.ProductId)
            {
                return NotFound();
            }

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


        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Category = GetCategories();
            Product product = new Product();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            ViewBag.Category = GetCategories();

            product.DataDodania = DateTime.Now;

            product.Ukryty = false;
            product.Bestseller = true;
            product.ImageUrl = "";


            string uniqueFileName = UploadFile(product);
            product.ImageUrl = uniqueFileName;

            ModelState.Remove("CategoryNavigation");
            ModelState.Remove("product_Images");

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var id = _ProductService.AddProduct(product);//wazne aby przypisac

            UploadFile2(product);

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
        private void UploadFile2(Product product)
        {

            List<ImageModel> images = new List<ImageModel>();
            product.product_Images = images;


            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                foreach (var item in files)
                {

                    var uploads = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var extension = Path.GetExtension(item.FileName);
                    var dynamicFileName = DateTime.Now.ToString("yymmssfff") + "_" + extension;

                    using (var filesStream = new FileStream(Path.Combine(uploads, dynamicFileName), FileMode.Create))
                    {
                        item.CopyTo(filesStream);
                    }

                    //add product Image for new product
                    product.product_Images.Add(new ImageModel
                    {
                        Tytul = product.Name,
                        ImageName = dynamicFileName,
                        ProductId = product.ProductId
                    });
                }
            }

        }
        private string UploadFile(Product product)
        {
            string uniqueFileName = null;

            if (product.FrontImage != null)
            {
                ModelState.Remove("CategoryNavigation");
                ModelState.Remove("product_Images");

                if (ModelState.IsValid)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\produkty\\" + product.Symbol);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                }
                //uniqueFileName = "Front" + DateTime.Now.ToString("yymmssfff") + "_" + product.FrontImage.FileName;


                uniqueFileName = "Front" + DateTime.Now.ToString("yymmssfff");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        product.FrontImage.CopyTo(fileStream);
                    }


                }
            }

            return uniqueFileName;
        }

        private List<SelectListItem> GetCategories()
        {
            var lstCategories = new List<SelectListItem>();

            lstCategories = _ProductService.GetListCategory().Select(ct => new SelectListItem()
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