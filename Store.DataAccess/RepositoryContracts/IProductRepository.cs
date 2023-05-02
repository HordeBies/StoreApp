using Store.DataAccess.Repositories;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.RepositoryContracts
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
