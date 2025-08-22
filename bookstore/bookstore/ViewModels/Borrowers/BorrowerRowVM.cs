namespace bookstore.ViewModels.Borrowers
{
    public class BorrowerRowVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SimpleBookVM> Books { get; set; }
    }
    public class SimpleBookVM
    {
        public string Title { get; set; }
    }
}
