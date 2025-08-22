using bookstore.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace bookstore.ViewModels.Books
{
    public class AddBookVM
    {
        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }
        [Required]
        [StringLength(200)]
        public string Author { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        [Required]
        [AllowedImageExtensions(".jpg,.jpeg,.png,.gif,.bmp,.webp", 5)]
        public IFormFile CoverPic { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TotalCopies { get; set; }
    }
}
