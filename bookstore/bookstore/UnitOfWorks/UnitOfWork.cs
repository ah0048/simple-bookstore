using bookstore.Models;
using bookstore.Repositories.BookRepository;
using bookstore.Repositories.BorrowerBooksRepository;
using bookstore.Repositories.BorrowerRepository;

namespace bookstore.UnitOfWorks
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IBookRepository? bookRepo;
        private IBorrowerRepository? borrowerRepo;
        private IBorrowerBooksRepository? borrowerBooksRepo;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBookRepository BookRepo
        {
            get
            {
                if (bookRepo == null)
                {
                    bookRepo = new BookRepository(_dbContext);
                }
                ;
                return bookRepo;
            }
        }
        public IBorrowerRepository BorrowerRepo
        {
            get
            {
                if (borrowerRepo == null)
                {
                    borrowerRepo = new BorrowerRepository(_dbContext);
                }
                ;
                return borrowerRepo;
            }
        }
        public IBorrowerBooksRepository BorrowerBooksRepo
        {
            get
            {
                if (borrowerBooksRepo == null)
                {
                    borrowerBooksRepo = new BorrowerBooksRepository(_dbContext);
                }
                ;
                return borrowerBooksRepo;
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
