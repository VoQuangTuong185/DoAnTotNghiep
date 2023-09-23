using System.Threading.Tasks;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Repositories
{
    public interface IUnitOfWork
    {
        bool SaveChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        IRepository<TEntity> Repository<TEntity>() where TEntity : DbEntity;
        void Disposing(bool disposing);
        void ExcuteSqlRaw(string sql);
        Task<bool> SaveChangesAsync();
    }
}
