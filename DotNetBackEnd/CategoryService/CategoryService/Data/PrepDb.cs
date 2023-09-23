using CategoryService.Data.Entities;

namespace CategoryService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        private static void SeedData(AppDbContext context)
        {

            if (!context.Categories.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Categories.AddRange(
                    new Category() { CategoryName = "Dot Net", Image = "Microsoft", Description = "Free" },
                    new Category() { CategoryName = "SQL Server Express", Image = "Microsoft", Description = "Free" },
                    new Category() { CategoryName = "Kubernetes", Image = "Cloud Native Computing Foundation", Description = "Free" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
