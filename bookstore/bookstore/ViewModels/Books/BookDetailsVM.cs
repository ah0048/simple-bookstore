using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels.Books
{
    public class BookDetailsVM
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string CoverUrl { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public ICollection<BookBorrowerVM> BookBorrowers { get; set; }
    }

    public class BookBorrowerVM
    {
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; }
        public int BorrowedCopies { get; set; }
    }
}
