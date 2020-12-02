using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository
{
    public class BaseRepositoryAsync<T> : IBaseRepositoryAsync<T> where T : Models.BaseModel
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public BaseRepositoryAsync(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddAsync(JToken entity)
        {
            await AddAsync(entity.ToObject<T>());
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, bool noTracking = false, string includeProperties = null)
        {
            IQueryable<T> query = noTracking ? dbSet.AsNoTracking() : dbSet;
           
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, bool noTracking = false, string includeProperties = null)
        {
            IQueryable<T> query = noTracking ? dbSet.AsNoTracking() : dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(int id)
        {
            T entity = await dbSet.FindAsync(id);
            await RemoveAsync(entity);
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await dbSet.AddRangeAsync(entity);
        }
        public int? Lookup(string Term)
        {
            if (String.IsNullOrEmpty(Term))
            {
                return null;
            }

            return _db.Set<T>().Where(t =>
                    t.Name == Term
                )
                .Select(t => t.Id)
                .FirstOrDefault();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
