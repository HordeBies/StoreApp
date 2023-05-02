using System.Linq.Expressions;

namespace Store.DataAccess.RepositoryContracts
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetFirstOrDefault(Expression<Func<T,bool>> query);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
