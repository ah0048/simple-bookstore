using bookstore.Helpers;
using bookstore.ViewModels.Books;

namespace bookstore.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResult> AddNewBook(AddBookVM addBookVM);
        Task<ServiceResult<List<BookCardVM>>> GetBookList(int pageNumber);
        Task<ServiceResult<BookDetailsVM>> GetBookDetails(int bookId);
    }
}
