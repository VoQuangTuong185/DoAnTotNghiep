using Microsoft.CodeAnalysis;
using DoAnTotNghiep.Models.Entities;
using WebAppAPI.Models.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DoAnTotNghiep.Data
{
    public interface IProductRepo
    {
        bool SaveChanges();

        // Category
        IEnumerable<Category> GetAllCategory();
        void CreateCategory(Category category);
        Category GetCategoryById(int categoryId);
        bool CategoryExits(int categoryId);
        bool ExternalCategoryExists(int externalCategoryId);
        void UpdateCategory(Category category);

        // Products
        IEnumerable<Product> GetProductsForPlatform(int categoryId);
        Product GetProduct(int categoryId, int productId);
        void CreateProduct(int categoryId, Product Product);
    }
}
