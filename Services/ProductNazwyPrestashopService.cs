using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;


namespace partner_aluro.Services
{
    public class ProductNazwyPrestashopService : IProductNazwyPrestashop
    {
        private readonly ApplicationDbContext _context;

        public ProductNazwyPrestashopService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ProductNazwyPrestashop productNazwy)
        {
            _context.ProductsNamePrestashop.Add(productNazwy);
            _context.SaveChanges();
        }

        public ProductNazwyPrestashop Get(int id)
        {
            ProductNazwyPrestashop productNazwa = _context.ProductsNamePrestashop.Where(x => x.Id == id).FirstOrDefault();
            return productNazwa;
        }

        public string NazwaProduktu(int id_product)
        {
            string nazwa = _context.ProductsNamePrestashop.Where(x => x.id_product == id_product).FirstOrDefault().name;
            if (nazwa == null)
            {
                return "Brak Nazwy";
            }
            else
            {
                return nazwa;
            }
        }
        public string KrotkiOpisProduktu(int id_product)
        {
            string krotkiOpis = _context.ProductsNamePrestashop.Where(x => x.id_product == id_product).FirstOrDefault().description_short;
            return krotkiOpis;
        }

        public string DlugiOpisProduktu(int id_product)
        {
            string description = _context.ProductsNamePrestashop.Where(x => x.id_product == id_product).FirstOrDefault().description;
            return description;
        }
    }
}
