using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using partner_aluro.Data;
using partner_aluro.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> IndexAsync()
        {

            var produkty = await _content.Products.Include(x => x.CategoryId).ToListAsync();
            produkty.OrderBy(x => x.Symbol);



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

            //XmlNode export_productsNode = doc.CreateElement("export_products");
            //doc.DocumentElement.AppendChild(export_productsNode);

            XmlNode productNode = doc.CreateElement("product");
            produkt.AppendChild(productNode);

            //symbol
            XmlNode productNameNode = doc.CreateElement("symbol");
            productNameNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].Symbol + " ]]>"));
            productNode.AppendChild(productNameNode);

            //product_name
            XmlNode product_nameNode = doc.CreateElement("productGroup");
            product_nameNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].Name + " ]]>"));
            productNode.AppendChild(product_nameNode);

            //images
            string imagePath = "";
            foreach (var image in produkty[0].Product_Images)
            {
                imagePath += image.path + image.ImageName + ",";
            }
            //images
            XmlNode imagesNode = doc.CreateElement("images");
            imagesNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + imagePath + " ]]>"));
            productNode.AppendChild(imagesNode);

            //stock
            XmlNode stockNode = doc.CreateElement("stock");
            stockNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].Ilosc + " ]]>"));
            productNode.AppendChild(stockNode);

            //cenadetaliczna
            XmlNode cena_detalicznaNode = doc.CreateElement("cena_detaliczna");
            cena_detalicznaNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].CenaProduktuDetal + " ]]>"));
            productNode.AppendChild(cena_detalicznaNode);

            //EAN13
            XmlNode EAN13Node = doc.CreateElement("EAN13");
            EAN13Node.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].EAN13 + " ]]>"));
            productNode.AppendChild(EAN13Node);

            //EAN13
            XmlNode OpisNode = doc.CreateElement("Opis");
            OpisNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].Description + " ]]>"));
            productNode.AppendChild(OpisNode);

            //kategoria_domyslna
            XmlNode kategoria_domyslnaNode = doc.CreateElement("kategoria_domyslna");
            kategoria_domyslnaNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].CategoryNavigation.Name + " ]]>"));
            productNode.AppendChild(kategoria_domyslnaNode);

            //szerokosc
            XmlNode szerokoscNode = doc.CreateElement("szerokosc");
            szerokoscNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].SzerokoscProduktu + " ]]>"));
            productNode.AppendChild(szerokoscNode);

            //wysokosc
            XmlNode wysokoscNode = doc.CreateElement("wysokosc");
            wysokoscNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].WysokoscProduktu + " ]]>"));
            productNode.AppendChild(wysokoscNode);

            //glebokosc
            XmlNode glebokoscNode = doc.CreateElement("glebokosc");
            glebokoscNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].GlebokoscProduktu + " ]]>"));
            productNode.AppendChild(glebokoscNode);

            //waga_z_opakowaniem
            XmlNode waga_z_opakowaniemNode = doc.CreateElement("waga_z_opakowaniem");
            waga_z_opakowaniemNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + produkty[0].WagaProduktu + " ]]>"));
            productNode.AppendChild(waga_z_opakowaniemNode);


            string cechy = "";

            string Wymiar_wewnętrzny = "Wymiar_wewnętrzny: ";

            if(produkty[0].Materiał != "")
            {
                cechy += "Materiał: " + produkty[0].Materiał + "";
            }
            //cechy
            XmlNode cechyNode = doc.CreateElement("cechy");
            cechyNode.AppendChild(doc.CreateTextNode("<![CDATA[+" + cechy + " ]]>"));
            productNode.AppendChild(waga_z_opakowaniemNode);





            //patch root www
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