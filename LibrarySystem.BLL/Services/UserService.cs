using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Results;
using LibrarySystem.BLL.DTOs.Account;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IReadOnlyList<UserRoleDto>> GetUsersAsync()
    {
        var users = _userManager.Users.OrderBy(user => user.Email).ToList();
        var results = new List<UserRoleDto>(users.Count);
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            results.Add(new UserRoleDto(user.Id, user.Email, user.FullName, roles.ToArray()));
        }

        return results;
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync() =>
        _roleManager.Roles.OrderBy(role => role.Name)
            .Select(role => role.Name!)
            .ToList();

    public async Task<Result> AssignRoleAsync(string userId, string role)
    {
        role = role?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(role))
            return Result.Failure("Role is required.");

        var roleEntity = await _roleManager.FindByNameAsync(role);
        if (roleEntity is null)
            return Result.Failure("Role not found.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure("User not found.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count == 1 && string.Equals(currentRoles[0], roleEntity.Name, StringComparison.OrdinalIgnoreCase))
            return Result.Success();

        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
        {
            var errors = string.Join(", ", removeResult.Errors.Select(x => x.Description));
            return Result.Failure(errors);
        }

        var addResult = await _userManager.AddToRoleAsync(user, roleEntity.Name!);

        if (!addResult.Succeeded)
        {
            var errors = string.Join(", ", addResult.Errors.Select(x => x.Description));

            return Result.Failure(errors);
        }

        return Result.Success();
    }
}
