using Store.DataAccess.Data;
using Store.DataAccess.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public ICategoryRepository Category { get; private set; }
        public UnitOfWork(ApplicationDbContext db, ICategoryRepository categoryRepository)
        {
            this.db = db;
            this.Category = categoryRepository;
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
