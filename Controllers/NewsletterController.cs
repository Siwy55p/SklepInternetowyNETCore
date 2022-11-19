using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{

    public class NewsletterController : Controller
    {
        public readonly INewsletter _newsletter;

        public readonly ApplicationDbContext _context;

        public NewsletterController(INewsletter newsletter, ApplicationDbContext context)
        {
            _newsletter = newsletter;
            _context = context;
        }

        // GET: Newsletter
        public async Task<ActionResult> Index()
        {
            var listaNewsletter =  await _context.Newsletter.ToListAsync();
            return View(listaNewsletter);

            
        }

        // GET: Newsletter/Create
        public ActionResult Add()
        {
            Newsletter newsletter = new Newsletter();

            return View(newsletter);
        }

        // POST: Newsletter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(IFormCollection collection)
        {
            Newsletter newsletter = new Newsletter();

            return View(newsletter);

        }

        // GET: Newsletter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Newsletter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Newsletter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Newsletter/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
