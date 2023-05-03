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
    internal class CompanyProductRepository : Repository<CompanyProduct>, ICompanyProductRepository
    {
        public CompanyProductRepository(ApplicationDbContext db) : base(db)
        {
        }
        public void Update(CompanyProduct obj)
        {
            var objFromDb = db.CompanyProducts.FirstOrDefault(s => s.ProductId == obj.ProductId && s.CompanyId == obj.CompanyId);
            if (objFromDb != null)
            {
                objFromDb.Price = obj.Price;
                db.CompanyProducts.Update(objFromDb);
            }
        }
    }
}
