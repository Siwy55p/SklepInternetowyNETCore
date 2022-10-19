using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface ICategoryService
    {
        //AddSave kategoria do bazy
        int AddSave(Category category);
        Category Update(Category category);
        List<Category> List();
        List<Category> List(string name);
        Category Get(int id);
        Category Get(string name);
        int Delete(int id);
        int Delete(string name);

        Task <List<Product>> ListProductCategoryAll();

        List<Product> ListProductInCategory(string CategoryName);
    }
}
