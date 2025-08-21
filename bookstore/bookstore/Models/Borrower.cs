using System.ComponentModel.DataAnnotations;

namespace bookstore.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public virtual ICollection<BorrowerBooks> Books { get; set; }
    }
}
