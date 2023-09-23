using CategoryService.Data.Entities;

namespace CategoryService.Data
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Add(category);
        }
        public Category UpdateCategory(Category category)
        {
            var existedCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existedCategory != null)
            {
                existedCategory.CategoryName = category.CategoryName;
                existedCategory.Description = category.Description;
                existedCategory.Image = category.Image;
            }
            _context.Categories.Update(existedCategory);
            SaveChanges();
            return existedCategory;
        }
        public IEnumerable<Category> GetAllCategories(string type)
        {
            if (type == "admin")
            {
                return _context.Categories.ToList();
            }
            else
            {
                return _context.Categories.Where(x => x.IsActive);
            }
        }
        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }
        public Category InactiveCategory(int categoryId)
        {
            var existedCategory = _context.Categories.FirstOrDefault(p => p.Id == categoryId);
            if (existedCategory != null)
            {
                existedCategory.IsActive = !existedCategory.IsActive;
            }
            _context.Categories.Update(existedCategory);
            SaveChanges();
            return existedCategory;
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
