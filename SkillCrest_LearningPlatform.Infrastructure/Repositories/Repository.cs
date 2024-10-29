using Microsoft.EntityFrameworkCore;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext context, DbSet<T> dbSet)
        {
            this.context = context;
            this.dbSet = dbSet;
        }

        public void Add(T entity)
        {
            this.dbSet.Add(entity);
            this.context.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            await this.dbSet.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public bool Delete(string id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.dbSet.Remove(entity);
                this.context.SaveChanges();
                return true;
            }

            return false;
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

        public IEnumerable<T> GetAll()
        {
            return this.dbSet.ToArray();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public T? GetById(string id)
        {
            T? entity = this.dbSet.Find(id);

            return entity;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            T? entity = await this.dbSet.FindAsync(id);

            return entity;
        }

        public bool Update(T entity)
        {
            try
            {
                this.dbSet.Attach(entity);
                this.context.Entry(entity).State = EntityState.Modified;
                this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
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
