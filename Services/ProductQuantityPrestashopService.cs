using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class ProductQuantityPrestashopService : IProductQuantityPrestashop
    {
        public readonly ApplicationDbContext _context;
        public ProductQuantityPrestashopService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(ProductQuantityPrestashop ProductQuantity)
        {
            _context.ProductsQuantityPrestashop.Add(ProductQuantity);
            _context.SaveChanges();
        }

        public ProductQuantityPrestashop Get(int id)
        {
            ProductQuantityPrestashop pq = _context.ProductsQuantityPrestashop.Where(x => x.id == id).FirstOrDefault();
            return pq;
        }

        public int iloscProduktu(int product_id)
        {
            int ilosc = _context.ProductsQuantityPrestashop.Where(x => x.id_product == product_id).FirstOrDefault().quantity;
            return ilosc;
        }
    }
}
