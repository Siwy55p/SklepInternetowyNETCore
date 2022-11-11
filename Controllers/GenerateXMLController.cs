using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.Controllers
{
    public class GenerateXMLController : Controller
    {


        public readonly IWebHostEnvironment _webHostEnvironment;
        public readonly ApplicationDbContext _content;

        public GenerateXMLController(IWebHostEnvironment hostEnvironment, ApplicationDbContext content)
        {
            _webHostEnvironment = hostEnvironment;
            _content = content;
        }   

        public IActionResult Index()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement produkt = doc.CreateElement("export_products");
            //produkt.SetAttribute("xmlns:xsi", "http://www.abc.com");
            //produkt.SetAttribute("schemaLocation", "http://www.abc.com/XML", "http://abc.com aaa.xsd");
            //produkt.SetAttribute("xmlns", "http:www.abc.com");
            doc.AppendChild(produkt);


            //XmlNode headerNode = doc.CreateElement("Header");
            //produkt.AppendChild(headerNode);

            //XmlNode contentDateNode = doc.CreateElement("ContentDate");
            //contentDateNode.AppendChild(doc.CreateTextNode("2021-07-15"));

            //headerNode.AppendChild(contentDateNode);

            XmlNode export_productsNode = doc.CreateElement("export_products");
            doc.DocumentElement.AppendChild(export_productsNode);

            XmlNode productNode = doc.CreateElement("product");
            export_productsNode.AppendChild(productNode);

            //Name
            XmlNode productNameNode = doc.CreateElement("productName");
            productNameNode.AppendChild(doc.CreateTextNode("ProduktName2"));
            productNode.AppendChild(productNameNode);

            //Gropup
            XmlNode productGroupNode = doc.CreateElement("productGroup");
            productGroupNode.AppendChild(doc.CreateTextNode("ProduktName2"));
            productNode.AppendChild(productGroupNode);

            //Adress
            XmlNode startDateNode = doc.CreateElement("StartDate");
            startDateNode.AppendChild(doc.CreateTextNode("2021-11-11"));
            productNode.AppendChild(startDateNode);

            //var basePath = Path.Combine(Environment.CurrentDirectory, @"XMLFiles\");

            ////Save image to wwwroot/image
            //var basePath = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);


            string webRootPath = _webHostEnvironment.WebRootPath;


            var basePath = Path.Combine(webRootPath, "\\modules\\nvn_export_products\\download\\");

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            //var newFileName = string.Format("{0}{1}",Guid.NewGuid().ToString("N"),".xml");

            var newFileName = string.Format("{0}{1}", "aluro_products_export_ldWd8HWmUY", ".xml");
            doc.Save(webRootPath + basePath + newFileName);





            //http://www.partner.aluro.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml
            string var = "";
            string xml = System.IO.File.ReadAllText(webRootPath + basePath + newFileName, Encoding.UTF8);

            //var newFileName = string.Format("{0}{1}", "aluro_products_export_ldWd8HWmUY", ".xml");
            //var newFileName = string.Format("{0}{1}",Guid.NewGuid().ToString("N"),".xml");
            //doc.Save(basePath + newFileName);
            //string xml = doc.OuterXml;
            return this.Content(xml, "text/xml");


        }
    }
}