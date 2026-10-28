using LibrarySystem.BLL.DTOs.Loans;
using LibrarySystem.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController(ILoanService services) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await services.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpGet("{bookId:int}/{borrowerId:int}")]
    public async Task<IActionResult> GetById(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken)
    {
        var result = await services.GetByIdAsync(
            bookId,
            borrowerId,
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
        [FromBody] CreateLoanDto dto,
        CancellationToken cancellationToken)
    {
        var result = await services.CreateAsync(
            dto,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                bookId = result.Data!.BookId,
                borrowerId = result.Data.BorrowerId
            },
            result.Data);
    }

    [HttpPut("{bookId:int}/{borrowerId:int}")]
    public async Task<IActionResult> Update(
        int bookId,
        int borrowerId,
        [FromBody] UpdateLoanDto dto,
        CancellationToken cancellationToken)
    {
        var result = await services.UpdateAsync(
            bookId,
            borrowerId,
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

    [HttpDelete("{bookId:int}/{borrowerId:int}")]
    public async Task<IActionResult> Delete(
        int bookId,
        int borrowerId,
        CancellationToken cancellationToken)
    {
        var result = await services.DeleteAsync(
            bookId,
            borrowerId,
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
