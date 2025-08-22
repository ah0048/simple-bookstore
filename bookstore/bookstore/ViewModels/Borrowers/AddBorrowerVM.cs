using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels.Borrowers
{
    public class AddBorrowerVM
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
