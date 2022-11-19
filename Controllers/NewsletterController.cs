using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace partner_aluro.Controllers
{

    public class NewsletterController : Controller
    {
        public readonly INewsletter _newsletter;

        public readonly ApplicationDbContext _context;

        public readonly IEmailService _emailService;



        public readonly IWebHostEnvironment _hostEnvironment;

        public NewsletterController(INewsletter newsletter, ApplicationDbContext context, IEmailService emailService, IWebHostEnvironment hostEnvironment)
        {
            _newsletter = newsletter;
            _context = context;
            _emailService = emailService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Newsletter
        public async Task<ActionResult> Index()
        {
            var listaNewsletter =  await _context.Newsletter.ToListAsync();
            return View(listaNewsletter);

            
        }

        // GET: Newsletter/Create
        public async Task<ActionResult> Add()
        {
            Newsletter newsletter = new Newsletter();

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            return View(newsletter);
        }

        // POST: Newsletter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAsync(Newsletter newsletter)
        {

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            _newsletter.Add(newsletter);


            return RedirectToAction(nameof(Index));

        }

        // GET: Newsletter/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Newsletter newsletter = await _newsletter.GetAsync(id);

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            return View(newsletter);
        }

        // POST: Newsletter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Newsletter newsletter)
        {

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            _newsletter.Edit(newsletter);

            return RedirectToAction(nameof(Index));
        }

        // GET: Newsletter/Delete/5
        public ActionResult Delete(int id)
        {
            _newsletter.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> SendEmail(int id)
        {
            Newsletter newsletter = await _newsletter.GetAsync(id);

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            EmailDto emailDto = new EmailDto()
            {
                Body = newsletter.MessagerBody,
                To = "szuminski.p@gmail.com",
                Subject = "Newsletter"
            };

            await _emailService.SendEmailAsync(emailDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload()
        {
            var fil = Request.Form.Files[0];

            string wwwRootPath = _hostEnvironment.WebRootPath;

            using (var fs = new FileStream(wwwRootPath + $"\\Images\\" + fil.FileName, FileMode.Create))
            {
                fil.CopyTo(fs);
            }

            return Ok(new { location = $"/Images/" + fil.FileName });
        }

        [HttpGet]
        [Route("filelist")]
        public IActionResult Filelist()
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var files = Directory.GetFiles(wwwRootPath + $"\\Images");

            List<Fileinf> res = new List<Fileinf>();
            foreach (var item in files)
            {
                res.Add(new Fileinf() { title = Path.GetFileName(item), value = $"/Images/" + Path.GetFileName(item) });
            }
            return Json(res);
        }
    }

    internal class Fileinf
    {
        public Fileinf()
        {
        }

        public string title { get; set; }
        public string value { get; set; }
    }
}
