using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace bookstore.Models
{
    public class AppDbContext: DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Borrower> Borrowers { get; set; }
        public virtual DbSet<BorrowerBooks> BorrowerBooks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
