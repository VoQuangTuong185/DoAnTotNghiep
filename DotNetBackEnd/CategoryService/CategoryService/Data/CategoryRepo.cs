using CategoryService.Data.Entities;
using Optional;
using Optional.Async.Extensions;

namespace CategoryService.Data
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;       
        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Option<bool, string>> CreateCategory(Category category)
        {
            return await (category)
                .SomeNotNull().WithException("Null input")
                .FlatMapAsync(async req =>
                {
                    var existedCategoryName = _context.Categories.FirstOrDefault(c => c.CategoryName.ToUpper().TrimStart().TrimEnd() == category.CategoryName.ToUpper().TrimStart().TrimEnd());
                    if (existedCategoryName != null)
                    {
                        return Option.None<bool, string>("Đã tồn tại danh mục này. Hãy thử lại!");
                    }
                    _context.Categories.Add(category);
                    if (SaveChanges())
                    {
                        return Option.Some<bool, string>(true);
                    }
                    return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
                });
        }
        public async Task<Option<bool, string>> UpdateCategory(Category category)
        {
            return await (category)
               .SomeNotNull().WithException("Null input")
               .FlatMapAsync(async req =>
               {
                   var allCategory = GetAllCategories("admin");
                   var existedCategoryName = allCategory.FirstOrDefault(c => c.CategoryName.ToUpper().TrimStart().TrimEnd() == category.CategoryName.ToUpper().TrimStart().TrimEnd() && c.Id != category.Id);
                   if (existedCategoryName != null)
                   {
                       return Option.None<bool, string>("Đã tồn tại danh mục này. Hãy thử lại!");
                   }
                   var existedCategory = allCategory.FirstOrDefault(c => c.Id == category.Id);
                   if (existedCategory != null)
                   {
                       existedCategory.CategoryName = category.CategoryName;
                       existedCategory.Description = category.Description;
                       existedCategory.Image = category.Image;
                   }
                   _context.Categories.Update(existedCategory);
                   if (SaveChanges())
                   {
                       return Option.Some<bool, string>(true);
                   }
                   return Option.None<bool, string>("Đã xảy ra lỗi trong quá trình xử lý. Hãy thử lại!");
               });
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
