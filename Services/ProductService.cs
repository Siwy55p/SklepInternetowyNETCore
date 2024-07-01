using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;


        public readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {

            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public int DeleteProductId(int id)
        {
            var product = _context.Products.Find(id);


            List<ProductCategory> productCategor = _context.ProductCategory.Where(x=>x.ProductID==id).ToList();
            for(int i = 0; i < productCategor.Count; i++)
            {
                _context.ProductCategory.Remove(productCategor[i]);
                _context.SaveChanges();
            }
            product.Ilosc = 0;
            product.CategoryId = 28;
            
            _context.Products.Update(product);
            _context.SaveChanges();

            if (product.pathImageUrl250x250 != null)
            {
                var usubn_path = product.pathImageUrl250x250;


                int wystapienie_znaku_ostatnie = usubn_path.LastIndexOf("/") + 1; //11
                int dlugosc = usubn_path.Length; //20
                int ilosc_znakow = dlugosc - wystapienie_znaku_ostatnie;

                var path = usubn_path.Remove(wystapienie_znaku_ostatnie, ilosc_znakow);

                //znajdz zdjecia produktu i je usun 

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string patha = Path.Combine(wwwRootPath, path);

                //znajdz produkt i go usun z bazy danych

                System.IO.DirectoryInfo di = new DirectoryInfo(patha);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            _context.Products.Attach(product);
            _context.Products.Remove(product);
            _context.SaveChanges();


            //dlaczego nie placisz?

            return id;
        }


        public async Task<Product> GetProductId(int? id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.CategoryNavigation)
                .Include(p => p.Product_Images)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            return product;
        }

        public async Task<List<Product>> GetProductList()
        {
            List<Product> list = await _context.Products
                .AsNoTracking()
                .Include(p => p.CategoryNavigation)
                .Where(p=>p.CategoryNavigation.Aktywny == true)
                .Include(p => p.Product_Images)
                .ToListAsync();
            
            return list;
        }

        public int AddProduct(Product product)
        {

            _context.Attach(product);
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();

            //_context.Products.Add(product);//Narazie wiemy co

            if(_context.SaveChanges() >0)
            {
                System.Console.WriteLine("Sukces");
            }; //tutaj jest dodanie - zwraca ilosc wykonanych operacji

            //logika zapisujaca do bazy
            return product.ProductId;
        }

        public Category GetCategoryId(int id)
        {
            Category category = _context.Category.Find(id);
            return category;
        }

        public List<Category> GetListCategory()
        {
            List<Category> listaCategori = _context.Category.OrderBy(x=>x.CategoryId).ToList();


            return listaCategori;
        }

        public ICollection<Category> GetCategory()
        {
            return _context.Category
                .AsNoTracking()
                .ToList();
        }

        public int GetCategoryName(string name)
        {
            return _context.Category.Find(name).CategoryId;
        }

        public void ZmiejszIloscProductIdAsync(int ProductId, decimal ile)
        {
            Product product = _context.Products.FirstOrDefault(x => x.ProductId == ProductId);
            product.Ilosc -= ile;

            //_context.Attach(product);
            //_context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Product produkt)
        {
            _context.Products.Update(produkt);
            await _context.SaveChangesAsync();
        }

        public int GetProductId(string Symbol)
        {
            var product = _context.Products.FirstOrDefault(x => x.Symbol == Symbol);
            return product.ProductId;
        }

        public Product GetProduct(string Symbol)
        {
            Product produkt = new Product();
            produkt = _context.Products.Where(x => x.Symbol == Symbol).FirstOrDefault();
            return produkt;
        }
    }
}
