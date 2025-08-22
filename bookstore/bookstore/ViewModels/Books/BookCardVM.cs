using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels.Books
{
    public class BookCardVM
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string CoverUrl { get; set; }
    }
}
