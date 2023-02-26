using DeepL;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using System.Collections;
using System.Net;
using System.Resources.NetStandard;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class InicializerProductCategory : Controller
    {
        ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public InicializerProductCategory(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;

            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var Produkty = _context.Products.Select(x=> new { x.ProductId, x.CategoryId }).ToList();
            ProductCategory productCategory = new ProductCategory();

            for(int i = 0; i < Produkty.Count; i++)
            {

                ProductCategory productCategory1 = new ProductCategory()
                {
                    ProductID = Produkty[i].ProductId,
                    CategoryID = Produkty[i].CategoryId
                };
                _context.ProductCategory.Add(productCategory1);
                _context.SaveChanges();

            }
        List<ProductCategory> produktyMultipleCategory = _context.ProductCategory.ToList();

            return View(produktyMultipleCategory);
        }

        public async Task<IActionResult> Translate()
        {

            List<Product> produkty = _context.Products.ToList();

            for (int i = 0; i < produkty.Count(); i++)
            {


                var authKey = $"bbc4aaae-78af-4f5e-37dd-34e29f91a480:fx"; // Replace with your key
                var translator = new Translator(authKey);

                string NameEn = produkty[i].Name.ToString();
                string NameDe = produkty[i].Name.ToString();

                var translatedText1 = await translator.TranslateTextAsync(
                  NameEn,
                  "PL",
                  "en-US");
                NameEn = translatedText1.Text;

                var translatedText2 = await translator.TranslateTextAsync(
                  NameDe,
                  "PL",
                  "DE");
                NameDe = translatedText2.Text;

                produkty[i].NameEn = NameEn;
                produkty[i].NameDe = NameDe;

                _context.Products.Update(produkty[i]);
                _context.SaveChanges();

            }
            return View();
        }

        public IActionResult EAN13Init()
        {
            var listaprozPresta = _context.ProductsPrestashop.ToList();

            List<Product> produkty = _context.Products.ToList();

            for(int x = 0; x <listaprozPresta.Count; x++)
            {

                for(int y = 0; y < produkty.Count(); y++)
                {
                    var symbol = listaprozPresta[x].reference;
                    if (produkty[y].Symbol == symbol)
                    {
                        produkty[y].EAN13 = listaprozPresta[x].ean13;
                        _context.Products.Update(produkty[y]);
                        _context.SaveChanges();
                    }
                }

            }


            return View();
        }

        public IActionResult PathImageAsync()
        {
            IEnumerable<ImageModel> listaObrazkow =  _context.Images.Where(x => x.fullPath == null).ToList();

            foreach (var item in listaObrazkow)
            {
                item.fullPath = item.path + @"/" + item.ImageName;

                _context.Update(item);
                _context.SaveChanges();

            }
            //int i = 0;
            //i++;
            //string test="";

            return View(listaObrazkow);
        }

        public async Task<IActionResult> TlumaczAsync()
        {
            List<Product> Prodykty = _context.Products.ToList();

            for (int i = 0; i < Prodykty.Count; i++)
            {

                var authKey = $"bbc4aaae-78af-4f5e-37dd-34e29f91a480:fx"; // Replace with your key
                var translator = new Translator(authKey);

                string NameEn = Prodykty[i].Name.ToString();
                string NameDE = Prodykty[i].Name.ToString();

                var translatedText1 = await translator.TranslateTextAsync(
                  NameEn,
                  "PL",
                  "en-US");
                NameEn = translatedText1.Text;

                var translatedText2 = await translator.TranslateTextAsync(
                  NameDE,
                  "PL",
                  "DE");
                NameDE = translatedText2.Text;


                //Dodanie do pliku resx tlumaczenia nazwy produktu
                string webRootPath = _webHostEnvironment.ContentRootPath;
                string resxFile1 = webRootPath + "\\Resources\\SharedResource.pl-PL.resx";

                Dictionary<string, string> dict1 = new Dictionary<string, string>();
                dict1.Add(Prodykty[i].Name, Prodykty[i].Name);
                Hashtable data1 = new Hashtable(dict1);
                UpdateResourceFile(data1, resxFile1);
                // KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu



                //Dodanie do pliku resx tlumaczenia nazwy produktu
                string resxFile2 = webRootPath + "\\Resources\\SharedResource.en-US.resx";

                Dictionary<string, string> dict2 = new Dictionary<string, string>();
                dict2.Add(Prodykty[i].Name, NameEn);
                Hashtable data2 = new Hashtable(dict2);
                UpdateResourceFile(data2, resxFile2);
                // KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu

                //Dodanie do pliku resx tlumaczenia nazwy produktu
                string resxFile3 = webRootPath + "\\Resources\\SharedResource.de-DE.resx";

                Dictionary<string, string> dict3 = new Dictionary<string, string>();
                dict3.Add(Prodykty[i].Name, NameDE);
                Hashtable data3 = new Hashtable(dict3);
                UpdateResourceFile(data3, resxFile3);
                // KONIEC Dodanie do pliku resx tlumaczenia nazwy produktu Niemiecki
            }

            return View();
        }

        public static void UpdateResourceFile(Hashtable data, String path)
        {
            Hashtable resourceEntries = new Hashtable();

            //Get existing resources
            ResXResourceReader reader = new ResXResourceReader(path);
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            //Modify resources here...
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {

                    String value = data[key].ToString();
                    if (value == null) value = "";

                    resourceEntries.Add(key, value);
                }
            }

            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);

            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();

        }


        public void CenaPromocyjnaChangeNotNull()
        {
            List<Product> produkty = new List<Product>();

            List<Product> Produkty = _context.Products.Where(x => x.CenaPromocyja == null).ToList();

            for (int i = 0; i < Produkty.Count(); i++)
            {
                Product produkt = Produkty[i];
                produkt.CenaPromocyja = 0;
                _context.Products.Update(produkt);
                _context.SaveChanges();
            }

        }

        public void ImagePathZnakSpecjalny()
        {
            List<ImageModel> images = new List<ImageModel>();

            List<ImageModel> imagines = _context.Images.ToList();

            for (int i = 0; i < imagines.Count(); i++)
            {
                ImageModel Image = imagines[i];

                string str1 = Image.path;
                Image.path = str1.Replace(@"\", @"/");

                string str2 = Image.fullPath;
                Image.fullPath = str2.Replace(@"\", @"/");

                string str3 = Image.pathImageCompress250x250;
                Image.pathImageCompress250x250 = str3.Replace(@"\", @"/");

                _context.Images.Update(Image);
                _context.SaveChanges();


                //for (int y = 0; y < imagines.Count(); y++)
                //{
                //    if (imagines[y].ImageName == Image.ImageName && imagines[y].ImageId != Image.ImageId)
                //    {
                //        _context.Images.Remove(imagines[y]);
                //        _context.SaveChanges();
                //    }
                //}


            }

        }

        public void ImageNameRemoveIfDuplicate()
        {
            List<ImageModel> images = new List<ImageModel>();

            List<ImageModel> imagines = _context.Images.ToList();

            for (int i = 0; i < imagines.Count(); i++)
            {
                ImageModel Image = imagines[i];

                for(int y = 0; y < imagines.Count(); y++)
                {
                    if (imagines[y].ImageName == Image.ImageName && imagines[y].ImageId != Image.ImageId)
                    {
                        _context.Images.Remove(imagines[y]);
                        _context.SaveChanges();
                    }
                }

            }

        }

        public void AdresDostawyNullAdress()
        {
            List<Adress2dostawy> images = new List<Adress2dostawy>();

            List<Adress2dostawy> adresyDostawy = _context.Adress2dostawy.ToList();

            for (int i = 0; i < adresyDostawy.Count(); i++)
            {
                Adress2dostawy AdresDostawy = adresyDostawy[i];

                if(AdresDostawy.Ulica == null && AdresDostawy.UserID ==null && AdresDostawy.Adres2UserID ==null)
                {
                    _context.Adress2dostawy.Remove(AdresDostawy);
                    _context.SaveChanges();
                }


            }

        }



        public void ImageCompres250x250()
        {

            List<ImageModel> Images = _context.Images.ToList();

            for (int i = 5559; i < Images.Count; i++)
            {
                ImageModel image = Images[i];

                using (WebClient client2 = new WebClient())
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string path0 = image.path;
                    var uploadsFolder = image.fullPath;
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var dynamicFileName = image.ImageName;


                    //client2.DownloadFileAsync(new Uri(webRootPath + image.fullPath), uploadsFolder + dynamicFileName);

                    ImageModel imgModelFromDB = _context.Images.Where(x => x.ImageId == Images[i].ImageId).FirstOrDefault();



                    string pathCompresImage = webRootPath + @"/" + path0;

                    string pathname = pathCompresImage + @"/" + dynamicFileName;
                    string ImageNameCompres = "250x250_" + dynamicFileName;
                    string pathSaveCompres = pathCompresImage + ImageNameCompres;

                    string path1 = path0 + ImageNameCompres;
                    //save compres image
                    using (MagickImage imageE = new MagickImage(pathname))
                    {
                        imageE.Format = MagickFormat.WebP; // Get or Set the format of the image.
                        imageE.Resize(250, 250); // fit the image into the requested width and height. 
                        imageE.Quality = 50; // This is the Compression level.
                        imageE.Write(pathSaveCompres);
                    }

                        imgModelFromDB.ImageNameCompress250x250 = ImageNameCompres;

                        imgModelFromDB.pathImageCompress250x250 = path1;

                    _context.Images.Update(imgModelFromDB);
                    _context.SaveChanges();
                }
            }

        }


        public void ImageFrontSetCompresImage()
        {

            List<Product> Products = _context.Products.ToList();

            for (int i = 0; i < Products.Count(); i++)
            {
                Product product = Products[i];

                product = _context.Products.Where(x => x.ProductId == Products[i].ProductId).FirstOrDefault();

                product.ImageUrl = "250x250_" + product.ImageUrl;

                    _context.Products.Update(product);
                    _context.SaveChanges();
            }

        }
    }
}
