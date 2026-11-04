using LibrarySystem.BLL.DTOs.Account;
using LibrarySystem.BLL.Interfaces.Services;
using LibrarySystem.BLL.Results;
using LibrarySystem.DAL.Constants;
using LibrarySystem.Entities.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController :  ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AccountController(UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager
        , IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        var user = new ApplicationUser()
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        
        if(!result.Succeeded)
            return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, RoleNames.Member);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return BadRequest(roleResult.Errors);
        }

        return CreatedAtAction(nameof(Login), new { email = user.Email }, new { user.Id, user.Email, user.FullName });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Unauthorized();

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized();

        var token =await _jwtService.GenerateTokenAsync(user);
        var response = GenericResult<string>.Success(token);
        return Ok(response);
    }
}
