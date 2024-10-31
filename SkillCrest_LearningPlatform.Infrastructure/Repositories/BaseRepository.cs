using Microsoft.EntityFrameworkCore;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;

namespace SkillCrest_LearningPlatform.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext context;
        private DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext _context)
        {
            this.context = _context;
            this.dbSet = this.context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await this.dbSet.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await this.GetByIdAsync(id);

            if (entity != null)
            {
                this.dbSet.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToArrayAsync();
        }
        
        public IQueryable<T> GetAllAttached()
        {
            return this.dbSet.AsQueryable();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            T? entity = await this.dbSet.FindAsync(id);

            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                this.dbSet.Attach(entity);
                this.context.Entry(entity).State = EntityState.Modified;
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
