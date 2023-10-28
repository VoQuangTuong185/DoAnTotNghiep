using Microsoft.EntityFrameworkCore;
using WebAppAPI.Data;
using WebAppAPI.Models.Bases;
using System.Threading.Tasks;

namespace WebAppAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly Dictionary<Type, object> repositories;
        public UnitOfWork(ApplicationDbContext ApplicationDbContext)
        {
            _ApplicationDbContext = ApplicationDbContext;
            repositories = new Dictionary<Type, object>();
         }
        public bool SaveChanges()
        {
            return _ApplicationDbContext.SaveChanges() >= 1;
        }

        public void BeginTransaction()
        {
            _ApplicationDbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _ApplicationDbContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _ApplicationDbContext.Database.RollbackTransaction();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : DbEntity
        {
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories.Add(type, new APIRepository<TEntity>(_ApplicationDbContext));
            }
            return (IRepository<TEntity>)repositories[type];
        }

        public void Disposing(bool disposing)
        {
            if(disposing && _ApplicationDbContext != null)
            {
                _ApplicationDbContext.Dispose();
            }
        }

        public void ExcuteSqlRaw(string sql)
        {
            if(_ApplicationDbContext != null)
            {
                _ApplicationDbContext.Database.ExecuteSqlRaw(sql);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _ApplicationDbContext.SaveChangesAsync()) >= 0;
        }
    }
}
