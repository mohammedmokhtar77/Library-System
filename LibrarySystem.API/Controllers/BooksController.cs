
using LibrarySystem.BLL.DTOs.Books;
using LibrarySystem.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService services) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var result = await services.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await services.GetByIdAsync(
            id,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error?.Contains(
                    "not found",
                    StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound(new { error = result.Error });
            }

            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookDto dto,
        CancellationToken cancellationToken)
    {
        var result = await services.CreateAsync(
            dto,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data!.Id },
            result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateBookDto dto,
        CancellationToken cancellationToken)
    {
        var result = await services.UpdateAsync(
            id,
            dto,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error?.Contains(
                    "not found",
                    StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound(new { error = result.Error });
            }

            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await services.DeleteAsync(
            id,
            cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.Error?.Contains(
                    "not found",
                    StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound(new { error = result.Error });
            }

            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }
}
