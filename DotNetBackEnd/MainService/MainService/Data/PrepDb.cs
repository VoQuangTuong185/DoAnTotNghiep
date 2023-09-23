using Microsoft.CodeAnalysis;
using THUCTAPTOTNGHIEP.Models.Entities;
using THUCTAPTOTNGHIEP.SyncDataServices.Grpc;
using WebAppAPI.Services.Business;
using WebAppAPI.Services.Contracts;

namespace THUCTAPTOTNGHIEP.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<ICategoryDataClient>();

                var categories = grpcClient.ReturnAllCategory();

                SeedData(serviceScope.ServiceProvider.GetService<IProductRepo>(), categories);
            }
        }

        private static void SeedData(IProductRepo repo, IEnumerable<Category> categories)
        {
            Console.WriteLine("Seeding new categories...");

            foreach (var category in categories)
            {
                if (!repo.ExternalCategoryExists(category.ExternalID))
                {
                    repo.CreateCategory(category);
                }
                repo.SaveChanges();
            }
        }
    }
}
