using LibrarySystem.BLL.Results;
using LibrarySystem.BLL.DTOs.Account;

namespace LibrarySystem.BLL.Interfaces.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserRoleDto>> GetUsersAsync();
    Task<IReadOnlyList<string>> GetRolesAsync();
    Task<Result> AssignRoleAsync(string userId, string role);
}
