using bookstore.Repositories.BookRepository;
using bookstore.Repositories.BorrowerBooksRepository;
using bookstore.Repositories.BorrowerRepository;

namespace bookstore.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepo { get; }
        IBorrowerRepository BorrowerRepo { get; }
        IBorrowerBooksRepository BorrowerBooksRepo { get; }
        Task SaveAsync();
        void Dispose();
    }
}
