using CategoryService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CategoryService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}
