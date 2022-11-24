using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace partner_aluro.Controllers
{
    [Authorize]
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
            ViewData["produkty"] = await _context.Products.ToListAsync();
            Newsletter newsletter = new Newsletter();

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            return View(newsletter);
        }

        // POST: Newsletter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAsync(Newsletter newsletter)
        {

            ViewData["produkty"] = await _context.Products.ToListAsync();

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            _newsletter.Add(newsletter);


            return RedirectToAction(nameof(Index));

        }

        // GET: Newsletter/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ViewData["produkty"] = await _context.Products.ToListAsync();

            Newsletter newsletter = await _newsletter.GetAsync(id);

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            return View(newsletter);
        }

        // POST: Newsletter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Newsletter newsletter)
        {
            ViewData["produkty"] = await _context.Products.ToListAsync();

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

        static string body = "";


        static string tab1 = "";
        static string tab2 = "";
        static string tab3 = "";
        static string tab4 = "";
        static string tab5 = "";


        public string AddProductNewsletter(int ProduktId, int ile, string content)
        {
            Product produkt = _context.Products.Where(x => x.ProductId == ProduktId).FirstOrDefault();

            if(ile == 0)
            {

            }


            //string tresc = 
            //    $"<div class='row border=1'>" +
            //    "<div class='mojstyl'>"+
            //    $"<img src=../../images/produkty/"+produkt.Symbol + "/" + produkt.ImageUrl + " alt = " + produkt.Name + " style='width:200px;'>" +
            //    "</div>"+
            //    "<div class='col'>"+ produkt.Name+"</div>"+
            //    "<div class='col'>" + produkt.CenaProduktu + "</div>" +
            //    "<div class='col'>" + produkt.Symbol + "</div>" +
            //    "</div>";
            string tresc =
                $"<img src=../../images/produkty/" + produkt.Symbol + "/" + produkt.ImageUrl + " alt = " + produkt.Name + " style='width:200px;'>" +
                " " + produkt.Name + " "  + produkt.CenaProduktu + " " + produkt.Symbol + " " +
                " ";


            string td1 = "<th>  \"<img src=../../images/produkty/" + produkt.Symbol + "/" + produkt.ImageUrl + " alt = " + produkt.Name + " style='width:200px;'>\"  </th>\r\n\t";
            string td2 = "<td>" + produkt.Name + "</td>\r\n\t";


                thead = thead + td1;
                tbody = tbody + td2;
                tfoot = tfoot;


            string value = thead + tbody + tfoot;
            if (ile%3 == 0)
            {
                thead = cthead;
                tbody = ctbody;
                tfoot = ctfoot;
                tab1 += value;
                value = "";
            }

            string tabelka = "" +
                "<table \">\r\n\tTabela 1" +
                    "<thead>\r\n\t" +
                        "<tr>\r\n\t\t" +
                            td1+
                            "</tr>\r\n\t" +
                    "</thead>\r\n\t" +
                    "<tbody>\r\n\t" +
                        "<tr>\r\n\t\t" +
                            td2+
                        "</tr>\r\n\t" +
                    "</tbody>\r\n" +
                "</table>";



            return tab1 + value;
        }
        static string thead = "<table \">\r\n\tTabela 1" +
                    "<thead>\r\n\t" +
                        "<tr>\r\n\t\t";
        const string cthead = "<table \">\r\n\tTabela 1" +
                    "<thead>\r\n\t" +
                        "<tr>\r\n\t\t";

        static string tbody = "</tr>\r\n\t" +
                    "</thead>\r\n\t" +
                    "<tbody>\r\n\t" +
                        "<tr>\r\n\t\t";
        const string ctbody = "</tr>\r\n\t" +
                   "</thead>\r\n\t" +
                   "<tbody>\r\n\t" +
                       "<tr>\r\n\t\t";

        static string tfoot = "</ tr >\r\n\t" +
                    "</tbody>\r\n" +
                "</table>";
        const string ctfoot = "</ tr >\r\n\t" +
                    "</tbody>\r\n" +
                "</table>";


        public string tabelka()
        {
            string tab = thead + tbody + tfoot;
            return tab;
        }




        [HttpPost]
        [Route("upload")]
        public IActionResult Upload()
        {
            var fil = Request.Form.Files[0];


            var context = HttpContext.Features;

            string wwwRootPath = _hostEnvironment.WebRootPath;

            using (var fs = new FileStream(wwwRootPath + $"\\Images\\" + fil.FileName, FileMode.Create))
            {
                fil.CopyTo(fs);
            }

            return Ok(new { location = $"/Images/" + fil.FileName });
        }

        [HttpGet]
        [Route("filelist")]   //Do edytora laduje liste z Images tak jak jest w tej funkcji
        public IActionResult Filelist()
        {
            var context = HttpContext.Features;

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
