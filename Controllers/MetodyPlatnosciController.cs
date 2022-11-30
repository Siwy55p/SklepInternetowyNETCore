using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    public class MetodyPlatnosciController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly IMetodyPlatnosci _metodyPlatnosci;

        public MetodyPlatnosciController(ApplicationDbContext context, IMetodyPlatnosci metodyPlatnosci)
        {
            _context = context;
            _metodyPlatnosci = metodyPlatnosci;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<MetodyPlatnosci> metodyPlatnosci = _context.MetodyPlatnosci.ToList();
            return View(metodyPlatnosci);
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            MetodyPlatnosci metodaPlatnosci = new MetodyPlatnosci();
            return View(metodaPlatnosci);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MetodyPlatnosci metodaPlatnosci)
        {
            _context.MetodyPlatnosci.Add(metodaPlatnosci);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            MetodyPlatnosci metodaPlatnosci = _context.MetodyPlatnosci.Where(x => x.Id == id).FirstOrDefault();

            return View(metodaPlatnosci);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MetodyPlatnosci metodaPlatnosci)
        {
            _context.Update(metodaPlatnosci);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _metodyPlatnosci.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
