using Kolokwium.DTOs;
using Kolokwium.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium.Controllers;
[ApiController]
[Route("/api/books")]

public class BookController: ControllerBase
{
    private readonly IBookRepository _bookRepository;
    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet("{id}/authors")]
    public async Task<IActionResult> getInformationAboutBook(int id)
    {
        BooksAuthorsDTO book = await _bookRepository.GetInformationAboutAuthors(id);
        if (book != null) return Ok(book);
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> addBook(AddBookDTO request)
    {
        try
        {
            var bookId = _bookRepository.AddBook(request);

            return Ok(new { Message = "Book added successfully", BookId = bookId });
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}