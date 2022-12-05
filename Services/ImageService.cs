using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NuGet.Protocol.Core.Types;
using partner_aluro.Controllers;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using Polly;
using System.Linq;

namespace partner_aluro.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private static IWebHostEnvironment _hostingEnvironment;

        public ImageService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public static bool IsInitialized { get; private set; }
        public static void Initialize(IWebHostEnvironment hostEnvironment)
        {
            if (IsInitialized)
            {
                //throw new InvalidOperationException("Object already initialized");
                
            }

            _hostingEnvironment = hostEnvironment;
            IsInitialized = true;
        }


        //Obrazek prezentujacy
        //public async Task<string> CreateImageAddAsync(ImageModel imageModel)
        //{
        //    string komunikat = "1";

        //    if (imageModel.ImageFile != null)
        //    {
        //        //Save image to wwwroot/image
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
        //        string extension = Path.GetExtension(imageModel.ImageFile.FileName);
        //        imageModel.ImageName = fileName + extension;
        //        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
        //        using (var fileStream = new FileStream(path, FileMode.Create))
        //        {
        //            await imageModel.ImageFile.CopyToAsync(fileStream);
        //        }
        //        //insert record
        //        imageModel.fullPath = path + "\\" + fileName + extension;
        //        imageModel.path = path;



        //        var image = _context.Images.Where(x => x.ImageName == imageModel.ImageName).FirstOrDefault();
        //        if (image != null)
        //        {
        //            _context.Images.Update(image);
        //        }
        //        else
        //        {
        //            _context.Add(imageModel);
        //            await _context.SaveChangesAsync();
        //        }

        //        return wwwRootPath;
        //    }

        //    return komunikat;
        //}



        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task UploadFilesAsync(IFormFileCollection files, Product? product = null, Slider? slider = null)
        {
            //var files = HttpContext.Request.Form.Files;

            Initialize(_webHostEnvironment);


            if (!IsInitialized)
                throw new InvalidOperationException("Object is not initialized");

            //var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            ImageModel imgModel = new ImageModel();  // sprawdz czy istnieje.

            if (files.Count > 0) //To oznacza ze frontowy obrazek został dodany
            {
                string webRootPath = _hostingEnvironment.WebRootPath;

                for (int i = 0; i < files.Count; i++)
                {
                    //Save image to wwwroot/image
                    string path0 = "images\\";
                    if (product != null)
                    {
                        path0 = "images\\produkty\\";
                    }
                    if (slider != null)
                    {
                        path0 = "images\\SliderHome\\" + slider.ImageSliderID + "\\";
                    }

                    var uploadsFolder = Path.Combine(webRootPath, "images\\");
                    if (product != null)
                    {
                        uploadsFolder = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);
                    }
                    if (slider != null)
                    {
                        uploadsFolder = Path.Combine(webRootPath, "images\\SliderHome\\" + slider.ImageSliderID);
                    }

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    FileInfo fileInfo = new FileInfo(files[i].FileName);

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = fileInfo.Name + extension;


                    if (product != null)
                    {
                        dynamicFileName = product.Symbol + "_" + i + "_" + DateTime.Now.ToString("mm_ss") + extension;
                    }
                    if (slider != null)
                    {
                        dynamicFileName = "slider" + i + "_" + DateTime.Now.ToString("mm_ss") + extension;
                    }

                    //save orginal image
                    using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
                    {
                        files[i].CopyTo(filesStream);
                    }


                    string pathCompresImage = Path.Combine(webRootPath, uploadsFolder);
                    string pathname = pathCompresImage +"\\"+ dynamicFileName;
                    string ImageNameCompres = "250x250_" + dynamicFileName;
                    string pathSaveCompres = pathCompresImage + "\\" + ImageNameCompres;
                    //save compres image
                    using (MagickImage image = new MagickImage(pathname))
                    {
                        image.Format = MagickFormat.WebP; // Get or Set the format of the image.
                        image.Resize(250, 250); // fit the image into the requested width and height. 
                        image.Quality = 50; // This is the Compression level.
                        image.Write(pathSaveCompres);
                    }

                    //        //using (MagickImage image = new MagickImage(@"YourImage.jpg"))
                    //        //{
                    //        //    image.Format = image.Format; // Get or Set the format of the image.
                    //        //    image.Resize(40, 40); // fit the image into the requested width and height. 
                    //        //    image.Quality = 10; // This is the Compression level.
                    //        //    image.Write("YourFinalImage.jpg");
                    //        //}






                    //add product Image for new product
                    if (product != null)
                    {
                        imgModel = new()
                        {
                            path = path0 + product.Symbol + "\\",
                            fullPath = path0 + product.Symbol + "\\" + dynamicFileName,
                            kolejnosc = i,
                            Tytul = product.Name,
                            ImageName = dynamicFileName,
                            pathImageCompress250x250 = pathSaveCompres,
                            ImageNameCompress250x250 = ImageNameCompres,
                            ProductId = product.ProductId
                        };
                    }
                    if (slider != null)
                    {
                        imgModel = new()
                        {
                            path = path0 +"\\",
                            fullPath = path0 + "\\" + dynamicFileName,
                            kolejnosc = i,
                            Tytul = "sliderHome",
                            ImageName = dynamicFileName,
                            pathImageCompress250x250 = pathSaveCompres,
                            ImageNameCompress250x250 = ImageNameCompres,
                            SliderIds = slider.ImageSliderID,
                            Opis = "slider",
                        };
                    }

                    if (product != null)
                    {
                        product.Product_Images.Add(imgModel);
                    }
                    if (slider != null)
                    {
                        slider.ObrazkiDostepneWSliderze.Add(imgModel);
                        //product.Product_Images.Add(imgModel);
                    }

                    await AddAsync(imgModel);
                }

            }

            //imgModel = new()
            //{
            //    path = "images\\",
            //    fullPath = "\\images\\",
            //    kolejnosc = -1,
            //    Tytul = "",
            //    ImageName = ".jpg",
            //    //ProductId = product.ProductId
            //};
            //await AddAsync(imgModel);



        }


        public async Task<string> DeleteFrontImage(Product product) //Dodaj obrazek Front przy dodawaniu produktu
        {
            var imageModel = await _context.Images.FirstOrDefaultAsync(x => x.ImageName == product.ImageUrl);


            if (imageModel != null && product.ImageUrl != null)
            {
                //delete image from wwwroot/images
                var imagePath = Path.Combine(imageModel.path, imageModel.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //delete tge record
            }

            if (imageModel != null)
            {
                _context.Images.Attach(imageModel);
                _context.Images.Remove(imageModel);
            }

            await _context.SaveChangesAsync();

            return "";
        }


        //public async Task<string> CreateImageAddAsync(Product product) //Dodaj obrazek Front przy dodawaniu produktu
        //{
        //    string uniqueFileName = "";

        //    if (product.product_Image.ImageFile != null)
        //    {

        //        //Save image to wwwroot/image
        //        string webRootPath = _webHostEnvironment.WebRootPath;
        //        string path0 = "images\\produkty\\";
        //        var uploadsFolder = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);

        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        var extension = Path.GetExtension(product.product_Image.ImageFile.FileName);

        //        uniqueFileName = "Front_" + product.Symbol + extension;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);


        //        string fileName = Path.GetFileNameWithoutExtension(product.product_Image.ImageFile.FileName);
        //        //string extension = Path.GetExtension(product.product_Image.ImageFile.FileName);
        //        //product.product_Image.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        //        //string path = Path.Combine(wwwRootPath + "/Images/", fileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await product.product_Image.ImageFile.CopyToAsync(fileStream);
        //        }
        //        //insert record
        //        product.product_Image.path = path0 + product.Symbol +"\\";
        //        product.product_Image.kolejnosc = 0;
        //        product.product_Image.ProductId = product.ProductId;
        //        product.product_Image.Tytul = product.Name;
        //        product.product_Image.ProductImagesId = product.ProductId;
        //        product.product_Image.ImageName = uniqueFileName;

        //        product.product_Image.fullPath = path0 + product.Symbol + "\\" + uniqueFileName;


        //        var image = _context.Images.Where(x => x.ImageName == product.product_Image.ImageName).FirstOrDefault();
        //        if (image != null)
        //        {
        //            _context.Images.Update(image);
        //        }
        //        else
        //        {
        //            _context.Add(product.product_Image);
        //            await _context.SaveChangesAsync();
        //        }


        //        return uniqueFileName;
        //    }

        //    return uniqueFileName;
        //}

        public async Task<List<ImageModel>> ListImageAsync()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task AddAsync(ImageModel imgModel)
        {
            ImageModel imgExist = await _context.Images.Where(x => x.ImageName == imgModel.ImageName).FirstOrDefaultAsync();

            if (imgExist != null)
            {
                //To zamien i zrob update
                _context.Update(imgModel);
                await _context.SaveChangesAsync();
            }
            else
            {
                //Nieistenie wiec dodaj nowy rekord do bazy
                _context.Images.Add(imgModel);
                _context.SaveChanges();
            }
        }

        public async Task Edit(int id, ImageModel imageModel)
        {
            if (id != imageModel.ImageId)
            {
                try
                {
                    _context.Update(imageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
        }

        public ImageModel Get(string name)
        {
            var image = _context.Images.Where(x => x.ImageName == name).FirstOrDefault();
            if (image != null)
            {
                return image;
            }
            else
            {
                return null;
            }
        }

        public void Update(ImageModel imageModel)
        {
            //var content = _context.Images.Where(x => x.ImageId == imageModel.ImageId).FirstOrDefault();
            //if( content != null)
            //{
                _context.Images.Update(imageModel);
                _context.SaveChanges();
            //}
        }

    }
}
