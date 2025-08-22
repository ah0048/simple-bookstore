using AutoMapper;
using bookstore.Helpers;
using bookstore.Models;
using bookstore.Services.PhotoService;
using bookstore.UnitOfWorks;
using bookstore.ViewModels.Books;
using bookstore.ViewModels.Borrowers;

namespace bookstore.Services.BookService
{
    public class BookService: IBookService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public BookService(IUnitOfWork unit, IMapper mapper, IPhotoService photoService)
        {
            _unit = unit;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<ServiceResult> AddNewBook(AddBookVM addBookVM)
        {
            if (!string.IsNullOrWhiteSpace(addBookVM.ISBN))
            {
                bool isbnExists = await _unit.BookRepo.IsDuplicateISBN(addBookVM.ISBN);
                if (isbnExists)
                    return ServiceResult.CreateError("This ISBN already exists !!");
            }
            else
                return ServiceResult.CreateError("The ISBN cannot be empty !!");

            Book newBook = _mapper.Map<Book>(addBookVM);
            newBook.AvailableCopies = newBook.TotalCopies;

            if (addBookVM.CoverPic != null)
            {
                try
                {
                    var result = await _photoService.AddPhotoAsync(addBookVM.CoverPic);
                    if (result?.SecureUrl == null)
                        return ServiceResult.CreateError("Failed to upload Book Cover");

                    newBook.CoverUrl = result.SecureUrl.AbsoluteUri;
                    newBook.CoverPublicId = result.PublicId;
                }
                catch (Exception ex)
                {
                    return ServiceResult.CreateError($"Cover upload failed: {ex.Message}");
                }
            }

            try
            {
                await _unit.BookRepo.AddAsync(newBook);
                await _unit.SaveAsync();

                return ServiceResult.CreateSuccess("The new book was added successfully !!");
            }
            catch (Exception ex)
            {
                return ServiceResult.CreateError($"An error occurred while adding the book: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<BookCardVM>>> GetBookList(int pageNumber)
        {
            try
            {
                List<Book> bookList = await _unit.BookRepo.GetByPageAsync(pageNumber);
                if (bookList == null)
                    return ServiceResult<List<BookCardVM>>.CreateError("No Books were Found");
                if (!bookList.Any())
                    return ServiceResult<List<BookCardVM>>.CreateSuccess(new List<BookCardVM>());

                List<BookCardVM> bookCardVMs = _mapper.Map<List<BookCardVM>>(bookList);
                return ServiceResult<List<BookCardVM>>.CreateSuccess(bookCardVMs);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<BookCardVM>>.CreateError($"An error occurred while fetching books data: {ex?.InnerException?.Message}");
            }
        }

        public async Task<ServiceResult<BookDetailsVM>> GetBookDetails(int bookId)
        {
            try
            {
                Book? book = await _unit.BookRepo.GetBookWithBorrowersAsync(bookId);
                if (book == null)
                    return ServiceResult<BookDetailsVM>.CreateError("No Book was Found");

                BookDetailsVM bookDetailsVM = _mapper.Map<BookDetailsVM>(book);
                foreach (var bookBorrower in book.Borrowers)
                {
                    bookDetailsVM.BookBorrowers.Add(new BookBorrowerVM
                    {
                        BorrowerId = bookBorrower.BorrowerId,
                        BorrowerName = bookBorrower.Borrower.Name,
                        BorrowedCopies = bookBorrower.Copies
                    });
                }
                return ServiceResult<BookDetailsVM>.CreateSuccess(bookDetailsVM);
            }
            catch (Exception ex)
            {
                return ServiceResult<BookDetailsVM>.CreateError($"An error occurred while fetching book details: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<BookCardVM>>> SearchBooks(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return ServiceResult<List<BookCardVM>>.CreateSuccess(new List<BookCardVM>());

                List<Book> books = await _unit.BookRepo.SearchBooksAsync(searchTerm);
                List<BookCardVM> bookCardVMs = _mapper.Map<List<BookCardVM>>(books);
                
                return ServiceResult<List<BookCardVM>>.CreateSuccess(bookCardVMs);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<BookCardVM>>.CreateError($"An error occurred while searching books: {ex.Message}");
            }
        }
    }
}
