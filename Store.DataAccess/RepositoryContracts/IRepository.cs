using System.Linq.Expressions;

namespace Store.DataAccess.RepositoryContracts
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string? includeProperties = null);
        Task<T?> GetFirstOrDefault(Expression<Func<T,bool>> query, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
