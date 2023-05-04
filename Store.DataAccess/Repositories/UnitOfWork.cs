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
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public ICompanyProductRepository CompanyProduct { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            this.Category = new CategoryRepository(db);
            this.Product = new ProductRepository(db);
            this.Company = new CompanyRepository(db);
            this.CompanyProduct = new CompanyProductRepository(db);
            this.ShoppingCart = new ShoppingCartRepository(db);
            this.ApplicationUser = new ApplicationUserRepository(db);
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
