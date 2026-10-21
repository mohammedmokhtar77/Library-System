using LibrarySystem.BLL.DTOs.Authors;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IAuthorRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuthorDto>>>
        GetAllAsync(CancellationToken cancellationToken = default)
    {
        var authors = await repository.GetAllAsync(cancellationToken);
        var authorDto = authors.Select(a => new AuthorDto
        {
            Id = a.Id ,Name = a.Name ,BirthDate = a.BirthDate  
        }).ToList();
        
        return Ok(authorDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>>
        GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await repository.GetByIdAsync(id, cancellationToken);
        if (author is null)
            return NotFound();
        var authorDto = new AuthorDto()
        {
            Id = author.Id,
            Name = author.Name,
            BirthDate = author.BirthDate
        };
        
        return Ok(authorDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create(
        CreateAuthorDto authorDto,
        CancellationToken cancellationToken = default
    )
    {
        var author = new Author()
        {
            Name = authorDto.Name,
            BirthDate = authorDto.BirthDate
        };
        var createdAuthor  = await repository.AddAsync(author, cancellationToken);

        var result = new AuthorDto()
        {
            Id = author.Id,
            Name = author.Name,
            BirthDate = author.BirthDate
        };
        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdAuthor.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateAuthorDto authorDto,
        CancellationToken cancellationToken = default)
    {
        var updated = await repository.UpdateAsync(new Author()
        {
            Id = id,
            Name = authorDto.Name,
            BirthDate = authorDto.BirthDate
        }, cancellationToken);
        
        return updated ? NoContent() : NotFound();
    }
    
    [HttpDelete("{id:int}")]  
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
        
    }

    
}