using Microsoft.CodeAnalysis;
using System;
using System.ComponentModel.Design;
using THUCTAPTOTNGHIEP.Models.Entities;
using WebAppAPI.Data;
using WebAppAPI.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace THUCTAPTOTNGHIEP.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CategoryExits(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }
        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.ExternalID == categoryId);
        }
        public void CreateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Add(category);
        }
        public void CreateProduct(int categoryId, Product Product)
        {
            if (Product == null)
            {
                throw new ArgumentNullException(nameof(Product));
            }

            Product.CategoryId = categoryId;
            _context.Products.Add(Product);
        }
        public void UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Update(category);
        }
        public bool ExternalCategoryExists(int externalCategoryId)
        {
            return _context.Categories.Any(c => c.ExternalID == externalCategoryId);
        }
        public IEnumerable<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
        }
        public Product GetProduct(int categoryId, int productId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId && p.Id == productId).FirstOrDefault();
        }
        public IEnumerable<Product> GetProductsForPlatform(int categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId).OrderBy(c => c.category.CategoryName);
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
