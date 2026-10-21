using LibrarySystem.BLL.DTOs.Books;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookDto>>>
        GetAllAsync(CancellationToken cancellationToken = default)
    {
        var books = await repository.GetAllAsync(cancellationToken);

        var bookDto = books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            ISBN = b.ISBN,
            AuthorId = b.AuthorId
        }).ToList();

        return Ok(bookDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> 
        GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await repository.GetByIdAsync(id, cancellationToken);

        if (book is null)
            return NotFound();

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            AuthorId = book.AuthorId
        };

        return Ok(bookDto);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>>
        CreateAsync(CreateBookDto bookDto, CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Title = bookDto.Title,
            ISBN = bookDto.ISBN,
            AuthorId = bookDto.AuthorId
        };

        var createdBook = await repository.AddAsync(book, cancellationToken);

        var result = new BookDto
        {
            Id = createdBook.Id,
            Title = createdBook.Title,
            ISBN = createdBook.ISBN,
            AuthorId = createdBook.AuthorId
        };

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = createdBook.Id },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        UpdateBookDto bookDto,
        CancellationToken cancellationToken = default)
    {
        var updated = await repository.UpdateAsync(new Book
        {
            Id = id,
            Title = bookDto.Title,
            ISBN = bookDto.ISBN,
            AuthorId = bookDto.AuthorId
        }, cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}