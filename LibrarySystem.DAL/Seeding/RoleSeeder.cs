using LibrarySystem.DAL.Constants;
using Microsoft.AspNetCore.Identity;
namespace LibrarySystem.DAL.Seeding;

public static class RoleSeeder
{
   public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
   {
      foreach (var roleName in RoleNames.All)
      {
         var roleExists = await roleManager.RoleExistsAsync(roleName);
         if (roleExists)
            continue;
         
         var  result = await roleManager.CreateAsync(new IdentityRole(roleName));
         
         if (!result.Succeeded)
         {
            var errors = string.Join(", ", result.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Failed to create role '{roleName}': {errors}");
         }
      }
   }
}