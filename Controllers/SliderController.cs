using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    public class SliderController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IImageService _imageService;


        public SliderController(IImageService imageService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }

        public IActionResult Index()  // Lista wszystkich sliderow
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            Slider slider = new Slider();

            return View(slider);
        }


        [HttpPost]
        public IActionResult Add(Slider slider)
        {
            UploadFile2Async(slider);

            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        private Task UploadFile2Async(Slider slid)
        {
            var files = HttpContext.Request.Form.Files;

                string webRootPath = _webHostEnvironment.WebRootPath;

                for (int i = 1; i <= files.Count; i++)
                {
                    //Save image to wwwroot/image
                    string path0 = "images\\SliderHome\\";
                    var uploadsFolder = Path.Combine(webRootPath, "images\\SliderHome" );

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = "slider" + i + extension;

                    using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
                    {
                        files[i].CopyTo(filesStream);
                    }

                    //add product Image for new product
                    ImageModel imgModel = new()
                    {
                        path = path0 + dynamicFileName,
                        kolejnosc = i,
                        Tytul = "sliderHome",
                        ImageName = dynamicFileName
                    };

                    _imageService.Add(imgModel);

                }

            if (files.Count > 0 ) //Fronyowy obrazek nie został dodany zacznij dodawac od 0
            {

                webRootPath = _webHostEnvironment.WebRootPath;

                for (int i = 0; i <= files.Count; i++)
                {
                    string path0 = "images\\SliderHome\\";
                    var uploads = Path.Combine(webRootPath, "images\\SliderHome");

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = "slider" + i + extension;

                    using (var filesStream = new FileStream(Path.Combine(uploads, dynamicFileName), FileMode.Create))
                    {
                        files[i].CopyTo(filesStream);
                    }


                    //add product Image for new product
                    ImageModel imgModel = new()
                    {
                        path = path0 + dynamicFileName,
                        kolejnosc = i,
                        Tytul = "sliderHome",
                        ImageName = dynamicFileName
                    };

                    _imageService.Add(imgModel);
                }
            }

            return Task.CompletedTask;
        }


    }

   
}
