using bookstore.Services.BookService;
using bookstore.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;

namespace bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> All(int pageNumber = 1)
        {
            var result = await _bookService.GetBookList(pageNumber);
            if (result.Success)
                return View("All", result.Data);

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("Error");
        }
        public IActionResult NewBook()
        {
            return View("Add");
        }

        public async Task<IActionResult> AddNewBook(AddBookVM addBookVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.AddNewBook(addBookVM);

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

            return View("Add", addBookVM);
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            var result = await _bookService.GetBookDetails(id);
            if (result.Success)
                return View("Details", result.Data);

            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("Error");
        }

        // Search action for books list view
        [HttpGet]
        public async Task<IActionResult> SearchBooksForList(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new { success = true, data = new List<object>() });

            var result = await _bookService.SearchBooks(term);
            if (result.Success && result.Data != null)
            {
                var booksData = result.Data.Select(b => new {
                    id = b.Id,
                    title = b.Title,
                    author = b.Author,
                    coverUrl = b.CoverUrl
                });
                return Json(new { success = true, data = booksData });
            }

            return Json(new { success = false, data = new List<object>() });
        }
    }
}
