using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookstore.Models
{
    [PrimaryKey(nameof(BorrowerId), nameof(BookId))]
    public class BorrowerBooks
    {
        [ForeignKey(nameof(Borrower))]
        public int BorrowerId { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Borrower Borrower { get; set; }
        public virtual Book Book { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Copies { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
    }
}
