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

        IEnumerable<T> GetAll();

        Task<T> GetByIdAsync(string id);

        T GetById(string id);

        Task AddAsync(T entity);

        void Add(T entity);

        Task<bool> UpdateAsync(T entity);

        bool Update(T entity);

        Task<bool> DeleteAsync(string id);

        bool Delete(string id);
    }
}
