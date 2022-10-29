using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface ICategoryService
    {
        //AddSave kategoria do bazy
        int AddSave(Category category);
        int AddSave(SubCategory category);
        Category Update(Category category);
        Task<List<Category>> List();
        List<Category> GetList();
        Task<List<Category>> List(string name);
        Task<Category> GetAsync(int id);
        Task<Category> GetAsync(string name);
        int Delete(int id);
        int Delete(string name);

        Task <List<Product>> ListProductCategoryAll();

        List<Product> ListProductInCategory(string CategoryName);
    }
}
