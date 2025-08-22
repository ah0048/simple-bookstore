using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace bookstore.Models
{
    [Index(nameof(ISBN), IsUnique = true)]
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(13)]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN must be exactly 13 digits.")]
        public string ISBN { get; set; }
        [Required]
        [StringLength(200)]
        public string Author { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        [Required]
        public string CoverUrl { get; set; }
        [Required]
        public string CoverPublicId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TotalCopies { get; set; }
        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }
        public virtual ICollection<BorrowerBooks> Borrowers { get; set; }

    }
}
