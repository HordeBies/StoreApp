using Store.Models;

namespace Store.DataAccess.RepositoryContracts
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
