using bookstore.ViewModels.Books;
using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels.Borrowers
{
    public class BorrowerDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BorrowerBookVM> Books { get; set; }
    }

    public class BorrowerBookVM
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string CoverUrl { get; set; }
        public int Copies { get; set; }
        public DateTime LastBorrowDate { get; set; }
        public DateTime? LastReturnDate { get; set; }
    }
}
