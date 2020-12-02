using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RecipeSearch.DataAccess.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Models.BaseModel
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Add(JToken entity)
        {
            Add(entity.ToObject<T>());
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
           
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

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
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

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            Remove(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            dbSet.AddRange(entity);
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
