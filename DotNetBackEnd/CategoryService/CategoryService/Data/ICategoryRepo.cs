using CategoryService.Data.Entities;
using CategoryService.DTO;

namespace CategoryService.Data
{
    public interface ICategoryRepo
    {
        bool SaveChanges();
        IEnumerable<Category> GetAllCategories(string type);
        Category GetCategoryById(int id);
        void CreateCategory(Category category);
        Category InactiveCategory(int categoryId);
        Category UpdateCategory(Category category);
    }
}
