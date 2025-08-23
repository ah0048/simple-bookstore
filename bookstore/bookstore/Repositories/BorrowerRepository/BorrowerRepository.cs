using bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace bookstore.Repositories.BorrowerRepository
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly AppDbContext _dbContext;
        public BorrowerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Borrower obj)
        {
            await _dbContext.Borrowers.AddAsync(obj);
        }

        public async Task DeleteAsync(int id)
        {
            Borrower? borrower = await GetByIdAsync(id);
            if (borrower != null)
            {
                _dbContext.Borrowers.Remove(borrower);
            }
        }

        public Task EditAsync(Borrower obj)
        {
            if (obj != null)
            {
                _dbContext.Attach(obj);
                _dbContext.Entry(obj).State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }

        public async Task<List<Borrower>> GetAllAsync()
        {
            return await _dbContext.Borrowers.OrderBy(b => b.Name).ToListAsync();
        }

        public async Task<Borrower?> GetByIdAsync(int id)
        {
            return await _dbContext.Borrowers.FindAsync(id);
        }

        public async Task<List<Borrower>> GetByPageAsync(int pageNumber = 1)
        {
            int pageSize = 20;
            return await _dbContext.Borrowers
                .OrderBy(b => b.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Borrower>> GetWithBooksByPageAsync(int pageNumber = 1)
        {
            int pageSize = 3;
            return await _dbContext.Borrowers
                .OrderBy(b => b.Name)
                .Include(b=> b.Books)
                    .ThenInclude(bb=> bb.Book)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Borrower?> GetBorrowerWithBooksAsync(int id)
        {
            return await _dbContext.Borrowers
                .Where(b => b.Id == id)
                .Include(b => b.Books)
                    .ThenInclude(bb => bb.Book)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> IsDuplicateName(string name)
        {
            return await _dbContext.Borrowers.AnyAsync(b => b.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Borrower>> SearchBorrowersAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return new List<Borrower>();

            return await _dbContext.Borrowers
                .Where(b => b.Name.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(b => b.Name)
                .Take(10)
                .ToListAsync();
        }
    }
}
