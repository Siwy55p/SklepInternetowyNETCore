﻿using Microsoft.EntityFrameworkCore;
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

        public int AddSave(Category category)
        {
            _context.Category.Add(category);
            var id = _context.SaveChanges();
            return id;
        }

        public int Delete(int id)
        {
            var category = _context.Category.Find(id);
            _context.Category.Remove(category);

            _context.SaveChanges();
            return id;
        }
        public int Delete(string name)
        {
            var category = _context.Category.Find(name);
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
            var cat = _context.Update(category);
            _context.SaveChanges();

            return category.CategoryId;
        }

        public Category Update(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();

            return category;
        }

        public int AddSave(SubCategory category)
        {
            _context.SubCategory.Add(category);
            var id = _context.SaveChanges();
            return id;
        }
    }
}
