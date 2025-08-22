using bookstore.Models;

namespace bookstore.Repositories.BookRepository
{
    public interface IBookRepository: IRepository<Book>
    {
        Task<List<Book>> GetByPageAsync(int pageNumber = 1);
        Task<bool> IsDuplicateISBN(string ISBN);
        Task<Book?> GetBookWithBorrowersAsync(int id);
        Task<List<Book>> SearchBooksAsync(string searchTerm);
    }
}
