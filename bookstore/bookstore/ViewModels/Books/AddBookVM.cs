using bookstore.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels.Books
{
    public class AddBookVM
    {
        [Required]
        [StringLength(13, ErrorMessage ="ISBN is required")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN must be exactly 13 digits.")]
        public string ISBN { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Author is required")]
        public string Author { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required]
        [AllowedImageExtensions(".jpg,.jpeg,.png,.gif,.bmp,.webp", 5)]
        public IFormFile CoverPic { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total Copies is required and it's a positive number greater than 0")]
        public int TotalCopies { get; set; }
    }
}
