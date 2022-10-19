using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductService
    {
        int AddProduct(Product product);

        Task<List<Product>> GetProductList();

        Task<Product> GetProductId(int? id);

        Category GetCategoryId(int id);

        List<Category> GetListCategory();

        ICollection<Category> GetCategory();

        int DeleteProductId(int id);
    }
}
