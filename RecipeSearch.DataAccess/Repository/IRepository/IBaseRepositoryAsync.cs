using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IBaseRepositoryAsync<T> where T : Models.BaseModel
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
                bool noTracking = false,
                string includeProperties = null
            );
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, bool noTracking = false, string includeProperties = null);
        Task AddAsync(T entity);
        Task AddAsync(JToken entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task RemoveAsync(int id);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entity);
        void Save();
    }
}
