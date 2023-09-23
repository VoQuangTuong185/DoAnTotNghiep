using Microsoft.CodeAnalysis;
using THUCTAPTOTNGHIEP.Models.Entities;

namespace THUCTAPTOTNGHIEP.SyncDataServices.Grpc
{
    public interface ICategoryDataClient
    {
        IEnumerable<Category> ReturnAllCategory();
    }
}
