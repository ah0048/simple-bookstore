using bookstore.Helpers;
using bookstore.ViewModels.Borrowers;

namespace bookstore.Services.BorrowerService
{
    public interface IBorrowerService
    {
        Task<ServiceResult> AddNewBorrower(AddBorrowerVM addBorrowerVM);
        Task<ServiceResult> BorrowBook(ActionBookVM borrowBookVM);
        Task<ServiceResult> ReturnBook(ActionBookVM returnBookVM);
        Task<ServiceResult<List<BorrowerRowVM>>> GetBorrowerList(int pageNumber);
        Task<ServiceResult<BorrowerDetailsVM>> GetBorrowerDetails(int borrowerId);
    }
}
