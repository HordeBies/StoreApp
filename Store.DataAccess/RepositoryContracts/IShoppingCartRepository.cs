using Store.Models;

namespace Store.DataAccess.RepositoryContracts
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}
