using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal readonly ApplicationDbContext db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            this.dbSet = db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null) // Comma separated, Case Sensitive (e.g. "Category,Product") include properties
        {
            IQueryable<T> dbSetQuery = dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbSetQuery = dbSetQuery.Include(includeProperty);
                }
            }
            return await dbSetQuery.ToListAsync();
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> query, string? includeProperties = null)
        {

            IQueryable<T> dbSetQuery = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbSetQuery = dbSetQuery.Include(includeProperty);
                }
            }
            return await dbSetQuery.FirstOrDefaultAsync(query);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
