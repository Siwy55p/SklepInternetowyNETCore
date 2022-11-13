using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductCategoryService
    {
        void AddProductCategoryMultiple(ProductCategory productCategory);
        void DeleteProductCategoryMultiple(ProductCategory productCategory);
        void DeleteProductCategoryMultiple(int ProduktID, int KategoriaID);
    }
}
