using CategoryService.DTO;

namespace CategoryService.SyncDataServices.Http
{
    public interface IProductDataClient
    {
        Task SendCategoryToProduct(CategoryReadDto category);
    }
}
