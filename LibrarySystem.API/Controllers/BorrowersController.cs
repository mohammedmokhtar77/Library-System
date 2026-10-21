using LibrarySystem.BLL.DTOs.Borrowers;
using LibrarySystem.BLL.Interfaces.Repositories;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowersController(IBorrowerRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BorrowerDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var borrowers = await repository.GetAllAsync(cancellationToken);

        var borrowerDtos = borrowers.Select(b => new BorrowerDto
        {
            Id = b.Id,
            Name = b.Name,
            MembershipDate = b.MembershipDate
        }).ToList();

        return Ok(borrowerDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BorrowerDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var borrower = await repository.GetByIdAsync(id, cancellationToken);

        if (borrower is null)
            return NotFound();

        var borrowerDto = new BorrowerDto
        {
            Id = borrower.Id,
            Name = borrower.Name,
            MembershipDate = borrower.MembershipDate
        };

        return Ok(borrowerDto);
    }

    [HttpPost]
    public async Task<ActionResult<BorrowerDto>> CreateAsync(
        CreateBorrowerDto borrowerDto,
        CancellationToken cancellationToken = default)
    {
        var borrower = new Borrower
        {
            Name = borrowerDto.Name,
            MembershipDate = borrowerDto.MembershipDate
        };

        var createdBorrower = await repository.AddAsync(
            borrower, cancellationToken);

        var result = new BorrowerDto
        {
            Id = createdBorrower.Id,
            Name = createdBorrower.Name,
            MembershipDate = createdBorrower.MembershipDate
        };

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = createdBorrower.Id },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        UpdateBorrowerDto borrowerDto,
        CancellationToken cancellationToken = default)
    {
        var updated = await repository.UpdateAsync(new Borrower
        {
            Id = id, // نحدد المستعير الموجود بالـ ID
            Name = borrowerDto.Name,
            MembershipDate = borrowerDto.MembershipDate
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