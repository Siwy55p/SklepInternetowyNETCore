using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class ProductPrestashopService : IProductPrestashop
    {
        private readonly ApplicationDbContext _context;

        public ProductPrestashopService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ProductPrestashop product)
        {
            _context.ProductsPrestashop.Add(product);
            _context.SaveChanges();
        }

        public ProductPrestashop Get(int id)
        {
            ProductPrestashop address = _context.ProductsPrestashop.Where(x => x.Id == id).FirstOrDefault();
            return address;
        }

        public List<ProductPrestashop> ListaProduktowPrestashop()
        {
            List<ProductPrestashop> lista = _context.ProductsPrestashop.ToList();
            return lista;
        }
    }
}
