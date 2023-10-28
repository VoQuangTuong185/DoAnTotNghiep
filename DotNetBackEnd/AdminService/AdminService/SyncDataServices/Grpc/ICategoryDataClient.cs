using Microsoft.CodeAnalysis;
using DoAnTotNghiep.Models.Entities;

namespace DoAnTotNghiep.SyncDataServices.Grpc
{
    public interface ICategoryDataClient
    {
        IEnumerable<Category> ReturnAllCategory();
    }
}
