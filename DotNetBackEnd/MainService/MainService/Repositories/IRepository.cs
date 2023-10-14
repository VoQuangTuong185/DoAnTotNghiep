using System.Linq.Expressions;
using WebAppAPI.Models.Bases;

namespace WebAppAPI.Repositories
{
    public interface IRepository <TEntity> where TEntity : DbEntity
    {
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        public bool Any(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        TEntity Find(int id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> All();
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetNoTracking(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        void Detached(IEnumerable<TEntity> entities);
    }
}
