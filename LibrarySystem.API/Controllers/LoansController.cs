using LibrarySystem.DAL.Constants;
using LibrarySystem.BLL.DTOs.Loans;
using LibrarySystem.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController(ILoanService services) : ControllerBase
{
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian}")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await services.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Data);
    }
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian},{RoleNames.Member}")]
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
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian}")]
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
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian}")]
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
    [Authorize(Roles = $"{RoleNames.Admin}")]
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
