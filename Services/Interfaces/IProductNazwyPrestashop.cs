using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductNazwyPrestashop
    {
        void Add(ProductNazwyPrestashop product);

        ProductNazwyPrestashop Get(int id);

    }
}
