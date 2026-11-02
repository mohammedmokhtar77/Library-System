using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.DAL.Constants;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = RoleNames.Admin)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers() => Ok(await _userService.GetUsersAsync());

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles() => Ok(await _userService.GetRolesAsync());

    [HttpPut("{userId}/role")]
    public async Task<IActionResult> AssignRole(
        string userId,
        [FromBody] AssignRoleRequest request)
    {
        var result = await _userService.AssignRoleAsync(
            userId,
            request.Role);

        if (!result.IsSuccess)
            return result.Error == "User not found." || result.Error == "Role not found."
                ? NotFound(new { error = result.Error })
                : BadRequest(new { error = result.Error });

        return NoContent();
    }
}
