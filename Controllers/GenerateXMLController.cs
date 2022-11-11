﻿using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using partner_aluro.Data;
using partner_aluro.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

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

        [Route("modules/nvn_export_products/download/")]
        public async Task<IActionResult> IndexAsync()
        {
            webRootPath2 = _webHostEnvironment.WebRootPath;

            //var produkty = await _content.Products.Include(x => x.CategoryId).ToListAsync();
            //produkty.OrderBy(x => x.Symbol);

            var produkts = _content.Products.Where(p => p.Ukryty == false).Where(p =>p.CategoryNavigation.Aktywny == true).Include(p => p.CategoryNavigation);
            var produkty = await produkts.OrderBy(p=>p.Symbol).ToListAsync();


            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement export_products = doc.CreateElement("export_products");
            //produkt.SetAttribute("xmlns:xsi", "http://www.abc.com");
            //produkt.SetAttribute("schemaLocation", "http://www.abc.com/XML", "http://abc.com aaa.xsd");
            //produkt.SetAttribute("xmlns", "http:www.abc.com");
            doc.AppendChild(export_products);


            //XmlNode headerNode = doc.CreateElement("Header");
            //produkt.AppendChild(headerNode);

            //XmlNode contentDateNode = doc.CreateElement("ContentDate");
            //contentDateNode.AppendChild(doc.CreateTextNode("2021-07-15"));

            //headerNode.AppendChild(contentDateNode);

            //XmlNode export_productsNode = doc.CreateElement("export_products");
            //doc.DocumentElement.AppendChild(export_productsNode);

            for (int i = 1; i < produkty.Count - 1; i++)
            {
                XmlNode productNode = doc.CreateElement("product");
            export_products.AppendChild(productNode);


                //symbol CData
                XmlCDataSection CDataSymbol;
                CDataSymbol = doc.CreateCDataSection(produkty[i].Symbol);

                XmlNode symbolNode = doc.CreateElement("symbol");
                symbolNode.AppendChild(CDataSymbol);
                productNode.AppendChild(symbolNode);

                //product_name CData
                XmlCDataSection CDataproduct_name;
                CDataproduct_name = doc.CreateCDataSection(produkty[i].Name);


                //product_name
                XmlNode product_nameNode = doc.CreateElement("product_name");
                product_nameNode.AppendChild(CDataproduct_name);
                productNode.AppendChild(product_nameNode);

                //images
                string imagePath = "";

                foreach (var image in produkty[i].Product_Images)
                {
                    imagePath += image.path + image.ImageName + ",";
                }


                XmlCDataSection CDataImages;
                CDataImages = doc.CreateCDataSection(imagePath);

                //images
                XmlNode imagesNode = doc.CreateElement("images");
                imagesNode.AppendChild(CDataImages);
                productNode.AppendChild(imagesNode);



                //product_name CData
                XmlCDataSection CDatastoc;
                CDatastoc = doc.CreateCDataSection(produkty[i].Ilosc.ToString());

                //stock
                XmlNode stockNode = doc.CreateElement("stock");
                stockNode.AppendChild(CDatastoc);
                productNode.AppendChild(stockNode);



                //cenadetaliczna CData
                XmlCDataSection CDatacenadetaliczna;
                float cena_detaliczna = (float)produkty[i].CenaProduktuDetal;
                CDatacenadetaliczna = doc.CreateCDataSection(cena_detaliczna.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //cenadetaliczna
                XmlNode cena_detalicznaNode = doc.CreateElement("cena_detaliczna");
                cena_detalicznaNode.AppendChild(CDatacenadetaliczna);
                productNode.AppendChild(cena_detalicznaNode);



                //cenadetaliczna CData
                XmlCDataSection CDataEAN13;
                CDataEAN13 = doc.CreateCDataSection(produkty[i].EAN13);

                //EAN13
                XmlNode EAN13Node = doc.CreateElement("EAN13");
                EAN13Node.AppendChild(CDataEAN13);
                productNode.AppendChild(EAN13Node);


                //Opis CData
                XmlCDataSection CDataOpis;
                CDataOpis = doc.CreateCDataSection(produkty[i].Description);

                //Opis
                XmlNode OpisNode = doc.CreateElement("opis");
                OpisNode.AppendChild(CDataOpis);
                productNode.AppendChild(OpisNode);



                //Opis CData
                XmlCDataSection CDatakategoria_domyslna;
                CDatakategoria_domyslna = doc.CreateCDataSection(produkty[i].CategoryNavigation.Name);

                //kategoria_domyslna
                XmlNode kategoria_domyslnaNode = doc.CreateElement("kategoria_domyslna");
                kategoria_domyslnaNode.AppendChild(CDatakategoria_domyslna);
                productNode.AppendChild(kategoria_domyslnaNode);



                //Opis CDataszerokosc
                XmlCDataSection CDataszerokosc;

                float SzerokoscProduktu = 0;

                if (produkty[i].SzerokoscProduktu != null)
                {
                    SzerokoscProduktu = (float)produkty[i].SzerokoscProduktu;
                }

                CDataszerokosc = doc.CreateCDataSection(SzerokoscProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //szerokosc
                XmlNode szerokoscNode = doc.CreateElement("szerokosc");
                szerokoscNode.AppendChild(CDataszerokosc);
                productNode.AppendChild(szerokoscNode);


                //Opis CDatawysokosc
                XmlCDataSection CDatawysokosc;

                float WysokoscProduktu = 0;

                if (produkty[i].WysokoscProduktu != null)
                {
                    WysokoscProduktu = (float)produkty[i].WysokoscProduktu;
                }

                CDatawysokosc = doc.CreateCDataSection(WysokoscProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //wysokosc
                XmlNode wysokoscNode = doc.CreateElement("wysokosc");
                wysokoscNode.AppendChild(CDatawysokosc);
                productNode.AppendChild(wysokoscNode);


                //CDataglebokosc
                XmlCDataSection CDataglebokosc;


                float GlebokoscProduktu = 0;

                if (produkty[i].GlebokoscProduktu != null)
                {
                    GlebokoscProduktu = (float)produkty[i].GlebokoscProduktu;
                }

                CDataglebokosc = doc.CreateCDataSection(GlebokoscProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //glebokosc
                XmlNode glebokoscNode = doc.CreateElement("glebokosc");
                glebokoscNode.AppendChild(CDataglebokosc);
                productNode.AppendChild(glebokoscNode);

                //CDataWaga
                XmlCDataSection CDataWaga;

                float WagaProduktu = 0;

                if (produkty[i].WagaProduktu != null)
                {
                    WagaProduktu = (float)produkty[i].WagaProduktu;
                }

                CDataWaga = doc.CreateCDataSection(WagaProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //WagaProduktu
                XmlNode WagaNode = doc.CreateElement("waga_z_opakowaniem");
                WagaNode.AppendChild(CDataWaga);
                productNode.AppendChild(WagaNode);



                //Cechy CHECHY
                string cechy = "";
                string Wymiar_wewnętrzny = "Wymiar_wewnętrzny: ";

                if (produkty[i].Materiał != "")
                {
                    cechy += "Materiał: " + produkty[i].Materiał + "";
                }

                XmlCDataSection CDataCechy;
                CDataCechy = doc.CreateCDataSection(cechy);
                //cechy
                XmlNode cechyNode = doc.CreateElement("cechy");
                cechyNode.AppendChild(CDataCechy);

            }



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

        public static async Task GenerateProductXMLAsync(ApplicationDbContext _content)
        {

            //var produkty = await _content.Products.Include(x => x.CategoryId).ToListAsync();
            //produkty.OrderBy(x => x.Symbol);

            var produkts = _content.Products.Where(p => p.Ukryty == false).Where(p => p.CategoryNavigation.Aktywny == true).Include(p => p.CategoryNavigation);
            var produkty = await produkts.OrderBy(p => p.Symbol).ToListAsync();


            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement export_products = doc.CreateElement("export_products");
            //produkt.SetAttribute("xmlns:xsi", "http://www.abc.com");
            //produkt.SetAttribute("schemaLocation", "http://www.abc.com/XML", "http://abc.com aaa.xsd");
            //produkt.SetAttribute("xmlns", "http:www.abc.com");
            doc.AppendChild(export_products);


            //XmlNode headerNode = doc.CreateElement("Header");
            //produkt.AppendChild(headerNode);

            //XmlNode contentDateNode = doc.CreateElement("ContentDate");
            //contentDateNode.AppendChild(doc.CreateTextNode("2021-07-15"));

            //headerNode.AppendChild(contentDateNode);

            //XmlNode export_productsNode = doc.CreateElement("export_products");
            //doc.DocumentElement.AppendChild(export_productsNode);

            for (int i = 1; i < produkty.Count - 1; i++)
            {
                XmlNode productNode = doc.CreateElement("product");
                export_products.AppendChild(productNode);


                //symbol CData
                XmlCDataSection CDataSymbol;
                CDataSymbol = doc.CreateCDataSection(produkty[i].Symbol);

                XmlNode symbolNode = doc.CreateElement("symbol");
                symbolNode.AppendChild(CDataSymbol);
                productNode.AppendChild(symbolNode);

                //product_name CData
                XmlCDataSection CDataproduct_name;
                CDataproduct_name = doc.CreateCDataSection(produkty[i].Name);


                //product_name
                XmlNode product_nameNode = doc.CreateElement("product_name");
                product_nameNode.AppendChild(CDataproduct_name);
                productNode.AppendChild(product_nameNode);

                //images
                string imagePath = "";

                foreach (var image in produkty[i].Product_Images)
                {
                    imagePath += image.path + image.ImageName + ",";
                }


                XmlCDataSection CDataImages;
                CDataImages = doc.CreateCDataSection(imagePath);

                //images
                XmlNode imagesNode = doc.CreateElement("images");
                imagesNode.AppendChild(CDataImages);
                productNode.AppendChild(imagesNode);



                //product_name CData
                XmlCDataSection CDatastoc;
                CDatastoc = doc.CreateCDataSection(produkty[i].Ilosc.ToString());

                //stock
                XmlNode stockNode = doc.CreateElement("stock");
                stockNode.AppendChild(CDatastoc);
                productNode.AppendChild(stockNode);



                //cenadetaliczna CData
                XmlCDataSection CDatacenadetaliczna;
                float cena_detaliczna = (float)produkty[i].CenaProduktuDetal;
                CDatacenadetaliczna = doc.CreateCDataSection(cena_detaliczna.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //cenadetaliczna
                XmlNode cena_detalicznaNode = doc.CreateElement("cena_detaliczna");
                cena_detalicznaNode.AppendChild(CDatacenadetaliczna);
                productNode.AppendChild(cena_detalicznaNode);



                //cenadetaliczna CData
                XmlCDataSection CDataEAN13;
                CDataEAN13 = doc.CreateCDataSection(produkty[i].EAN13);

                //EAN13
                XmlNode EAN13Node = doc.CreateElement("EAN13");
                EAN13Node.AppendChild(CDataEAN13);
                productNode.AppendChild(EAN13Node);


                //Opis CData
                XmlCDataSection CDataOpis;
                CDataOpis = doc.CreateCDataSection(produkty[i].Description);

                //Opis
                XmlNode OpisNode = doc.CreateElement("opis");
                OpisNode.AppendChild(CDataOpis);
                productNode.AppendChild(OpisNode);



                //Opis CData
                XmlCDataSection CDatakategoria_domyslna;
                CDatakategoria_domyslna = doc.CreateCDataSection(produkty[i].CategoryNavigation.Name);

                //kategoria_domyslna
                XmlNode kategoria_domyslnaNode = doc.CreateElement("kategoria_domyslna");
                kategoria_domyslnaNode.AppendChild(CDatakategoria_domyslna);
                productNode.AppendChild(kategoria_domyslnaNode);



                //Opis CDataszerokosc
                XmlCDataSection CDataszerokosc;

                float SzerokoscProduktu = 0;

                if (produkty[i].SzerokoscProduktu != null)
                {
                    SzerokoscProduktu = (float)produkty[i].SzerokoscProduktu;
                }

                CDataszerokosc = doc.CreateCDataSection(SzerokoscProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //szerokosc
                XmlNode szerokoscNode = doc.CreateElement("szerokosc");
                szerokoscNode.AppendChild(CDataszerokosc);
                productNode.AppendChild(szerokoscNode);


                //Opis CDatawysokosc
                XmlCDataSection CDatawysokosc;

                float WysokoscProduktu = 0;

                if (produkty[i].WysokoscProduktu != null)
                {
                    WysokoscProduktu = (float)produkty[i].WysokoscProduktu;
                }

                CDatawysokosc = doc.CreateCDataSection(WysokoscProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //wysokosc
                XmlNode wysokoscNode = doc.CreateElement("wysokosc");
                wysokoscNode.AppendChild(CDatawysokosc);
                productNode.AppendChild(wysokoscNode);


                //CDataglebokosc
                XmlCDataSection CDataglebokosc;


                float GlebokoscProduktu = 0;

                if (produkty[i].GlebokoscProduktu != null)
                {
                    GlebokoscProduktu = (float)produkty[i].GlebokoscProduktu;
                }

                CDataglebokosc = doc.CreateCDataSection(GlebokoscProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //glebokosc
                XmlNode glebokoscNode = doc.CreateElement("glebokosc");
                glebokoscNode.AppendChild(CDataglebokosc);
                productNode.AppendChild(glebokoscNode);

                //CDataWaga
                XmlCDataSection CDataWaga;

                float WagaProduktu = 0;

                if (produkty[i].WagaProduktu != null)
                {
                    WagaProduktu = (float)produkty[i].WagaProduktu;
                }

                CDataWaga = doc.CreateCDataSection(WagaProduktu.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

                //WagaProduktu
                XmlNode WagaNode = doc.CreateElement("waga_z_opakowaniem");
                WagaNode.AppendChild(CDataWaga);
                productNode.AppendChild(WagaNode);



                //Cechy CHECHY
                string cechy = "";
                string Wymiar_wewnętrzny = "Wymiar_wewnętrzny: ";

                if (produkty[i].Materiał != "")
                {
                    cechy += "Materiał: " + produkty[i].Materiał + "";
                }

                XmlCDataSection CDataCechy;
                CDataCechy = doc.CreateCDataSection(cechy);
                //cechy
                XmlNode cechyNode = doc.CreateElement("cechy");
                cechyNode.AppendChild(CDataCechy);

            }



            //patch root www
            string webRootPath = webRootPath2;

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
        }



        public static string webRootPath2 = "";
    }
}