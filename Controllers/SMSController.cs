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
    public class SMSController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IImageService _imageService;

        private readonly ISliderService _sliderService;

        private readonly ISMS _sms;

        public SMSController(ISMS sms,ISliderService sliderService, IImageService imageService, IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
            _context = applicationDbContext;
            _sliderService = sliderService;
            _sms = sms;
        }

        public IActionResult Index()  // Lista wszystkich sms
        {
            List<SMS> smsy = _context.SMS.ToList();
            return View(smsy);
        }

        [HttpGet]
        public IActionResult Add()  // Lista wszystkich sms
        {
            SMS sms = new SMS();
            sms.apiKey = "NTA3MjQ4NzAzMzU5NjU1MDc2NDE1NTZkNmM0YjY5Mzg=";


            return View(sms);
        }

        [HttpPost]
        public IActionResult Add(SMS sms)  // Lista wszystkich sms
        {
            //add sms
            string message = _sms.sendSMS(sms.apiKey, sms.numbers, sms.message, sms.sender);
            //send sms

            return View(sms);
        }
    }
}
