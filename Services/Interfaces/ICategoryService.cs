using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface ICategoryService
    {
        //AddSave kategoria do bazy
        Task AddSave(Category category);
        Task AddSave(SubCategory category);
        Category Update(Category category);
        Task<List<Category>> List();
        List<Category> GetList();
        Task<List<Category>> List(string name);
        Task<Category> GetAsync(int id);
        Task<Category> GetAsync(string name);
        int Delete(int id);
        int DeleteSubCategory(int id);
        int Delete(string name);

        Task <List<Product>> ListProductCategoryAll();

        string GetName(int id);

        List<Product> ListProductInCategory(string CategoryName);
    }
}
