using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductQuantityPrestashop
    {
        void Add(ProductQuantityPrestashop QuantityProduct);

        ProductQuantityPrestashop Get(int id);

        int iloscProduktu(int product_id);
    }
}
