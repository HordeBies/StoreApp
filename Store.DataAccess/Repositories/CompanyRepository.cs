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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Company obj)
        {
            db.Companies.Update(obj);
        }
    }
}
