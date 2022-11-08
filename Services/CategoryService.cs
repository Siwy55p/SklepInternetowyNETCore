using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewComponents;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace partner_aluro.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSave(Category category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public int Delete(int id)
        {
            Category category = _context.Category
                .Include(x => x.SubCategories).FirstOrDefault(x => x.CategoryId == id);


            _context.Category.Remove(category);

            _context.SaveChanges();
            return id;
        }
        public int DeleteSubCategory(int id)
        {
            SubCategory subCategory = _context.SubCategory.FirstOrDefault(x => x.SubCategoryId == id);

            _context.SubCategory.Remove(subCategory);

            _context.SaveChanges();
            return id;
        }
        public int Delete(string name)
        {
            Category category = _context.Category.Include(x => x.SubCategories).FirstOrDefault(x => x.Name == name);
            int id = category.CategoryId;

            _context.Category.Remove(category);

            _context.SaveChanges();
            return id;
        }

        public async Task<Category> GetAsync(int id)
        {
            var category = await _context.Category
                .Include(p => p.Produkty)
                .Include(sc => sc.SubCategories)
                .FirstOrDefaultAsync(x => x.CategoryId == id);

            return category;
        }
        public async Task<Category> GetAsync(string name)
        {

            var category = await _context.Category
                .Include(p => p.Produkty)
                .Include(sc => sc.SubCategories)
                .FirstOrDefaultAsync(x => x.Name == name);


            return category;
        }

        public async Task<List<Category>> List()
        {
            var category = await _context.Category
                .Include(p => p.Produkty)
                .Include(sc => sc.SubCategories)
                .ToListAsync();


            //List<SubCategory> SubCategory = _context.SubCategory
            //    .Include(sp => sp.Produkty)
            //    .Where(x => x.SubCatId == category.CategoryId).ToList();

            //category.s = SubCategory;

            return category;
        }
        public List<Category> GetList()
        {
            List<Category> category = _context.Category.ToList();

            return category;
        }

        public async Task<List<Category>> List(string name)
        {
            var category = await _context.Category.ToListAsync();
            category.Find(a => a.Name == name);

            return category;
        }

        public async Task<List<Product>> ListProductCategoryAll()
        {
            return await _context.Products.ToListAsync(); 
        }

        public List<Product> ListProductInCategory(string CategoryName)
        {
            List<Product> pro = _context.Products.Where(k => k.CategoryNavigation.Name == CategoryName).ToList();
            return pro;
        }

        public int Save(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();

            return category.CategoryId;
        }

        public Category Update(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();

            return category;
        }

        public async Task AddSave(SubCategory category)
        {
            await _context.SubCategory.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public string GetName(int id)
        {
            List<Category> list = _context.Category.ToList();
            string name =list.Find(x => x.CategoryId == id).Name;
            return name;
        }

        public string GetNameSub(int id)
        {
            List<SubCategory> list = _context.SubCategory.ToList();
            string name = list.Find(x => x.SubCategoryId == id).Name;
            return name;
        }
    }
}
