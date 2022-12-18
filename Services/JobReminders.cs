using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using partner_aluro.Controllers;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using Quartz;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace partner_aluro.Services
{
    public class JobReminders : IJob
    {
        public IServiceScopeFactory ServiceScopeFactory { get; set; }
        public readonly IWebHostEnvironment _webHostEnvironment;

        public JobReminders(IServiceScopeFactory serviceScopeFactory, IWebHostEnvironment webHostEnvironment)
        {
            ServiceScopeFactory = serviceScopeFactory;
            _webHostEnvironment = webHostEnvironment;

        }

        public Task Execute(IJobExecutionContext context)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();



            // use your dbContext
            // This is my job... Do some Api requests and save to the Db

            string test = GenerateXMLController.GenerateProductXML(dbContext, _webHostEnvironment);
            string www = _webHostEnvironment.WebRootPath;
            string NameProdukt = dbContext.Products.Where(x => x.ProductId == 123).Select(x => x.Name).ToString();
            Common.Logs($"JobReminders at "+ test + " www: "+www +" Produkt: " + NameProdukt+ " Data: "  + DateTime.Now.ToString("dd-mm-yyyy hh:mm:ss"), " JobReminders " + DateTime.Now.ToString("hhmmss"));
            return Task.CompletedTask;
        }
    }
}
