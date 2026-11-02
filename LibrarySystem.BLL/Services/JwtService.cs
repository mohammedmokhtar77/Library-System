using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace LibrarySystem.BLL.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtService(IConfiguration configuration , UserManager<ApplicationUser>userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }
    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var roles =await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name , user.UserName!)
        };
        
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));
        
        var encodedKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);
        var key = new SymmetricSecurityKey(encodedKey);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenString;
        
    }
}