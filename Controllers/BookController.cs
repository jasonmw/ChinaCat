using ChinaCatSunflower.Models.Book;
using ChinaCatSunflower.Repositories;
using ChinaCatSunflower.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChinaCatSunflower.Controllers;

public class BookController : Controller
{
    private readonly BookRetrievalService _book_retrieval_service;
    private readonly BookRepository _book_repository;

    public BookController(BookRetrievalService book_retrieval_service, BookRepository book_repository) {
        _book_retrieval_service = book_retrieval_service;
        _book_repository = book_repository;
    }
    // GET
    public IActionResult Index() {
        var model = new BookIndexModel();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromForm]AddBook addBook) {
        if (!ModelState.IsValid) {
            return PartialView("_BookForm", addBook);
        }
        
        var book = await _book_retrieval_service.GetBookByIsbn(addBook.ISBN);
        if (book is null) {
            ModelState.AddModelError("ISBN", $"A book with the ISBN number {addBook.ISBN} was not found");
            return PartialView("_BookForm", addBook);
        }

        await _book_repository.AddBook(book);

        ModelState.Clear();
        return PartialView("_BookAdded", book);
    }
}