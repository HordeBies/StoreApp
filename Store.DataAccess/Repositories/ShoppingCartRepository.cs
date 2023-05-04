using Microsoft.EntityFrameworkCore;
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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(ShoppingCart obj)
        {
            //var objFromDb = db.ShoppingCarts.FirstOrDefault(x => x.Id == obj.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Count = obj.Count;
            //    objFromDb.ProductId = obj.ProductId;
            //    db.ShoppingCarts.Update(objFromDb);
            //}
            db.ShoppingCarts.Update(obj);
        }
    }
}
