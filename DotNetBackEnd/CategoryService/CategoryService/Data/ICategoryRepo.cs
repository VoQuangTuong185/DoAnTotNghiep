using CategoryService.Data.Entities;
using CategoryService.DTO;
using Optional;

namespace CategoryService.Data
{
    public interface ICategoryRepo
    {
        bool SaveChanges();
        IEnumerable<Category> GetAllCategories(string type);
        Category GetCategoryById(int id);
        Task<Option<bool, string>> CreateCategory(Category category);
        Category InactiveCategory(int categoryId);
        Task<Option<bool, string>> UpdateCategory(Category category);
    }
}
