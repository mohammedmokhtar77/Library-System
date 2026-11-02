using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Entities.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}