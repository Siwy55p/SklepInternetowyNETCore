using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.Controllers
{
    public class InicializerProductCategory : Controller
    {
        ApplicationDbContext _context;

        public InicializerProductCategory(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
