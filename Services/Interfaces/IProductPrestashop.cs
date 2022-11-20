using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductPrestashop
    {
        void Add(ProductPrestashop product);

        ProductPrestashop Get(int id);

    }
}
