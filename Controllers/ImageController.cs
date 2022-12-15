using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using ImageMagick;
using System.IO;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public readonly IWebHostEnvironment _webHostEnvironment;


        public readonly IImageService _imageService;

        public ImageController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IImageService imageServer)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageServer;
        }


        // GET: Image
        public async Task<IActionResult> Index()
        {
            //List<ImageModel> lista1 = await _context.Images.ToListAsync();

            //for (int i = 0; i < lista2.Count(); i++)
            //{

            //    var img = _context.Images.Where(x => x.ImageId == lista2[i].ImageId).FirstOrDefault();
            //    if (img != null)
            //    { 
            //    _context.Images.Remove(img);
            //    _context.SaveChanges();
            //    }
            //}
            //List<ImageModel> lista = await _context.Images.ToListAsync();

            //for (int i = 0; i < lista.Count; i++)
            //{

            //    string sz = lista[i].path;
            //    if (sz[sz.Length - 1] == '\\')
            //    {
            //    }
            //    else
            //    {
            //        //lista[i].path = lista[i].path + '\\';
            //        ImageModel image = await _context.Images.Where(x => x.ImageId == lista[i].ImageId).FirstOrDefaultAsync();
            //        image.path = image.path + '\\';
            //        _context.Update(image);
            //        _context.SaveChanges();
            //    }

            //}
            //return View(lista2);


            //await _context.Images.OrderByDescending(x => x.ImageId).Take(1000).ToListAsync();

            //List<ImageModel> lista = _context.Images.TakeLast(100).ToList();
            return View(await _context.Images.OrderByDescending(x => x.ImageId).Take(1000).ToListAsync());
        }
        public async Task<IActionResult> GetFileServer()
        {
            //String path = Server.MapPath("~/images/"); // get the server path images folder


            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(wwwRootPath + @"/img/");

            String[] imagesfiles = Directory.GetFiles(path); //get all file from path
            ViewBag.images = imagesfiles;

            return View();
        }

        // GET: Image/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();
            var returnUrl = Request.Headers["Referer"].ToString();


            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // GET: Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageModel imageModel)
        {
            ModelState.Remove("Product");

            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.ImageName =  fileName = fileName +"_"+ DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + @"/img/", fileName); 
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                //insert record
                imageModel.path = @"img/";
                imageModel.fullPath = @"img/" + fileName;
                _context.Add(imageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }


        //usuniecie funkcji dodwanie glownego obrazku
        // POST: Product/CreateImageProductFront
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task <IActionResult> CreateImageProductFront(Product product)
        //{
        //    ModelState.Remove("Product");

        //    if (ModelState.IsValid)
        //    {
        //        //Sabe image to wwwroot/image
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        string fileName = Path.GetFileNameWithoutExtension(product.product_Image.ImageFile.FileName);
        //        string extension = Path.GetExtension(product.product_Image.ImageFile.FileName);
        //        product.product_Image.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                
        //        string path = Path.Combine(wwwRootPath + "/img/produkty", fileName);



        //        //save image in orginal format
        //        using (var fileStream = new FileStream(path, FileMode.Create))
        //        {
        //            await product.product_Image.ImageFile.CopyToAsync(fileStream);
        //        }

        //        //string imagePath = Path.Combine(wwwRootPath, "img");
        //        //string webPFileName = Path.GetFileNameWithoutExtension(product.product_Image.ImageFile.FileName) + ".webp";
        //        //string normalImagePath = Path.Combine(wwwRootPath,imagePath, fileName);
        //        //string webPImagePath = Path.Combine(wwwRootPath, imagePath, webPFileName);

        //        //using (var webPimageFileStream = new FileStream(webPFileName, FileMode.Create))
        //        //{
        //        //    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false)
        //        //    {
        //        //        imageFactory.Load(product.product_Image.ImageFile.FileName.OpenReadStream())
        //        //        .Format(new webPFortma())
        //        //        .Quality(100)
        //        //        .Save(webPFileName);
        //        //    }
        //        //}


        //        using (MagickImage image = new MagickImage(path))
        //        {
        //            image.Format = image.Format; // Get or Set the format of the image.
        //            image.Resize(40, 40); // fit the image into the requested width and height. 
        //            image.Quality = 100; // This is the Compression level.
        //            image.Write(path);
        //        }

        //        //using (MagickImage image = new MagickImage(@"YourImage.jpg"))
        //        //{
        //        //    image.Format = image.Format; // Get or Set the format of the image.
        //        //    image.Resize(40, 40); // fit the image into the requested width and height. 
        //        //    image.Quality = 10; // This is the Compression level.
        //        //    image.Write("YourFinalImage.jpg");
        //        //}


        //        //insert record

        //        //migajace
        //        //przenika



        //        _context.Add(product.product_Image);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Add", "Product", new { Product = product });
        //        //return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction("Add", "Product", new { Product = product });
        //    //return View(product.product_Image);
        //}

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        // GET: Image/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();
            var returnUrl = Request.Headers["Referer"].ToString();
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images.FindAsync(id);

            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }


        // POST: Image/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Edit(int id, ImageModel imageModel, string returnUrl)
        {

            //ViewData["returnUrl"] = Request.Headers["Referer"].ToString();
            //var returnUrl = Request.Headers["Referer"].ToString();
            if (id != imageModel.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var files = HttpContext.Request.Form.Files;

                    if (files.Count > 0 && imageModel.ImageFile != null) //To oznacza ze frontowy obrazek został dodany
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        for (int i = 0; i < files.Count; i++)
                        {
                            //Save image to wwwroot/image
                            string path0 = imageModel.path;
                            var uploadsFolder = Path.Combine(webRootPath, imageModel.path);

                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            var extension = Path.GetExtension(files[i].FileName);
                            var dynamicFileName = imageModel.ImageName;

                            using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
                            {
                                files[i].CopyTo(filesStream);
                            }



                            string pathCompresImage = webRootPath + @"/" + path0;
                            string pathname = pathCompresImage + @"/" + dynamicFileName;
                            string ImageNameCompres = "250x250_" + dynamicFileName;
                            string pathSaveCompres = pathCompresImage + @"/" + ImageNameCompres;

                            string path1 = path0 + ImageNameCompres;
                            //save compres image
                            using (MagickImage image = new MagickImage(pathname))
                            {
                                image.Format = MagickFormat.WebP; // Get or Set the format of the image.
                                image.Resize(250, 250); // fit the image into the requested width and height. 
                                image.Quality = 50; // This is the Compression level.
                                image.Write(pathSaveCompres);
                            }

                            imageModel = _context.Images.Where(x => x.ImageId == imageModel.ImageId).FirstOrDefault();


                                imageModel.ImageNameCompress250x250 = ImageNameCompres;


                                imageModel.pathImageCompress250x250 = path1;


                            _imageService.Update(imageModel);
                        }
                        return Redirect(returnUrl);
                    }

                    _imageService.Update(imageModel);

                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(imageModel.ImageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(returnUrl);
                //return RedirectToAction(nameof(Index));
            }
            return Redirect(returnUrl);
            //return View(imageModel);
        }

        //// GET: Image/Delete/5
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();
            var returnUrl = Request.Headers["Referer"].ToString();

            var imageModel = await _context.Images.FindAsync(id);

            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            //var imageModel = await _context.Images
            //    .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }


            return View(imageModel);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            var imageModel = await _context.Images.FindAsync(id);

            //delete image from wwwroot/img
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageModel.path, imageModel.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            //delete tge record




            if (_context.Images == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Images'  is null.");
            }
            if (imageModel != null)
            {
                _context.Images.Remove(imageModel);

                if (imageModel == null)
                {
                    return NotFound();
                }

                string webRootPath = _webHostEnvironment.WebRootPath;

                if (imageModel.path != null)
                {
                    if (imageModel.fullPath != null)
                    {
                        string ExitingFile = Path.Combine(webRootPath, imageModel.path, imageModel.ImageName);
                        System.IO.File.Delete(ExitingFile);
                    }
                    _context.Images.Remove(imageModel);
                    _context.SaveChanges();
                }


            }
            return Redirect(returnUrl);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete2(int ImageId, string returnUrl)
        {
            var imageModel = _context.Images.Find(ImageId);

            if (imageModel != null)
            {
                //delete image from wwwroot/img
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageModel.path, imageModel.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //delete the record

                if (imageModel != null)
                {
                    _context.Images.Remove(imageModel);

                    string webRootPath = _webHostEnvironment.WebRootPath;

                    if (imageModel.path != null)
                    {
                        if (imageModel.fullPath != null)
                        {
                            string ExitingFile = Path.Combine(webRootPath, imageModel.path, imageModel.ImageName);
                            System.IO.File.Delete(ExitingFile);
                        }
                        _context.Images.Remove(imageModel);
                        _context.SaveChanges();
                        return Redirect(returnUrl);
                    }
                }
            }
            return Redirect(returnUrl);
        }



        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //public static ImageModel UploadFiles(IFormFileCollection files, Product? product = null)
        //{
        //    //var files = HttpContext.Request.Form.Files;


        //    if (!IsInitialized)
        //        throw new InvalidOperationException("Object is not initialized");

        //    //var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        //    ImageModel imgModel = new ImageModel();  // sprawdz czy istnieje.

        //    if (files.Count > 0) //To oznacza ze frontowy obrazek został dodany
        //    {
        //        string webRootPath = _hostingEnvironment.WebRootPath;

        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            //Save image to wwwroot/image
        //            string path0 = "img\\p\\";

        //            var uploadsFolder = Path.Combine(webRootPath, "img\\");
        //            if (product != null)
        //            {
        //                uploadsFolder = Path.Combine(webRootPath, "img\\p\\" + product.Symbol);
        //            }


        //            if (!Directory.Exists(uploadsFolder))
        //            {
        //                Directory.CreateDirectory(uploadsFolder);
        //            }

        //            FileInfo fileInfo = new FileInfo(files[i].FileName);

        //            var extension = Path.GetExtension(files[i].FileName);
        //            var dynamicFileName = fileInfo.Name + extension;


        //            if (product != null)
        //            {
        //                dynamicFileName = product.Symbol + "_" + i + "_" + extension;
        //            }


        //            using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
        //            {
        //                files[i].CopyTo(filesStream);
        //            }

        //            //add product Image for new product
        //            imgModel = new()
        //            {
        //                path = path0 + product.Symbol + "\\",
        //                fullPath = path0 + product.Symbol + "\\" + dynamicFileName,
        //                kolejnosc = i,
        //                Tytul = product.Name,
        //                ImageName = dynamicFileName,
        //                //ProductId = product.ProductId
        //            };

        //            if (product != null)
        //            {
        //                product.Product_Images.Add(imgModel);
        //            }

        //            //_imageService.AddAsync(imgModel);
        //        }
        //        return imgModel;
        //    }

        //    imgModel = new()
        //    {
        //        path = "img\\",
        //        fullPath = "\\img\\",
        //        kolejnosc = -1,
        //        Tytul = "",
        //        ImageName = ".jpg",
        //        //ProductId = product.ProductId
        //    };
        //    return imgModel;



        //}



        private bool ImageModelExists(int id)
        {
          return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
