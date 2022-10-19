using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;

namespace partner_aluro.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int DeleteProductId(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return id;
        }

        public async Task<Product> GetProductId(int? id)
        {
            var product = await _context.Products
                .Include(p => p.CategoryNavigation)
                .Include(p => p.product_Images)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            return product;
        }

        public async Task<List<Product>> GetProductList()
        {
            var applicationDbContext = _context.Products.Include(p => p.CategoryNavigation);
            var list = await applicationDbContext.ToListAsync();

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
            List<Category> listaCategori = _context.Category.ToList();
            return listaCategori;
        }

        public ICollection<Category> GetCategory()
        {
            return _context.Category.ToList();
        }

        public int GetCategoryName(string name)
        {
            return _context.Category.Find(name).CategoryId;
        }


    }
}
