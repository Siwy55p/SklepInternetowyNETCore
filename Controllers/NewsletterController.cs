﻿using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
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
            ViewData["kategorie"] = await _context.Category.ToListAsync();
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
            ViewData["kategorie"] = await _context.Category.ToListAsync();

            newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();

            _newsletter.Add(newsletter);


            return RedirectToAction(nameof(Index));

        }

        // GET: Newsletter/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ViewData["produkty"] = await _context.Products.ToListAsync();
            ViewData["kategorie"] = await _context.Category.ToListAsync();

            Newsletter newsletter = await _newsletter.GetAsync(id);


            ViewData["active1"] = "active";
            ViewData["active2"] = "";

            tab1 = "";

            thead = cthead;
            tbody = ctbody;
            tbody2 = ctbody2;
            tfoot = ctfoot;

            value = "";

            tabelka2 = "" +
                "<table style=\"padding: 0px; margin-left: auto; margin-right: auto; displey:table;\" >" +
                    "<thead>\r\n\t" +
                        "<tr>\r\n\t\t" +
                            "</tr>\r\n\t" +
                    "</thead>\r\n\t" +
                    "<tbody>\r\n\t" +
                        "<tr>\r\n\t\t" +
                        "</tr>\r\n\t" +
                        "<tr>\r\n\t\t" +
                        "</tr>\r\n\t" +
                    "</tfoot>\r\n" +
                "</table>";




            return View(newsletter);
        }

        // POST: Newsletter/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Newsletter newsletter)
        {
            ViewData["produkty"] = await _context.Products.ToListAsync();
            ViewData["kategorie"] = await _context.Category.ToListAsync();


            ViewData["active1"] = "";
            ViewData["active2"] = "active";

            Newsletter newsletter1 = _context.Newsletter.Where(x => x.NewsletterID == newsletter.NewsletterID).FirstOrDefault();

            newsletter1.MessagerBody = newsletter.MessagerBody;

            //ViewData["BodyProduct"] = newsletter.MessagerBody;

            _newsletter.Edit(newsletter1);


            return View(newsletter1);

        }

        [HttpGet]
        public async Task<ActionResult> Newsletter(int id)
        {
            Newsletter newsletter = _context.Newsletter.Where(x => x.NewsletterID == id).FirstOrDefault();

            return View(newsletter);
        }

        [HttpPost]
        public async Task<ActionResult> NewsletterEdit(Newsletter newsletter)
        {

            Newsletter newsletterDB = await _newsletter.GetAsync(newsletter.NewsletterID);
            newsletterDB.contentEmail = newsletter.contentEmail;

            newsletterDB.MessagerBody = "";
            _newsletter.Edit(newsletterDB);

            ////string content = _context.Newsletter.Where(x => x.NewsletterID == newsletter.NewsletterID).FirstOrDefault().contentEmail;
            //newsletter.listaEmail = await _context.Users.Where(x => x.Newsletter == true).Select(x => x.Email).ToListAsync();


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
                Body = newsletter.contentEmail,
                To = "szuminski.p@gmail.com",
                Subject = "Newsletter"
            };

            await _emailService.SendEmailAsync(emailDto);

            return RedirectToAction(nameof(Index));
        }

        public string WstawProdukty(string content)
        {
            Newsletter newsMessagaeBody =  _context.Newsletter.Where(x => x.NewsletterID == 2).FirstOrDefault();

            string produkty = newsMessagaeBody.MessagerBody;
            return produkty;
        }

        static string body = "";


        static string tab1 = "";
        //static string tab2 = "";
        //static string tab3 = "";
        //static string tab4 = "";
        //static string tab5 = "";


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


            string td1 = "<th style=\"margin:0px; padding-left:10px;padding-right:10px; padding-top:0px; padding-bottom:0px; margin: 0px;\" > <div style=\"border:1px solid black;width:200px;height:200px;margin:0px;padding:0px; \"> <img src=../../images/produkty/" + produkt.Symbol + "/" + produkt.ImageUrl + " alt = " + produkt.Name + " style='width:200px;'>  </div></th>\r\n\t";
            string td2 = "<td style=\"padding-left:10px;padding-right:10px; padding-top:0px; padding-bottom:0px; margin: 0px; text-align: center; \"><div style=\"border:1px solid black;width:200px; border-top:0px; margin:0px; background-color: #EBEBEB; \">" + produkt.Name + "</div></td>\r\n\t";
            string td3 = "<td style=\"padding-left:10px;padding-right:10px; padding-top:0px; padding-bottom:0px; margin: 0px; text-align: center; \"><div style=\"border:1px solid black;width:200px; border-top:0px; margin:0px;padding:0px; background-color: #EBEBEB; \">" + produkt.CenaProduktu + "</div></td>\r\n\t";

            thead = thead + td1;
            tbody = tbody + td2;
            tbody2 = tbody2 + td3;
            tfoot = tfoot;


            value = thead + tbody + tbody2 + tfoot;
            if (ile%3 == 0)
            {
                thead = cthead;
                tbody = ctbody;
                tbody2 = ctbody2;
                tfoot = ctfoot;
                tab1 += value;
                value = "";
            }

            tabelka2 = "" +
               "<table style=\"padding: 0px; margin-left: auto; margin-right: auto; displey:table;\" >" +
                    "<thead>\r\n\t" +
                        "<tr >" +
                            td1+
                            "</tr>\r\n\t" +
                    "</thead>\r\n\t" +
                    "<tbody>\r\n\t" +
                        "<tr >" +
                            td2+
                        "</tr>\r\n\t" +
                        "<tr  >\r\n\t\t" +
                            td3 +
                        "</tr>\r\n\t" +
                    "</tfoot>\r\n" +
                "</table>";



            return tab1 + value;
        }

        public string value;


        public string tabelka2 = "" +
               "<table style=\"padding: 0px; margin-left: auto; margin-right: auto; displey:table;\" >" +
                    "<thead>\r\n\t" +
                        "<tr>\r\n\t\t" +
                            "</tr>\r\n\t" +
                    "</thead>\r\n\t" +
                    "<tbody>\r\n\t" +
                        "<tr>\r\n\t\t" +
                        "</tr>\r\n\t" +
                    "</tbody>\r\n" +
                "</table>";

        static string thead = "<table style=\"padding: 0px; margin-left: auto; margin-right: auto; displey:table;\" >" +
                    "<thead>\r\n\t" +
                        "<tr>\r\n\t\t";
        const string cthead = "<table style=\"padding: 0px; margin-left: auto; margin-right: auto; displey:table;\" >" +
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

        static string tbody2 = "</tr>\r\n\t" +
                        "<tr>\r\n\t\t";

        const string ctbody2 = "</tr>\r\n\t" +
                        "<tr>\r\n\t\t";


        static string tfoot = "</ tr >\r\n\t" +
                    "</tfoot>\r\n" +
                "</table>";
        const string ctfoot = "</ tr >\r\n\t" +
                    "</tfoot>\r\n" +
                "</table>";





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
