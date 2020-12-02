using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IBaseRepository<T> where T : Models.BaseModel
    {
        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
        void Add(T entity);
        void Add(JToken entity);
        void AddRange(IEnumerable<T> entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        void Save();
    }
}
