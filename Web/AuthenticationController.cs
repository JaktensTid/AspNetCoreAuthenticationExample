using System.Security.Claims;
using AuthenticationExample.Infrastructure;
using AuthenticationExample.Infrastructure.Models;
using AuthenticationExample.Web.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationExample.Web;

[ApiController]
[Route("s/api/auth")]
public class AuthenticationController : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<MeDto> GetMe([FromServices] IApplicationContext ctx, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var user = await ctx.Users.FirstOrDefaultAsync(x => x.Id == userId, ct);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }
        
        return new MeDto(user.Id, user.Email!);
    }
    
    [HttpPost("login/cookies")]
    public async Task<IActionResult> LoginCookies(
        [FromServices] UserManager<User> userManager,
        [FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email!");
        }
        
        var success = await userManager.CheckPasswordAsync(user, model.Password);

        if (!success)
        {
            return Unauthorized("Invalid password!");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);
        
        return Ok();
    }

    
    [HttpPost("token")]
    public async Task<IActionResult> GenerateToken(
        [FromServices] UserManager<User> userManager,
        [FromServices] TokenService tokenService,
        LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email!");
        }
        
        var success = await userManager.CheckPasswordAsync(user, model.Password);

        if (!success)
        {
            return Unauthorized("Invalid password!");
        }

        return Ok(new LoginResponse
        {
            Email = user.Email!,
            Token = await tokenService.CreateToken(user)
        });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromServices] UserManager<User> userManager,
        RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (model.Password != model.PasswordRepeat)
        {
            return Unauthorized("Passwords do not match!");
        }

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user != null)
        {
            return Unauthorized("Email address already exists!");
        }

        user = new User(Guid.NewGuid(), model.Email);
        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return Unauthorized("Username not found and/or password incorrect");
        }

        return Ok();
    }
}

public record struct LoginModel(string Email, string Password);

public record struct RegisterModel(string Email, string Password, string PasswordRepeat);

public record struct LoginResponse(string Email, string Token);

public record struct MeDto(Guid Id, string Email);