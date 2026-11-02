using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DAL.Data;

public class AuthenticationContext : IdentityDbContext<ApplicationUser>
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options) 
        : base(options)
    {
        
    }
}