using bookstore.Models;

namespace bookstore.Repositories.BorrowerRepository
{
    public interface IBorrowerRepository: IRepository<Borrower>
    {
        Task<List<Borrower>> GetByPageAsync(int pageNumber = 1);
        Task<List<Borrower>> GetWithBooksByPageAsync(int pageNumber = 1);
        Task<Borrower?> GetBorrowerWithBooksAsync(int id);
        Task<bool> IsDuplicateName(string name);
    }
}
