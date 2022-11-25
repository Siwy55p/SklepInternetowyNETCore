using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    public class SettingController : Controller
    {

        private readonly ISetting _setting;

        private readonly ApplicationDbContext _context;

        public SettingController(ISetting setting, ApplicationDbContext context)
        {
            _setting = setting;
            _context = context;
        }   

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Edit");
        }


        [HttpGet]
        public IActionResult Add()
        {
            Setting setting = new Setting();


            return View(setting);
        }

        [HttpPost]
        public IActionResult Add(Setting setting)
        {
            _setting.AddSetting(setting);

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id=1)
        {
            ViewData["Slider"] = GetSliders();

            Setting setting = await _context.Setting.Where(x => x.SettingID == id).FirstOrDefaultAsync();
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id , Setting setting)
        {
            ViewData["Slider"] = GetSliders();
            //Setting settings = await _context.Setting.Where(x => x.SettingID == id).FirstOrDefaultAsync();
            _context.Update(setting);
            await _context.SaveChangesAsync();


            //_setting.EditSettingAsync(setting); //Update

            return View(setting);
        }


        private List<SelectListItem> GetSliders()
        {
            var SlidersList = new List<SelectListItem>();


            SlidersList = _context.Sliders.Select(ct => new SelectListItem()
            {
                Value = ct.ImageSliderID.ToString(),
                Text = ct.Name
            }).ToList();

            return SlidersList;
        }

    }
}
