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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Product product)
        {
            var objFromDb = db.Products.FirstOrDefault(s => s.Id == product.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.ISBN = product.ISBN;
                objFromDb.Author = product.Author;
                objFromDb.CategoryID = product.CategoryID;
                if (product.ImageURL != null)
                {
                    objFromDb.ImageURL = product.ImageURL;
                }
                db.Products.Update(objFromDb);
            }
        }
    }
}
