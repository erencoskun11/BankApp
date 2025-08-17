using BankAppDomain;
using BankAppDomain.Entities;
using Microsoft.EntityFrameworkCore;
using BankApp.Infrastructure.Data;

namespace BankApp.Infrastructure.Repositories
{
    public abstract class EfCoreRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext context;
        protected readonly DbSet<TEntity> _dbSet;

        public EfCoreRepository(TContext context)
        {
            this.context = context;
            _dbSet = context.Set<TEntity>();
        }


        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null) return null;

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteAsync(TEntity entity)  
        {
            _dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(int id,TEntity updatedEntity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
                throw new Exception($"Entity with id {id} not found.");

            context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

            await context.SaveChangesAsync();
        }
       
        public async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await context.SaveChangesAsync();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            {
                _dbSet.Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }

    public class EfCoreRepository<TEntity> : EfCoreRepository<TEntity, BankDbContext>, IRepository<TEntity>
        where TEntity : class, IEntity
    {
        public EfCoreRepository(BankDbContext context) : base(context) { }
    }
}
