using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    public class ProfilDzialalnosciController : Controller
    {
        private readonly IProfildzialalnosciService _profildzialalnosciService;

        public ProfilDzialalnosciController(IProfildzialalnosciService profildzialalnosciService)
        {
            _profildzialalnosciService = profildzialalnosciService;
        }

        

        // GET: ProfilDzialalnosciController
        public ActionResult Index()
        {
            List<ProfilDzialalnosci> lista = _profildzialalnosciService.GetListAllProfils();

            return View(lista);
        }

        // GET: ProfilDzialalnosciController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: ProfilDzialalnosciController/Create
        public ActionResult Create()
        {
            ProfilDzialalnosci profilDzialalnosci = new ProfilDzialalnosci();

            return View(profilDzialalnosci);
        }

        // POST: ProfilDzialalnosciController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProfilDzialalnosci profilDzialalnosci)
        {
            if(ModelState.IsValid)
            {
                return View(profilDzialalnosci);
            }

            _profildzialalnosciService.Create(profilDzialalnosci);

            return RedirectToAction(nameof(Index));
        }

        // GET: ProfilDzialalnosciController/Edit/5
        public ActionResult Edit(int id)
        {
            ProfilDzialalnosci profilDzialalnosci = _profildzialalnosciService.GetProfil(id);
            return View(profilDzialalnosci);
        }

        // POST: ProfilDzialalnosciController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProfilDzialalnosci profilDzialalnosci)
        {
            if (ModelState.IsValid)
            {
                return View(profilDzialalnosci);
            }

            _profildzialalnosciService.Update(profilDzialalnosci);

            return RedirectToAction(nameof(Index));
        }

        // GET: ProfilDzialalnosciController/Delete/5
        public ActionResult Delete(int id)
        {
            ProfilDzialalnosci profilDzialalnosci = _profildzialalnosciService.GetProfil(id);
            _profildzialalnosciService.Delete(profilDzialalnosci);
            return RedirectToAction(nameof(Index));
        }

    }
}
