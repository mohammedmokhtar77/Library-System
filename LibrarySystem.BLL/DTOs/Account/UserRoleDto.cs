namespace LibrarySystem.BLL.DTOs.Account;

public sealed record UserRoleDto(string Id, string? Email, string FullName, IReadOnlyList<string> Roles);
