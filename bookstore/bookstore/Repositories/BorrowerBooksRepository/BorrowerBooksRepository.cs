using bookstore.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace bookstore.Repositories.BorrowerBooksRepository
{
    public class BorrowerBooksRepository : IBorrowerBooksRepository
    {
        private readonly AppDbContext _dbContext;
        public BorrowerBooksRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(BorrowerBooks obj)
        {
            await _dbContext.BorrowerBooks.AddAsync(obj);
        }

        public async Task DeleteAsync(int borrowerId, int bookId)
        {
            BorrowerBooks? borrowerBook = await GetByIdsAsync(borrowerId, bookId);
            if (borrowerBook != null)
            {
                _dbContext.BorrowerBooks.Remove(borrowerBook);
            }
        }

        public Task EditAsync(BorrowerBooks obj)
        {
            if (obj != null)
            {
                _dbContext.Attach(obj);
                _dbContext.Entry(obj).State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }

        public async Task<List<BorrowerBooks>> GetByBookIdAsync(int bookId)
        {
            return await _dbContext.BorrowerBooks.Where(bb => bb.BookId == bookId).ToListAsync();
        }

        public async Task<List<BorrowerBooks>> GetByBorrowerIdAsync(int borrowerId)
        {
            return await _dbContext.BorrowerBooks.Where(bb => bb.BorrowerId == borrowerId).ToListAsync();
        }

        public async Task<BorrowerBooks?> GetByIdsAsync(int borrowerId, int bookId)
        {
            return await _dbContext.BorrowerBooks.FirstOrDefaultAsync(bb => bb.BorrowerId == borrowerId && bb.BookId == bookId);
        }
    }
}
