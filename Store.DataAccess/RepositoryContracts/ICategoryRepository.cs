using Store.Models;

namespace Store.DataAccess.RepositoryContracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}
