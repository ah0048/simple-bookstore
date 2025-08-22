using bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace bookstore.Repositories.BookRepository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;
        public BookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Book obj)
        {
            await _dbContext.Books.AddAsync(obj);
        }

        public async Task DeleteAsync(int id)
        {
            Book? book = await GetByIdAsync(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
            }
        }

        public Task EditAsync(Book obj)
        {
            if (obj != null)
            {
                _dbContext.Attach(obj);
                _dbContext.Entry(obj).State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _dbContext.Books.OrderBy(b => b.Title).ToListAsync();
        }

        public async Task<List<Book>> GetByPageAsync(int pageNumber = 1)
        {
            int pageSize = 20;
            return await _dbContext.Books
                .OrderBy(b => b.Title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)                    
                .ToListAsync();
        }
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _dbContext.Books.FindAsync(id);
        }

        public async Task<Book?> GetBookWithBorrowersAsync(int id)
        {
            return await _dbContext.Books
                .Where(b => b.Id == id)
                .Include(b => b.Borrowers)
                    .ThenInclude(bb => bb.Borrower)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsDuplicateISBN(string ISBN)
        {
            return await _dbContext.Books.AnyAsync(b => b.ISBN == ISBN);
        }
    }
}
