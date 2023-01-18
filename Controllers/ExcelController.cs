using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using System.Data;
using System.Drawing;

namespace partner_aluro.Controllers
{
    public class ExcelController : Controller
    {
        ApplicationDbContext _context;

        public readonly IWebHostEnvironment _webHostEnvironment;

        static public List<Product> produkty;

        public ExcelController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            
        }

        public static bool IsInitialized { get; private set; }
        public static void Initialize()
        {
            if (IsInitialized)
            {
                //throw new InvalidOperationException("Object already initialized");
            }
            else
            {
                produkty = new List<Product>();
                IsInitialized = true;
            }
        }
        public static void Initialize(bool change)
        {
            IsInitialized = change;
        }

        public IActionResult Index()
        {
            return View();
        }


        public ActionResult GenerateExcel()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;



            using XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.AddWorksheet("cennik");
            ws.SheetView.Freeze(3, 0);

            var col1 = ws.Column("A");
            col1.Style.Fill.BackgroundColor = XLColor.White;
            col1.Width = 26;
            col1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col2 = ws.Column("B");
            col2.Style.Alignment.WrapText = true;
            col2.Style.Fill.BackgroundColor = XLColor.White;
            col2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col3 = ws.Column("C");
            col3.Style.Alignment.WrapText = true;
            col3.Style.Fill.BackgroundColor = XLColor.White;
            col3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col3.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            col3.Width = 45;
            var col4 = ws.Column("D");
            col4.Width = 13;
            col4.Style.Alignment.WrapText = true;
            col4.Style.Fill.BackgroundColor = XLColor.White;
            col4.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col4.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col5 = ws.Column("E");
            col5.Width = 13;
            col5.Style.Alignment.WrapText = true;
            col5.Style.Fill.BackgroundColor = XLColor.White;
            col5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col5.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col6 = ws.Column("F");
            col6.Width = 13;
            col6.Style.Alignment.WrapText = true;
            col6.Style.Fill.BackgroundColor = XLColor.White;
            col6.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col6.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col7 = ws.Column("G");
            col7.Width = 14;
            col7.Style.Alignment.WrapText = true;
            col7.Style.Fill.BackgroundColor = XLColor.White;
            col7.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col7.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col8 = ws.Column("H");
            col8.Width = 13;
            col8.Style.Alignment.WrapText = true;
            col8.Style.Fill.BackgroundColor = XLColor.White;
            col8.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col8.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col9 = ws.Column("I");
            col9.Style.Alignment.WrapText = true;
            col9.Width = 40;
            col9.Style.Fill.BackgroundColor = XLColor.White;
            col9.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col9.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col10 = ws.Column("J");
            col10.Style.Alignment.WrapText = true;
            col10.Width = 60;
            col10.Style.Fill.BackgroundColor = XLColor.White;
            col10.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col10.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col11 = ws.Column("K");
            col11.Style.Alignment.WrapText = true;
            col11.Style.Fill.BackgroundColor = XLColor.White;
            col11.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col11.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col12 = ws.Column("L");
            col12.Style.Alignment.WrapText = true;
            col12.Style.Fill.BackgroundColor = XLColor.White;
            col12.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col12.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col13 = ws.Column("M");
            col13.Style.Alignment.WrapText = true;
            col13.Style.Fill.BackgroundColor = XLColor.White;
            col13.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col13.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col14 = ws.Column("N");
            col14.Style.Alignment.WrapText = true;
            col14.Style.Fill.BackgroundColor = XLColor.White;
            col14.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col14.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var col15 = ws.Column("O");
            col15.Style.Alignment.WrapText = true;
            col15.Style.Fill.BackgroundColor = XLColor.White;
            col15.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col15.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var row1 = ws.Row(1);
            row1.Height = 100;
            var row2 = ws.Row(2);
            var row3 = ws.Row(3);
            row3.Height = 50;

            var imagePath = @"wwwroot\img\logo\logo_excel.jpg";
            var image = ws.Pictures.Add(imagePath)
                .MoveTo(ws.Cell("A1"));
            //.Scale(0.5);

            image.GetOffset(ClosedXML.Excel.Drawings.XLMarkerPosition.TopLeft);

            ws.Cell("E1").Value = "Tel. kom.: (+48 61) 694 160 741 \r\ne-mail: marcin@aluro.pl \r\n www.aluro.pl";

            ws.Range("E1:G1").Row(1).Merge();

            DateTime AktualnaData = DateTime.Now;
            ws.Cell("C1").Value = "Oferta z dnia: " + AktualnaData.ToString("dd.MM.yyyy");
            ws.Cell("C1").Style.Font.Bold = true;

            ws.Range("A2:B2").Row(1).Merge();
            ws.Cell("A2").Value = "";
            ws.Cell("A2").Style.Font.Bold = true;
            ws.Cell("A2").Style.Font.FontSize = 16;
            ws.Cell("C2").Value = "Zamawiający: ";
            ws.Cell("C2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            ws.Cell("A3").Value = "ZDJĘCIE";
            ws.Cell("A3").Style.Alignment.WrapText = true;
            ws.Cell("A3").Style.Font.Bold = true;
            ws.Cell("A3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("A3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("A3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("A3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            ws.Cell("B3").Value = "SYMBOL";
            ws.Cell("B3").Style.Alignment.WrapText = true;
            ws.Cell("B3").Style.Font.Bold = true;
            ws.Cell("B3").Style.Font.FontSize = 10;
            ws.Cell("B3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("B3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("B3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("B3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("C3").Value = "NAZWA TOWARU";
            ws.Cell("C3").Style.Alignment.WrapText = true;
            ws.Cell("C3").Style.Font.Bold = true;
            ws.Cell("C3").Style.Font.FontSize = 10;
            ws.Cell("C3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("C3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("C3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("C3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            //ws.Cell("D3").Value = "STARA CENA Brutto";
            //ws.Cell("D3").Style.Alignment.WrapText = true;
            //ws.Cell("D3").Style.Font.Bold = true;
            //ws.Cell("D3").Style.Font.FontSize = 10;
            //ws.Cell("D3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            //ws.Cell("D3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            //ws.Cell("D3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            //ws.Cell("D3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("D3").Value = "CENA HURTOWA Brutto";
            ws.Cell("D3").Style.Alignment.WrapText = true;
            ws.Cell("D3").Style.Font.Bold = true;
            ws.Cell("D3").Style.Font.FontSize = 10;
            ws.Cell("D3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("D3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("D3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("D3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("E3").Value = "PAKOWANIE";
            ws.Cell("E3").Style.Alignment.WrapText = true;
            ws.Cell("E3").Style.Font.Bold = true;
            ws.Cell("E3").Style.Font.FontSize = 10;
            ws.Cell("E3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("E3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("E3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("E3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("F3").Value = "ILOŚC ZAMAWIANA [szt]";
            ws.Cell("F3").Style.Alignment.WrapText = true;
            ws.Cell("F3").Style.Font.Bold = true;
            ws.Cell("F3").Style.Font.FontSize = 10;
            ws.Cell("F3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("F3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("F3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("F3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("G3").Value = "WARTOŚĆ POZYCJI BRUTTO";
            ws.Cell("G3").Style.Alignment.WrapText = true;
            ws.Cell("G3").Style.Font.Bold = true;
            ws.Cell("G3").Style.Font.FontSize = 10;
            ws.Cell("G3").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("G3").Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("G3").Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("G3").Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            //List<Product> produkty = _context.Products.Where(x => x.Ukryty == false).Include(x => x.Product_Images).ToList();

            int row = 4;
            for (int i = 0; i < produkty.Count(); i++)
            {

                var rowP = ws.Row(row);
                rowP.Height = 130;

                //var imagePathProduct = @"wwwroot\img\p\"+produkty[i].Symbol+@"\"+ produkty[i].ImageUrl;
                var imagePathProduct = "";
                var images = _context.Images.Where(x=>x.ImageId == produkty[i].ProductImagesId).FirstOrDefault();
                if(images!=null)
                {
                    imagePathProduct = @"wwwroot\" + images.path + images.ImageName;
                }

                //var imagePathProduct = @"wwwroot\img\" + produkty[i].Product_Images[0].path + @"\" + produkty[i].Product_Images[0].ImageName;
                if (System.IO.File.Exists(imagePathProduct))
                {
                    int iColumnWidth = (int)(ws.Column(1).Width - 1) * 7 + 12; // To convert column width in pixel unit.


                    int xOffset = (iColumnWidth - 170) / 2;
                    int yOffset = 2;
                    var imageProduct = ws.AddPicture(imagePathProduct)
                        .MoveTo(ws.Cell("A" + row), new Point(xOffset, yOffset))
                        .WithSize(170, 170);
                    //.Scale(0.5);
                    imageProduct.MoveTo(ws.Cell("A" + row), new Point(xOffset, yOffset));
                }
                ws.Cell("A" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("A" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("A" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("A" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ws.Cell("B"+row).Value = produkty[i].Symbol;
                ws.Cell("B" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("B" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("B" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("B" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ws.Cell("C" + row).Value = produkty[i].Name + " \nSzerokość: " + produkty[i].SzerokoscProduktu + " cm \nGłębokość: " + produkty[i].GlebokoscProduktu + " cm \nWysokość: " + produkty[i].WysokoscProduktu + " cm \n";
                ws.Cell("C" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("C" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("C" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("C" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                //ws.Cell("D"+row).Value = "";
                //ws.Cell("D" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //ws.Cell("D" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //ws.Cell("D" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //ws.Cell("D" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ws.Cell("D"+row).Value = produkty[i].CenaProduktuBrutto + " zł";
                ws.Cell("D" + row).Style.Font.Bold = true;
                ws.Cell("D" + row).Style.Font.FontSize = 14;
                ws.Cell("D" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("D" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("D" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("D" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ws.Cell("E"+row).Value = produkty[i].Pakowanie;
                ws.Cell("E" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("E" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("E" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("E" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ws.Cell("F"+row).Value = "";
                ws.Cell("F" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("F" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("F" + row).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Cell("F" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                ws.Cell("G" + row).FormulaA1 = "=D" + row + "*F" + row + "";
                ws.Cell("G" + row).Style.NumberFormat.Format = "0.00 zł";
                //ws.Cell("G" + row).DataType = XLDataType.Number;
                ws.Cell("G" + row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Cell("G" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Cell("G" + row).Style.Border.RightBorder = XLBorderStyleValues.Thick;

                row++;
                int ostatni = row - 4;

                if (ostatni == produkty.Count())
                {

                    ws.Cell("A" + (row-1)).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    ws.Cell("B" + (row-1)).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    ws.Cell("C" + (row-1)).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    ws.Cell("D" + (row-1)).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    ws.Cell("E" + (row-1)).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    ws.Cell("F" + (row-1)).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                }
                else
                {
                }



            }

            var Lastrow = ws.Row(row);
            Lastrow.Height = 30;
            ws.Cell("F" + row).Value = "WARTOŚĆ ZAMÓWIENIA";
            ws.Cell("F" + row).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("F" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("F" + row).Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("F" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell("G" + row).FormulaA1 = "=SUM(G" + 4 + ":G" + (row-1) + ")";
            ws.Cell("G" + row).Style.NumberFormat.Format = "0.00 zł";
            //ws.Cell("G" + row).DataType = XLDataType.Number;   //PO AKTUALIZACJI
            ws.Cell("G" + row).Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Cell("G" + row).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            ws.Cell("G" + row).Style.Border.RightBorder = XLBorderStyleValues.Thick;
            ws.Cell("G" + row).Style.Border.BottomBorder = XLBorderStyleValues.Thick;


            var uploadsFolder = Path.Combine(webRootPath);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = "Oferta.xlsx";
            string path = uploadsFolder + "\\" + fileName;
            
            workbook.SaveAs(path);

            return View();
        }
        public IActionResult DownloadFile()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var memory = DownloadSingleFile("Oferta.xlsx", webRootPath);
            return File(memory.ToArray(), "application/vnd.ms-excel", "Oferta.xlsx");
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
