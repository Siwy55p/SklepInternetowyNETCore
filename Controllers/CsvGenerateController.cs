using CsvHelper;
using CsvHelper.Configuration;
using javax.swing.text.html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace partner_aluro.Controllers
{
    public class CsvGenerateController : Controller
    {
        readonly ApplicationDbContext _context;

        public CsvGenerateController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            List<ProductCsvBaselinker> data = _context.Products
                .Select(x => new ProductCsvBaselinker { SKU = x.Symbol, Name = x.Name, Description = x.KrotkiOpis ?? "", Price = x.CenaProduktuDetal, Quantity = (int)x.Ilosc, Category = x.CategoryId.ToString(), Weight = x.WagaProduktu.HasValue ? 0 : 00, Width = x.SzerokoscProduktu.HasValue ? 0 : 00, Height = x.WysokoscProduktu.HasValue ? 0 : 00, Depth = x.GlebokoscProduktu.HasValue ? 0:00 , EAN = x.EAN13??"" }).ToList();
            string filePath = $"C://linkTest//file.csv";
            WriteCsv(filePath, data);

            return View();
        }

        public static void WriteCsv<T>(string filePath, IEnumerable<T> records)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                Delimiter = ",",
                HasHeaderRecord = true
            };

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteHeader<T>();
                csv.WriteRecords(records);
            }
        }

    }
}
