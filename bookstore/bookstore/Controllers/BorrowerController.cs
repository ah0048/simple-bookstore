using bookstore.Services.BookService;
using bookstore.Services.BorrowerService;
using bookstore.ViewModels.Books;
using bookstore.ViewModels.Borrowers;
using Microsoft.AspNetCore.Mvc;

namespace bookstore.Controllers
{
    public class BorrowerController : Controller
    {
        private readonly IBorrowerService _borrowerService;
        private readonly IBookService _bookService;
        public BorrowerController(IBorrowerService borrowerService, IBookService bookService)
        {
            _borrowerService = borrowerService;
            _bookService = bookService;
        }

        public IActionResult NewBorrower()
        {
            return View("Add");
        }

        public async Task<IActionResult> AddNewBorrower(AddBorrowerVM addBorrowerVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _borrowerService.AddNewBorrower(addBorrowerVM);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.SuccessMessage;
                    return RedirectToAction("All");
                }
                else
                {
                    foreach (var error in result.ValidationErrors)
                        ModelState.AddModelError("", error);

                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                        ModelState.AddModelError("", result.ErrorMessage);
                }
            }

            return View("Add", addBorrowerVM);
        }

        public async Task<IActionResult> All(int pageNumber = 1)
        {
            var result = await _borrowerService.GetBorrowerList(pageNumber);
            if (result.Success)
            {
                ViewBag.page = pageNumber;
                return View("All", result.Data);
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("Error");
        }

        public async Task<IActionResult> BorrowerDetails(int id)
        {
            var result = await _borrowerService.GetBorrowerDetails(id);
            if (result.Success)
                return View("Details", result.Data);

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("Error");
        }

        public  IActionResult BorrowBookView(int id)
        {
            return View("BorrowBook");
        }

        public async Task<IActionResult> BorrowBook(ActionBookVM actionBookVM)
        {
            var result = await _borrowerService.BorrowBook(actionBookVM);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
                return RedirectToAction("All");
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("BorrowBook", actionBookVM);
        }

        public IActionResult ReturnBookView(int id)
        {
            return View("ReturnBook");
        }

        public async Task<IActionResult> ReturnBook(ActionBookVM actionBookVM)
        {
            var result = await _borrowerService.ReturnBook(actionBookVM);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
                return RedirectToAction("All");
            }

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("ReturnBook", actionBookVM);
        }

        // API endpoints for autocomplete
        [HttpGet]
        public async Task<IActionResult> SearchBooks(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new List<object>());

            var result = await _bookService.SearchBooks(term);
            if (result.Success && result.Data != null)
            {
                var suggestions = result.Data.Select(b => new { 
                    id = b.Id, 
                    text = $"{b.Title} by {b.Author}" 
                });
                return Json(suggestions);
            }
            
            return Json(new List<object>());
        }

        [HttpGet]
        public async Task<IActionResult> SearchBorrowers(string term)
     {
            try
            {
                if (string.IsNullOrEmpty(term) || term.Length < 2)
                    return Json(new List<object>());

                var result = await _borrowerService.SearchBorrowers(term);
                if (result.Success && result.Data != null && result.Data.Any())
                {
                    var suggestions = result.Data.Select(b => new { 
                        id = b.Id, 
                        text = b.Name 
                    }).ToList();
                    return Json(suggestions);
                }
                
                return Json(new List<object>());
            }
            catch (Exception)
            {
                return Json(new List<object>());
            }
        }

        // Search action for borrowers list view
        [HttpGet]
        public async Task<IActionResult> SearchBorrowersForList(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new { success = true, data = new List<object>() });

            var result = await _borrowerService.SearchBorrowers(term);
            if (result.Success && result.Data != null)
            {
                var borrowersData = result.Data.Select(b => new {
                    id = b.Id,
                    name = b.Name,
                    books = b.Books?.Select(book => new { title = book.Title }).ToList() ?? new List<object>().Select(_ => new { title = string.Empty }).ToList()
                });
                return Json(new { success = true, data = borrowersData });
            }

            return Json(new { success = false, data = new List<object>() });
        }
    }
}
