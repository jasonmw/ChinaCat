using ChinaCatSunflower.Models.Book;
using ChinaCatSunflower.Repositories;
using ChinaCatSunflower.Services;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Npgsql;

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
    public async Task<IActionResult> Index() {
        var model = new BookIndexModel();
        return View(model);
    }

    [HttpGet]
    [Route("/books/load")]
    public async Task<IActionResult> LoadBooks() {
        var model = await _book_repository.GetBooksWithoutTheJson();
        return PartialView("_BookRows", model);
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

        try {
            await _book_repository.AddBook(book);
        }
        catch (NpgsqlException sex) {
            if (sex.Message.Contains("duplicate key value")) {
                ModelState.Clear();
                ModelState.AddModelError("ISBN", $"A book with the ISBN number {addBook.ISBN} already exists in the database");
                
                return PartialView("_BookForm", new AddBook());
            }
        }

        ModelState.Clear();
        Response.Htmx(h => h.WithTrigger("bookadd"));
        return PartialView("_BookAdded", book);
    }
}