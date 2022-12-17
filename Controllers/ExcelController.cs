using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing.Constraints;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.Controllers
{
    public class ExcelController : Controller
    {
        ApplicationDbContext _context;

        public readonly IWebHostEnvironment _webHostEnvironment;


        public ExcelController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        public ActionResult GenerateExcel()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;


            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Sample Sheet");
            worksheet.Cell("A1").Value = "Hello World!";
            worksheet.Cell("A2").FormulaA1 = "MID(A1, 7, 5)";

            var uploadsFolder = Path.Combine(webRootPath);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = "Excel1.xlsx";
            string path = uploadsFolder + "\\" + fileName;
            workbook.SaveAs(path);

            return View();
        }

        public IActionResult DownloadFile()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var memory = DownloadSingleFile("Excel1.xlsx", webRootPath);
            return File(memory.ToArray(), "application/vnd.ms-excel", "excel.xlsx");
        }

        public MemoryStream DownloadSingleFile(string fileName, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, fileName);

            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;

            return memory;
        }

        private string CreateExcelFile()
        {
            return "test";

        }

        private void CreateSheet(Category category)
        {
        }

    }
}
