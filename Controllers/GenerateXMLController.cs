﻿using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Text;
using partner_aluro.Data;
using partner_aluro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Serialization;
using partner_aluro.Services.Interfaces;
using System.Net;

namespace partner_aluro.Controllers
{
    [AllowAnonymous]
    public class GenerateXMLController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public readonly IImageService _imageService;

        public readonly IProductService _productService;

        public readonly ApplicationDbContext _content;

        public static IWebHostEnvironment _hostingEnvironment;

        public GenerateXMLController(IWebHostEnvironment hostEnvironment, ApplicationDbContext content, IImageService imageService, IProductService productService)
        {
            _webHostEnvironment = hostEnvironment;
            _content = content;
            _imageService = imageService;
            _productService = productService;   
        }

        public static bool IsInitialized { get; private set; }
        public static void Initialize(IWebHostEnvironment hostEnvironment)
        {
            if (IsInitialized)
            {
                //throw new InvalidOperationException("Object already initialized");

            }

            _hostingEnvironment = hostEnvironment;
            _webRootPath = hostEnvironment.WebRootPath;
            IsInitialized = true;
        }


        public static string _webRootPath = "D:\\HostingSpaces\\siwy55p\\partneralluro.hostingasp.pl\\wwwroot\\wwwroot\\";  //Pamiętaj zeby zmienic na prawidłowy adres wchodzac w ustawienia XML 

        public static string _adresStrony = "http://www.partner.aluro.pl";

        //https://partneralluro.hostingasp.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml

        //http://www.partner.aluro.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml
        public async Task<IActionResult> DeserializeXML(string url)
        {
            url = "http://www.partner.aluro.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml";

            var filepath = url;

            //XmlSerializer serializer = new XmlSerializer(typeof(ExportProducts));
            //WebClient wc = new WebClient();
            //using (Stream fs = wc.OpenWrite(url))
            //{
            //    var obj = (ExportProducts)serializer.Deserialize(fs);
            //    return View(obj.Product.Images);
            //}

            using (var client = new HttpClient())
            {

                var content = await client.GetStreamAsync(url);
                //var contents = await client.GetStreamAsync("http://www.partner.aluro.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml");

                XmlSerializer serializer = new XmlSerializer(typeof(Export_products));
                Export_products obj = (Export_products)serializer.Deserialize(content);

                //1215 w Images Mozna usuwac
                char[] delimiterChars = { ',' };



                var produkt = obj.Product.Where(x => x.Symbol == "A01306").FirstOrDefault();


                string text = produkt.Images;
                    string[] words = text.Split(delimiterChars);


                for (int i = 0; i < words.Count(); i++)
                {

                    using (WebClient client2 = new WebClient())
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string path0 = @"img/p/" + produkt.Symbol + @"/";
                        var uploadsFolder = Path.Combine(webRootPath, @"img/p/" + produkt.Symbol + @"/");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var dynamicFileName = produkt.Symbol + "_" + i + "_.jpg";


                        client2.DownloadFileAsync(new Uri(words[i]), uploadsFolder + dynamicFileName);

                        ImageModel imgModel = new ImageModel();
                        var cont = _productService.GetProduct(produkt.Symbol);
                        if (cont != null)
                        {
                            if(cont.Product_Images == null)
                            {
                                cont.Product_Images = new List<ImageModel>();
                            }

                            imgModel = new()
                            {
                                path = path0,
                                fullPath = path0 + dynamicFileName,
                                Opis = produkt.Symbol,
                                kolejnosc = i,
                                Tytul = produkt.Product_name,
                                ImageName = dynamicFileName,
                                ProductId = cont.ProductId,
                                ProductImagesId = cont.ProductId,

                            };

                            cont.Product_Images.Add(imgModel);
                            await _productService.UpdateAsync(cont);

                            _imageService.AddAsync(imgModel);
                        }
                        else
                        {
                            imgModel = new()
                            {
                                path = path0,
                                fullPath = path0 + dynamicFileName,
                                Opis = produkt.Symbol,
                                kolejnosc = i,
                                Tytul = produkt.Product_name,
                                ImageName = dynamicFileName
                            };
                        }
                        _imageService.AddAsync(imgModel);

                    }
                }


                ////src = "~/img/p/@Model.Symbol/@Model.ImageUrl"
                //for (int x = 0; x < obj.Product.Count(); x++)
                ////foreach (var item in obj.Product)
                //{

                //    string text = obj.Product[x].Images;
                //    string[] words = text.Split(delimiterChars);

                //    for(int i = 0; i < words.Count(); i++)
                //    {

                //        using (WebClient client2 = new WebClient())
                //        {
                //            string webRootPath = _webHostEnvironment.WebRootPath;
                //            string path0 = "img\\p\\" + obj.Product[x].Symbol + "\\";
                //            var uploadsFolder = Path.Combine(webRootPath, "img\\p\\" + obj.Product[x].Symbol+"\\");
                //            if (!Directory.Exists(uploadsFolder))
                //            {
                //                Directory.CreateDirectory(uploadsFolder);
                //            }

                //            var dynamicFileName = obj.Product[x].Symbol + "_" + i + "_.jpg";


                //            client2.DownloadFileAsync(new Uri(words[i]), uploadsFolder + dynamicFileName);

                //            ImageModel imgModel = new ImageModel();

                //            var cont = _content.Products.Where(p => p.Symbol == obj.Product[x].Symbol).FirstOrDefault()?.ProductId;
                //            if(cont != null)
                //            { 

                //                imgModel = new()
                //                {
                //                    path = path0,
                //                    fullPath = path0 + dynamicFileName,
                //                    Opis = obj.Product[x].Symbol,
                //                    kolejnosc = i,
                //                    Tytul = obj.Product[x].Product_name,
                //                    ImageName = dynamicFileName,
                //                    ProductId = cont,
                //                    ProductImagesId = cont,

                //                };
                //            }else
                //            {
                //                imgModel = new()
                //                {
                //                    path = path0,
                //                    fullPath = path0 + dynamicFileName,
                //                    Opis = obj.Product[x].Symbol,
                //                    kolejnosc = i,
                //                    Tytul = obj.Product[x].Product_name,
                //                    ImageName = dynamicFileName
                //                };
                //            }
                //        _imageService.AddAsync(imgModel);

                //        }

                //    }

                //}



                return View(produkt);

            }



            //return View();
        }

        public async Task<IActionResult> DeserializeXMLMaterial()
        {
            string url = "http://www.partner.aluro.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml";

            var filepath = url;
            ProductXML produkt = new ProductXML();
            using (var client = new HttpClient())
            {

                var content = await client.GetStreamAsync(url);
                //var contents = await client.GetStreamAsync("http://www.partner.aluro.pl/modules/nvn_export_products/download/aluro_products_export_ldWd8HWmUY.xml");

                XmlSerializer serializer = new XmlSerializer(typeof(Export_products));
                Export_products obj = (Export_products)serializer.Deserialize(content);



                //1215 w Images Mozna usuwac
                char[] delimiterChars = { ',' };



                for(int x = 0; x < obj.Product.Count(); x++)
                {

                    produkt = obj.Product[x];


                    string text = produkt.Cechy;
                    string[] words = text.Split(delimiterChars);


                    for (int i = 0; i < words.Count(); i++)
                    {
                        string textes = words[i];


                        //if(textes.Contains("Pakowanie:"))
                        //{
                        //    textes = textes.Replace("Pakowanie:", string.Empty);
                        //    Product produktA = _content.Products.Where(x => x.Symbol == produkt.Symbol).FirstOrDefault();
                        //    if (produktA != null)
                        //    {
                        //        produktA.Pakowanie = textes;
                        //        _content.Products.Update(produktA);
                        //        _content.SaveChanges();
                        //    }

                        //}

                        if (textes.Contains("Materiał:"))
                        {
                            textes = textes.Replace("Materiał:", string.Empty);
                            Product produktA = _content.Products.Where(x => x.Symbol == produkt.Symbol).FirstOrDefault();
                            if (produktA != null)
                            {
                                produktA.Materiał = textes;
                                _content.Products.Update(produktA);
                                _content.SaveChanges();
                            }

                        }

                        string ilosc = textes;

                    }

                }

            }
            return View(produkt);
        }




        [Route("modules/nvn_export_products/download/")]
        public async Task<IActionResult> Index()
        {

            Initialize(_webHostEnvironment);


            if (!IsInitialized)
                throw new InvalidOperationException("Object is not initialized");


            //patch root www
            string webRootPath = _hostingEnvironment.WebRootPath;

            string uploadsFolder = @"/modules/nvn_export_products/download/";
            string basePath = Path.Combine(_webRootPath, uploadsFolder);

            //var basePath = Path.Combine(webRootPath, uploadsFolder);

            if (!Directory.Exists(_webRootPath + basePath))
            {
                Directory.CreateDirectory(_webRootPath + basePath);
            }

            //var newFileName = string.Format("{0}{1}",Guid.NewGuid().ToString("N"),".xml");

            var newFileName = string.Format("{0}{1}", "aluro_products_export_ldWd8HWmUY", ".xml");

            //string xml = "";

            //using (StreamReader streamReader = new StreamReader(_webRootPath + basePath + newFileName, Encoding.UTF8))
            //{
            //    string xml = System.IO.File.ReadAllText(streamReader.Vale);
            //}

                //string xml = System.IO.File.ReadAllText(_webRootPath + basePath + newFileName, Encoding.UTF8);

            string xml = "";
            using (StreamReader streamReader = new StreamReader(_webRootPath + basePath + newFileName, Encoding.UTF8))
            {
                xml = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            return this.Content(xml, "text/xml");
        }
        public IActionResult GenerujRecznie()
        {
            GenerateProductXML(_content, _webHostEnvironment);
            return View();
        }

        public static string GenerateProductXML(ApplicationDbContext _content, IWebHostEnvironment _webHostEnvironment)
        {

            //////patch root www
            //string webRootPath = _webHostEnvironment.WebRootPath;

            var produkty = _content.Products.Include(p => p.Product_Images).Include(p => p.CategoryNavigation).OrderBy(p => p.Symbol).ToList();

            XmlDocument doc = new();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement export_products = doc.CreateElement("export_products");
            doc.AppendChild(export_products);


            for (int i = 0; i < produkty.Count(); i++)
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


                //images
                string imagePath = "";

                //for (int x = 0; x < produkty[i].Product_Images.Count; x++)
                //{
                //    var Item = produkty[i].Product_Images.Count;

                //    if (x < produkty[i].Product_Images.Count)
                //    {
                //        imagePath += webRootPath + "\\" + produkty[i].Product_Images[x].path + "\\" + produkty[i].Product_Images[x].ImageName + ", ";
                //    }
                //    else if(x== produkty[i].Product_Images.Count)
                //    {
                //        imagePath += webRootPath + "\\" + produkty[i].Product_Images[x].path + "\\" + produkty[i].Product_Images[x].ImageName;
                //    }
                //}




                var last = produkty[i].Product_Images.LastOrDefault();

                foreach (var image in produkty[i].Product_Images)
                {
                    if (image.Equals(last))
                    {
                        imagePath += _adresStrony + @"/" + image.path + image.ImageName;
                    }
                    else
                    {
                        imagePath += _adresStrony + @"/" + image.path + image.ImageName + ", ";
                    }
                }


                XmlCDataSection CDataImages;
                CDataImages = doc.CreateCDataSection(imagePath);

                //images
                XmlNode imagesNode = doc.CreateElement("images");
                imagesNode.AppendChild(CDataImages);
                productNode.AppendChild(imagesNode);



                //product_name CData
                XmlCDataSection CDatastoc;
                CDatastoc = doc.CreateCDataSection("0");
                if (produkty[i].Ilosc > 8)
                {
                    CDatastoc = doc.CreateCDataSection("8");
                } else if (produkty[i].Ilosc <= 8)
                {
                    int ile = Decimal.ToInt32(produkty[i].Ilosc);

                    CDatastoc = doc.CreateCDataSection(ile.ToString());
                }else if (produkty[i].Ukryty==true)
                {
                    CDatastoc = doc.CreateCDataSection("0");
                }

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



                //Cechy CECHY
                string cechy = "";
                //string Wymiar_wewnętrzny = "Wymiar_wewnętrzny: ";

                if (produkty[i].Pakowanie != null)
                {
                    cechy += "Pakowanie:" + produkty[i].Pakowanie + ", ";
                }
                if (produkty[i].Materiał != null)
                {
                    cechy += "Materiał:" + produkty[i].Materiał + "";
                }

                XmlCDataSection CDataCechy;
                CDataCechy = doc.CreateCDataSection(cechy);
                //cechy
                //WagaProduktu
                XmlNode cechyNode = doc.CreateElement("cechy");
                cechyNode.AppendChild(CDataCechy);
                productNode.AppendChild(cechyNode);

            }

            ////patch root www
            //string webRootPath = _webHostEnvironment.WebRootPath;



            Initialize(_webHostEnvironment);

            //if (!IsInitialized)
            //    throw new InvalidOperationException("Object is not initialized");


            //patch root www
            string webRootPath = _hostingEnvironment.WebRootPath;

            string uploadsFolder = @"/modules/nvn_export_products/download/";
            string basePath = Path.Combine(_webRootPath, uploadsFolder);

            //var basePath = Path.Combine(webRootPath, uploadsFolder);

            if (!Directory.Exists(_webRootPath+basePath))
            {
                Directory.CreateDirectory(_webRootPath+basePath);
            }

            //var newFileName = string.Format("{0}{1}",Guid.NewGuid().ToString("N"),".xml");

            var newFileName = string.Format("{0}{1}", "aluro_products_export_ldWd8HWmUY", ".xml");

            using (var writer = XmlTextWriter.Create(_webRootPath + basePath + newFileName))
            {
                doc.Save(writer);

                writer.Close();
                writer.Dispose();
            }

            //Save
            //doc.Save(_webRootPath + basePath + newFileName);
            //doc.Save(writer);
            //Save


            //string xml = System.IO.File.ReadAllText(_webRootPath + basePath + newFileName, Encoding.UTF8);


            return "Wykonane";
            //return this.Content(xml, "text/xml");
            ////patch root www
            //string webRootPath = _webHostEnvironment.WebRootPath;

            //var basePath = Path.Combine(webRootPath, "\\modules\\nvn_export_products\\download\\");

            //if (!Directory.Exists(basePath))
            //{
            //    Directory.CreateDirectory(basePath);
            //}

            //var newFileName = string.Format("{0}{1}", "aluro_products_export_ldWd8HWmUY", ".xml");

            //string xml = System.IO.File.ReadAllText(webRootPath + basePath + newFileName, Encoding.UTF8);

            //return this.Content(xml, "text/xml");
        }

        [HttpGet]
        public IActionResult Ustawienia()
        {


            Initialize(_webHostEnvironment);

            if (!IsInitialized)
                throw new InvalidOperationException("Object is not initialized");


            string webRootPath = _hostingEnvironment.WebRootPath;
            _webRootPath = webRootPath;

            GenerateXML generate = new GenerateXML()
            {
                path = _webRootPath,
                adresStrony = _adresStrony,
            };

            return View(generate);
        }

        [HttpPost]
        public IActionResult SetUstawienia(GenerateXML ustawienia)
        {
            _webRootPath = ustawienia.path;
            _adresStrony = ustawienia.adresStrony;

            return RedirectToAction("Ustawienia");

        }


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        private async Task UploadFile2Async(Product product)
        {
            var files = HttpContext.Request.Form.Files;

            if (product.ImageUrl != "" && files.Count > 1) //To oznacza ze frontowy obrazek został dodany
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                for (int i = 1; i <= files.Count; i++)
                {
                    //Save image to wwwroot/image
                    string path0 = @"img/p/";
                    var uploadsFolder = Path.Combine(webRootPath, @"img/p/" + product.Symbol);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var extension = Path.GetExtension(files[i].FileName);
                    var dynamicFileName = product.Symbol + "_" + i + "_" + extension;

                    using (var filesStream = new FileStream(Path.Combine(uploadsFolder, dynamicFileName), FileMode.Create))
                    {
                        files[i].CopyTo(filesStream);
                    }

                    //add product Image for new product
                    ImageModel imgModel = new()
                    {
                        path = path0 + product.Symbol,
                        kolejnosc = i,
                        Tytul = product.Name,
                        ImageName = dynamicFileName,
                        ProductId = product.ProductId
                    };

                    product.Product_Images.Add(imgModel);

                    _imageService.AddAsync(imgModel);

                }
            }
        }
    }
}