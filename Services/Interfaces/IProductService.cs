using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductService
    {
        int AddProduct(Product product);

        Task<List<Product>> GetProductList();

        Task<Product> GetProductId(int? id);

        Category GetCategoryId(int id);

        List <Category> GetListCategory();

        ICollection<Category> GetCategory();

        void ZmiejszIloscProductIdAsync(int ProductId, int ile);   /*MUSI ZOSTAC VOID*/

        int DeleteProductId(int id);


        Task UpdateAsync(Product produkt);

        int GetProductId(string Symbol);


        Product GetProduct(string Symbol);

    }
}
