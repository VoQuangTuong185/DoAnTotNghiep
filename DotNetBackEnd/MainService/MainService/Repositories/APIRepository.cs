using WebAppAPI.Models.Bases;
using System.Threading.Tasks;
using System.Linq.Expressions;
using WebAppAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebAppAPI.Repositories
{
    public class APIRepository<TEntity> : IRepository<TEntity> where TEntity : DbEntity
    {
        protected readonly ApplicationDbContext _ApplicationDbContext;
        protected DbSet <TEntity> DbSet => _ApplicationDbContext.Set<TEntity>();
        public APIRepository(ApplicationDbContext ApplicationDbContext) {
            _ApplicationDbContext = ApplicationDbContext;
        }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }
        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public IQueryable<TEntity> All()
        {
            return DbSet.AsQueryable();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).Any();
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public TEntity Find(int id)
        {
            return DbSet.Find(id);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }
        public IQueryable<TEntity> GetNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate);
        }
        public void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            _ApplicationDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.AttachRange(entities);
            foreach(var en in entities)
            {
                _ApplicationDbContext.Entry(en).State = EntityState.Modified;
            }
        }

        public void Detached(IEnumerable<TEntity> entities)
        {
            foreach (var en in entities)
            {
                _ApplicationDbContext.Entry(en).State = EntityState.Detached;
            }
        }
    }
}
