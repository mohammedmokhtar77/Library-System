using LibrarySystem.DAL.Constants;
using LibrarySystem.BLL.DTOs.Authors;
using LibrarySystem.BLL.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController(IAuthorService service) : ControllerBase
{
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian},{RoleNames.Member},{RoleNames.Viewer}")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
         int pageNumber ,
          int pageSize  ,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetAllAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Data);
    }
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian},{RoleNames.Member},{RoleNames.Viewer}")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(
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
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian}")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAuthorDto dto,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(
            dto,
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data!.Id },
            result.Data);
    }
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Librarian}")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateAuthorDto dto,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(
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
    [Authorize(Roles = $"{RoleNames.Admin}")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(
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
