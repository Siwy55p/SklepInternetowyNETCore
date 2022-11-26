using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Migrations;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using System.Data;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class SliderController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IImageService _imageService;

        private readonly ISliderService _sliderService;


        public SliderController(ISliderService sliderService, IImageService imageService, IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
            _context = applicationDbContext;
            _sliderService = sliderService;
        }

        public IActionResult Index()  // Lista wszystkich sliderow
        {
            List<Slider> slidery = _context.Sliders.ToList();
            return View(slidery);
        }
        [HttpGet]
        public IActionResult Add()
        {
            Slider slider = new Slider();

            return View(slider);
        }


        [HttpPost]
        public async Task<IActionResult> AddAsync(Slider slider)
        {

            if (ModelState.IsValid)
            {
                _sliderService.AddSlider(slider);
            }
            else
            {
                return View();
            }

            //Slider sliders =await _sliderService.GetAsync(slider.ImageSliderID);

            UploadFile2Async(slider);

            await _sliderService.EditSliderAsync(slider);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Slider sliders = await _sliderService.GetAsync(id);

            return View(sliders);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            _sliderService.DeleteSlider(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            UploadFile2Async(slider);

            await _sliderService.EditSliderAsync(slider);


            return RedirectToAction("Index");
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        private Task UploadFile2Async(Slider slid)
        {
            var files = HttpContext.Request.Form.Files;

                string webRootPath = _webHostEnvironment.WebRootPath;
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    //Save image to wwwroot/image
                    string path0 = "images\\SliderHome\\" + slid.ImageSliderID +"\\";
                    var uploadsFolder = Path.Combine(webRootPath, "images\\SliderHome\\"+ slid.ImageSliderID);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    FileInfo fileInfo = new FileInfo(files[i].FileName);

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = "slider" + i +"_"+ DateTime.Now.ToString("mm_ss") + extension;

                    using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
                    {
                        files[i].CopyTo(filesStream);
                    }

                    //add product Image for new product
                    ImageModel imgModel = new()
                    {
                        path = path0,
                        fullPath = path0 + dynamicFileName,
                        kolejnosc = i,
                        Tytul = "sliderHome",
                        SliderIds = slid.ImageSliderID,
                        Opis = "slider",
                        ImageName = dynamicFileName,
                        
                        
                    };

                    slid.ObrazkiDostepneWSliderze.Add(imgModel);

                    _imageService.AddAsync(imgModel);

                }

            }
            return Task.CompletedTask;
        }


    }

   
}
