using Store.Models;

namespace Store.DataAccess.RepositoryContracts
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}
