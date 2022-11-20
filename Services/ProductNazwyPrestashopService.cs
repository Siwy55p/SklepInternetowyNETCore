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
            string nazwa = "";
            var context = _context.ProductsNamePrestashop.Where(x => x.id_product == id_product).FirstOrDefault()?.name;

            if(context == null)
            {
                nazwa = "";
            }else
            {
                nazwa = context;
            }
            return nazwa;

        }
        public string KrotkiOpisProduktu(int id_product)
        {
            string krotkiopis = "";
            var ktorkiopiscon = _context.ProductsNamePrestashop.Where(x => x.id_product == id_product).FirstOrDefault()?.description_short;
            if (ktorkiopiscon == null)
            {
                krotkiopis = "";
            }else
            {
                krotkiopis = ktorkiopiscon;
            }
            return krotkiopis;
        }

        public string DlugiOpisProduktu(int id_product)
        {
            string description = "";
            var con = _context.ProductsNamePrestashop.Where(x => x.id_product == id_product).FirstOrDefault()?.description;
            if (con == null)
            {
                return description;
            }else
            {
                description = con;
            }
            return description;
        }
    }
}
