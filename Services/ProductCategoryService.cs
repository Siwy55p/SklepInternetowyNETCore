using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class ProductCategoryService : IProductCategoryService
    {

        private readonly ApplicationDbContext _context;

        public ProductCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddProductCategoryMultiple(ProductCategory productCategory)
        {
            //Product produkt = _context.Products.FirstOrDefault(x => x.ProductId == productCategory.ProductID);
            //productCategory.Product = produkt;
            //productCategory.CategoryID = productCategory.ProductCategoryId;
            _context.ProductCategory.Add(productCategory);
            _context.SaveChanges();
        }
    }
}
