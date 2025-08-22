using AutoMapper;
using bookstore.Helpers;
using bookstore.Models;
using bookstore.Services.PhotoService;
using bookstore.UnitOfWorks;
using bookstore.ViewModels.Books;
using bookstore.ViewModels.Borrowers;
using System.Linq.Expressions;

namespace bookstore.Services.BorrowerService
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public BorrowerService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<ServiceResult> AddNewBorrower(AddBorrowerVM addBorrowerVM)
        {
            if (!string.IsNullOrWhiteSpace(addBorrowerVM.Name))
            {
                string[] nameList = addBorrowerVM.Name.Split();

                string cleanedName = string.Join(" ",
                    nameList.Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(item => char.ToUpper(item[0]) + item.Substring(1).ToLower())
                );

                bool nameExists = await _unit.BorrowerRepo.IsDuplicateName(cleanedName);
                if (nameExists)
                    return ServiceResult.CreateError("This Name already exists !!");

                Borrower newBorrower = new Borrower { Name = cleanedName };

                try
                {
                    await _unit.BorrowerRepo.AddAsync(newBorrower);
                    await _unit.SaveAsync();

                    return ServiceResult.CreateSuccess("The new borrower was added successfully");
                }
                catch (Exception ex)
                {
                    return ServiceResult.CreateError($"An error occurred while adding the borrower: {ex.Message}");
                }
            }

            return ServiceResult.CreateError("Invalid borrower name.");
        }

        public async Task<ServiceResult> BorrowBook(ActionBookVM borrowBookVM)
        {
            Book? requiredBook = await _unit.BookRepo.GetByIdAsync(borrowBookVM.BookId);
            if (requiredBook == null)
                return ServiceResult.CreateError("The required book wasn't found");

            if (requiredBook.AvailableCopies == 0)
                return ServiceResult.CreateError("There's no available copies of this book at the moment");

            Borrower? requiredBorrower = await _unit.BorrowerRepo.GetByIdAsync(borrowBookVM.BorrowerId);
            if (requiredBorrower == null)
                return ServiceResult.CreateError("The required borrower wasn't found");

            BorrowerBooks? borrowerBooks = await _unit.BorrowerBooksRepo.GetByIdsAsync(borrowBookVM.BorrowerId, borrowBookVM.BookId);

            try
            {
                if (borrowerBooks == null)
                {
                    BorrowerBooks newBorrowerBooks = new BorrowerBooks
                    {
                        BookId = borrowBookVM.BookId,
                        BorrowerId = borrowBookVM.BorrowerId,
                        Copies = 1
                    };
                    await _unit.BorrowerBooksRepo.AddAsync(newBorrowerBooks);
                }
                else
                {
                    borrowerBooks.Copies += 1;
                    borrowerBooks.BorrowDate = DateTime.UtcNow;
                    await _unit.BorrowerBooksRepo.EditAsync(borrowerBooks);
                }

                requiredBook.AvailableCopies -= 1;
                await _unit.BookRepo.EditAsync(requiredBook);
                await _unit.SaveAsync();
                return ServiceResult.CreateSuccess($"{requiredBorrower.Name} borrowed {requiredBook.Title} successfully !!");
            }
            catch (Exception ex)
            {
                return ServiceResult.CreateError($"An error occurred while borrowing the book: {ex.Message}");
            }
        }

        public async Task<ServiceResult> ReturnBook(ActionBookVM returnBookVM)
        {
            Book? requiredBook = await _unit.BookRepo.GetByIdAsync(returnBookVM.BookId);
            if (requiredBook == null)
                return ServiceResult.CreateError("The required book wasn't found");

            if (requiredBook.AvailableCopies == requiredBook.TotalCopies)
                return ServiceResult.CreateError("All copies of this book already exists, Please make sure you are selecting the right book");

            Borrower? requiredBorrower = await _unit.BorrowerRepo.GetByIdAsync(returnBookVM.BorrowerId);
            if (requiredBorrower == null)
                return ServiceResult.CreateError("The required borrower wasn't found");

            BorrowerBooks? borrowerBooks = await _unit.BorrowerBooksRepo.GetByIdsAsync(returnBookVM.BorrowerId, returnBookVM.BookId);

            if (borrowerBooks == null || borrowerBooks.Copies == 0)
                return ServiceResult.CreateError("The borrower doesn't have any copies of this book");

            try
            {
                if (borrowerBooks.Copies == 1)
                    await _unit.BorrowerBooksRepo.DeleteAsync(returnBookVM.BorrowerId, returnBookVM.BookId);
                else
                {
                    borrowerBooks.Copies -= 1;
                    borrowerBooks.ReturnDate = DateTime.UtcNow;
                    await _unit.BorrowerBooksRepo.EditAsync(borrowerBooks);
                }

                requiredBook.AvailableCopies += 1;
                await _unit.BookRepo.EditAsync(requiredBook);
                await _unit.SaveAsync();
                return ServiceResult.CreateSuccess($"{requiredBorrower.Name} returned {requiredBook.Title} successfully !!");
            }
            catch (Exception ex)
            {
                return ServiceResult.CreateError($"An error occurred while returning the book: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<BorrowerRowVM>>> GetBorrowerList(int pageNumber)
        {
            try
            {
                List<Borrower> borrowers = await _unit.BorrowerRepo.GetWithBooksByPageAsync(pageNumber);
                if (borrowers == null || borrowers.Count == 0)
                    return ServiceResult<List<BorrowerRowVM>>.CreateError("No Borrowers were Found");

                List<BorrowerRowVM> borrowerRowVMs = new List<BorrowerRowVM>();
                foreach (var borrower in borrowers)
                {
                    borrowerRowVMs.Add(new BorrowerRowVM
                    {
                        Id = borrower.Id,
                        Name = borrower.Name,
                        Books = borrower.Books
                                .Select(bb => new SimpleBookVM
                                {
                                    Title = bb.Book.Title
                                })
                                .ToList()
                    });
                }
                return ServiceResult<List<BorrowerRowVM>>.CreateSuccess(borrowerRowVMs);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<BorrowerRowVM>>.CreateError($"An error occurred while fetching borrowers data: {ex.Message}");
            }
        }

        public async Task<ServiceResult<BorrowerDetailsVM>> GetBorrowerDetails(int borrowerId)
        {
            try
            {
                Borrower? borrower = await _unit.BorrowerRepo.GetBorrowerWithBooksAsync(borrowerId);
                if (borrower == null)
                    return ServiceResult<BorrowerDetailsVM>.CreateError("No Borrower was Found");

                BorrowerDetailsVM borrowerDetailsVM = new BorrowerDetailsVM
                {
                    Id = borrower.Id,
                    Name = borrower.Name,
                    Books = borrower.Books
                                .Select(bb => new BorrowerBookVM
                                {
                                    Author = bb.Book.Author,
                                    Title = bb.Book.Title,
                                    CoverUrl = bb.Book.CoverUrl,
                                    Copies = bb.Copies,
                                    LastBorrowDate = bb.BorrowDate,
                                    LastReturnDate = bb.ReturnDate
                                })
                                .ToList()
                };

                return ServiceResult<BorrowerDetailsVM>.CreateSuccess(borrowerDetailsVM);
            }
            catch (Exception ex)
            {
                return ServiceResult<BorrowerDetailsVM>.CreateError($"An error occurred while fetching borrower data: {ex.Message}");
            }
        }
    }
}
