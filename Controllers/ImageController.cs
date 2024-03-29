﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using ImageMagick;

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

            return View(await _context.Images.AsNoTracking().OrderByDescending(x => x.ImageId).Take(1000).ToListAsync());
        }

        public IActionResult zmianaNazwy2()
        {


            List<ImageModel> listaWszystkich = _context.Images.ToList();

            string webRootPath = _webHostEnvironment.WebRootPath;

            for (int i = 5446; i < 5459; i++)
            {
                ImageModel img = listaWszystkich[i];

                string pathPrawidlowy = img.pathImageCompress645x410;
                string sciezkaDobra = pathPrawidlowy.Replace("645x410_", "");

                img.path = sciezkaDobra;


                string ImageName1 = img.ImageNameCompress645x410;
                string PrawidloweImageName = ImageName1.Replace("645x410_", "");
                img.ImageName = PrawidloweImageName;

                img.fullPath = sciezkaDobra;

                string name = PrawidloweImageName;
                img.path = img.path.Replace(name, "");
                _context.Images.Update(img);
                _context.SaveChanges();
            }



            //List<Product> products = _context.Products.ToList();
            //for (int i = 0; i < products.Count(); i++)
            //{
            //    int ImageId = products[i].ProductImagesId;
            //    if (_context.Images.Where(x => x.ImageId == ImageId).FirstOrDefault() != null)
            //    {
            //        products[i].pathImageUrl250x250 = _context.Images.Where(x => x.ImageId == ImageId).FirstOrDefault().pathImageCompress250x250;
            //    }
            //}



            //List<Product> products = _context.Products.ToList();
            //for (int i = 0; i < products.Count(); i++)
            //{
            //    string nazwaObrazka = products[i].ImageUrl;
            //    if (_context.Images.Where(x => x.ImageName == nazwaObrazka).FirstOrDefault() != null)
            //    {
            //        products[i].ProductImagesId = _context.Images.Where(x => x.ImageName == nazwaObrazka).FirstOrDefault().ImageId;
            //        products[i].pathImageUrl250x250 = _context.Images.Where(x => x.ImageName == nazwaObrazka).FirstOrDefault().pathImageCompress250x250;
            //    }
            //}

            return View();

        }

            public IActionResult zmianaNazwy()
        {




            //List<Product> products = _context.Products.ToList();
            //for(int i = 99; i < products.Count(); i++)
            //{
            //    string nazwaObrazka = products[i].ImageUrl;
            //    if (_context.Images.Where(x => x.ImageName == nazwaObrazka).FirstOrDefault() != null)
            //    {
            //        products[i].ProductImagesId = _context.Images.Where(x => x.ImageName == nazwaObrazka).FirstOrDefault().ImageId;
            //        products[i].pathImageUrl250x250 = _context.Images.Where(x => x.ImageName == nazwaObrazka).FirstOrDefault().pathImageCompress250x250;
            //    }
            //}


            List<ImageModel> listaWszystkich = _context.Images.ToList();

            string webRootPath = _webHostEnvironment.WebRootPath;

                for (int i = 5361; i < 5596; i++)
                {
                    ImageModel img = listaWszystkich[i];

                    int IdImage = img.ImageId;

                    string slider = img.path;

                    if (!slider.Contains("SliderHome")) // jesli to nie jest slider to rob to co na dole
                    {


                        string IdImageString = IdImage.ToString();
                        char[] charArr = IdImageString.ToCharArray();

                        string path = img.path.ToString();
                        string path0 = img.path.ToString();

                        _context.Images.Update(img);
                        _context.SaveChanges();

                        string sciezka2 = img.pathImageCompress645x410.ToString();
                        string staraSciezka = sciezka2.Replace("645x410_", ""); //path pliku

                        string sourceFile = Path.Combine(webRootPath, staraSciezka);
                
                        string destinationFile = Path.Combine(webRootPath,path0, img.ImageName);

                        img.fullPath = path0 + img.ImageName;

                

                        if (System.IO.File.Exists(sourceFile))
                        {
                            // To move a file or folder to a new location:
                            System.IO.File.Move(sourceFile, destinationFile);
                        }
                        //// To move an entire directory. To programmatically modify or combine
                        //// path strings, use the System.IO.Path class.
                        //System.IO.Directory.Move(@"C:\Users\Public\public\test\", @"C:\Users\Public\private");


                        //string sourceFile2 = Path.Combine(webRootPath, img.pathImageCompress250x250);

                        //img.ImageNameCompress250x250 = "250x250_" + IdImageString + ".jpg";
                        //string destinationFile2 = Path.Combine(uploadsFolder, img.ImageNameCompress250x250);


                        //if (System.IO.File.Exists(sourceFile2))
                        //{
                        //    // To move a file or folder to a new location:
                        //    System.IO.File.Move(sourceFile2, destinationFile2);

                        //    img.pathImageCompress250x250 = path0 + img.ImageNameCompress250x250;
                        //}

                    }


                }

            return View();
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
        public IActionResult Edit(int id, ImageModel imageModel, string returnUrl)
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
            ViewData["returnUrl"] = Request.Headers["Referer"].ToString();
            returnUrl = Request.Headers["Referer"].ToString();

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



        private bool ImageModelExists(int id)
        {
          return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
