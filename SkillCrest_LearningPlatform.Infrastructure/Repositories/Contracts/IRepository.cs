using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(string id);

        IQueryable<T> GetAllAttached();
   
    }
}
