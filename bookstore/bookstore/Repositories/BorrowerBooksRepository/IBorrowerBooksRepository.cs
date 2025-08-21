using bookstore.Models;

namespace bookstore.Repositories.BorrowerBooksRepository
{
    public interface IBorrowerBooksRepository
    {
        Task AddAsync(BorrowerBooks obj);
        Task EditAsync(BorrowerBooks obj);
        Task<BorrowerBooks?> GetByIdsAsync(int borrowerId, int bookId);
        Task<List<BorrowerBooks>> GetByBorrowerIdAsync(int borrowerId);
        Task<List<BorrowerBooks>> GetByBookIdAsync(int bookId);
        Task DeleteAsync(int borrowerId, int bookId);

    }
}
