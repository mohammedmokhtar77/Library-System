using LibrarySystem.Entities.Models;

namespace LibrarySystem.BLL.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}