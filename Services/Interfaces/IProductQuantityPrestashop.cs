using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductQuantityPrestashop
    {
        void Add(ProductPrestashop product);

        ProductPrestashop Get(int id);

        List<ProductPrestashop> ListaProduktowPrestashop();

    }
}
