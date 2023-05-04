using Store.DataAccess.Data;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(OrderDetail obj)
        {
            db.OrderDetails.Update(obj);
        }
    }
}
