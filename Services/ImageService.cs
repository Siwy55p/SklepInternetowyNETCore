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

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task UploadFilesAsync(IFormFileCollection files, Product? product = null, Slider? slider = null)
        {
            //var files = HttpContext.Request.Form.Files;

            Initialize(_webHostEnvironment);


            if (!IsInitialized)
                throw new InvalidOperationException("Object is not initialized");

            //var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            //ImageModel imgModel = new ImageModel();  // sprawdz czy istnieje.

            


            if (files.Count > 0) //To oznacza ze frontowy obrazek został dodany
            {
                string webRootPath = _hostingEnvironment.WebRootPath;

                for (int i = 0; i < files.Count; i++)
                {

                    ImageModel imgModel = new ImageModel();  //Dodanie do bazy rekodu w ktorym bedzie znajdował sie obraz
                    int IdImage = await AddAsync(imgModel);

                    imgModel = _context.Images.Find(IdImage);

                    //Save image to wwwroot/img
                    string path0 = @"img/";
                    if (product != null)
                    {
                        path0 = @"img/p/";
                    }
                    string IdImageString = IdImage.ToString();
                    char[] charArr = IdImageString.ToCharArray();

                    string Folders = "";
                    string uploadsFolder = Path.Combine(webRootPath, @"img/");
                    for(int c = 0; c < charArr.Length; c++)
                    {
                        Folders += charArr[c] + "/";
                    }
                    path0 += Folders;


                    if (slider != null)
                    {
                        path0 = @"img/SliderHome/" + slider.ImageSliderID;
                        //path0 = @"img/SliderHome/" + slider.ImageSliderID + @"/";
                    }
                    if (product != null)
                    {
                        uploadsFolder = Path.Combine(webRootPath, @"img/p/");

                        uploadsFolder += Folders;
                    }
                    if (slider != null)
                    {
                        uploadsFolder = Path.Combine(webRootPath, @"img/SliderHome/" + slider.ImageSliderID);
                    }

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    FileInfo fileInfo = new FileInfo(files[i].FileName);

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = IdImageString + extension;


                    //if (product != null)
                    //{
                    //    dynamicFileName = product.Symbol + "_" + i + "_" + DateTime.Now.ToString("mm_ss") + extension;
                    //}
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
                    string pathname = pathCompresImage +@"/"+ dynamicFileName;
                    string ImageNameCompres = "250x250_" + dynamicFileName;
                    string pathSaveCompres = pathCompresImage + @"/" + ImageNameCompres;
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

                    string pathCompresImage2 = Path.Combine(webRootPath, uploadsFolder);
                    string pathname2 = pathCompresImage2 + @"/" + dynamicFileName;
                    string ImageNameCompres2 = "645x410_" + dynamicFileName;
                    string pathSaveCompres2 = pathCompresImage2 + @"/" + ImageNameCompres2;
                    //save compres image
                    using (MagickImage image = new MagickImage(pathname2))
                    {
                        image.Format = MagickFormat.WebP; // Get or Set the format of the image.
                        image.Resize(645, 410); // fit the image into the requested width and height. 
                        image.Quality = 50; // This is the Compression level.
                        image.Write(pathSaveCompres2);
                    }





                    //add product Image for new product
                    if (product != null)
                    {
                        int ilosc = 0;
                        if (product.Product_Images.Count() > 0)
                        {
                            ilosc = product.Product_Images.Count();
                            ilosc++;
                        }

                        imgModel.path = path0;
                        imgModel.fullPath = path0 + dynamicFileName;
                        imgModel.kolejnosc = ilosc + i;
                        imgModel.Tytul = product.Name;
                        imgModel.ImageName = dynamicFileName;
                        imgModel.pathImageCompress250x250 = path0 + ImageNameCompres;
                        imgModel.ImageNameCompress250x250 = ImageNameCompres;
                        imgModel.pathImageCompress645x410 = path0 + ImageNameCompres2;
                        imgModel.ImageNameCompress645x410 = ImageNameCompres2;
                        imgModel.ProductId = product.ProductId;
                        imgModel.Opis = product.Symbol;
                    }
                    if (slider != null)
                    {
                        int ilosc = 0;
                        if (slider.ObrazkiDostepneWSliderze.Count() > 0)
                        {
                            ilosc = slider.ObrazkiDostepneWSliderze.Count();
                            ilosc++;
                        }

                        imgModel.path = path0 + @"/";
                        imgModel.fullPath = path0 + @"/" + dynamicFileName;
                        imgModel.kolejnosc = ilosc + i;
                        imgModel.Tytul = "sliderHome";
                        imgModel.ImageName = dynamicFileName;
                        imgModel.pathImageCompress250x250 = path0 + ImageNameCompres;
                        imgModel.ImageNameCompress250x250 = ImageNameCompres;
                        imgModel.pathImageCompress645x410 = path0 + ImageNameCompres2;
                        imgModel.ImageNameCompress645x410 = ImageNameCompres2;
                        imgModel.SliderIds = slider.ImageSliderID;
                        imgModel.Opis = "slider";
                    }

                    if(slider == null && product == null)
                    {

                        imgModel.path = path0;
                        imgModel.fullPath = path0 + dynamicFileName;
                        imgModel.kolejnosc = i;
                        imgModel.Tytul = "";
                        imgModel.ImageName = dynamicFileName;
                        imgModel.pathImageCompress250x250 = path0 + ImageNameCompres;
                        imgModel.ImageNameCompress250x250 = ImageNameCompres;
                        imgModel.pathImageCompress645x410 = path0 + ImageNameCompres2;
                        imgModel.ImageNameCompress645x410 = ImageNameCompres2;


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

                    _context.Images.Update(imgModel);

                    //await AddAsync(imgModel);
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

        public async Task<List<ImageModel>> ListImageAsync()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<int> AddAsync(ImageModel imgModel)
        {
            ImageModel imgExist = await _context.Images.Where(x => x.ImageName == imgModel.ImageName).FirstOrDefaultAsync();

            if (imgExist != null)
            {
                //To zamien i zrob update
                _context.Update(imgModel);
                await _context.SaveChangesAsync();

                return imgModel.ImageId;
            }
            else
            {
                imgModel.path = "";
                imgModel.fullPath = "";
                imgModel.ImageName = "";

                //Nieistenie wiec dodaj nowy rekord do bazy
                _context.Images.Add(imgModel);
                _context.SaveChanges();

                return imgModel.ImageId;
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
