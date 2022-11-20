using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductNazwyPrestashop
    {
        void Add(ProductNazwyPrestashop product);

        ProductNazwyPrestashop Get(int id);

        string NazwaProduktu(int id_product);

        string KrotkiOpisProduktu(int id_product);

        string DlugiOpisProduktu(int id_product);

    }
}
