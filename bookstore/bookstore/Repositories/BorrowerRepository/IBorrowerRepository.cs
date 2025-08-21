using bookstore.Models;

namespace bookstore.Repositories.BorrowerRepository
{
    public interface IBorrowerRepository: IRepository<Borrower>
    {
        Task<List<Borrower>> GetByPageAsync(int pageNumber = 1);
        Task<bool> IsDuplicateName(string name);
    }
}
