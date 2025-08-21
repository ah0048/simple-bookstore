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
        public async Task<bool> IsDuplicateName(string name)
        {
            return await _dbContext.Borrowers.AnyAsync(b => b.Name == name);
        }
    }
}
