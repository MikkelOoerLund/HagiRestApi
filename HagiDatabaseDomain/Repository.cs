using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HagiDatabaseDomain
{
    public class Repository<TEntity> : IDisposable where TEntity : class
    {
        protected DbContext DbContext { get; }
        protected DbSet<TEntity> DbSet { get; }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        ~Repository()
        {
            Dispose();
        }

        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public List<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void SetValues(TEntity target, TEntity valueContainer)
        {
            DbContext.Entry(target)
                   .CurrentValues
                   .SetValues(valueContainer);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public async Task<List<TEntity>> FindEntitiesAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FindEntityWithIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }



        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
