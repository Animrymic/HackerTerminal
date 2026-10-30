using HackerTerminal.API.DTOs;
using HackerTerminal.Core.Entities;
using HackerTerminal.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace HackerTerminal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly HackerTerminalDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public UsersController(
        HackerTerminalDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
        _configuration = configuration;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMyProfile()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized();
        }

        User? user = await _context.Users
            .SingleOrDefaultAsync(item => item.Id == userId);

        if (user is null)
        {
            return NotFound();
        }

        UserDto userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };

        return Ok(userDto);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
        RegisterUserDto registerUserDto)
    {
        string username = registerUserDto.Username.Trim();
        string email = registerUserDto.Email.Trim();

        string normalizedUsername = username.ToUpperInvariant();
        string normalizedEmail = email.ToUpperInvariant();

        User? existingUser = await _context.Users
            .FirstOrDefaultAsync(user =>
                user.Username.ToUpper() == normalizedUsername ||
                user.Email.ToUpper() == normalizedEmail);

        if (existingUser is not null)
        {
            return Conflict("Username or email is already in use.");
        }

        User user = new User
        {
            Username = username.ToLowerInvariant(),
            Email = email.ToLowerInvariant(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            registerUserDto.Password);

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            bool duplicateExists = await _context.Users
                .AsNoTracking()
                .AnyAsync(existingUser =>
                    existingUser.Username.ToUpper() == normalizedUsername ||
                    existingUser.Email.ToUpper() == normalizedEmail);

            if (duplicateExists)
            {
                return Conflict("Username or email is already in use.");
            }

            throw;
        }

        UserDto userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };

        return CreatedAtAction(
            nameof(GetMyProfile),
            new { id = user.Id },
            userDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(
        LoginUserDto loginUserDto)
    {
        string normalizedUsername = loginUserDto.Username
            .Trim()
            .ToUpperInvariant();

        User? user = await _context.Users
            .FirstOrDefaultAsync(existingUser =>
                existingUser.Username.ToUpper() == normalizedUsername);

        if (user is null || !user.IsActive)
        {
            return Unauthorized("Invalid username or password.");
        }

        PasswordVerificationResult passwordResult =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginUserDto.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid username or password.");
        }

        UserDto userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        string? jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            return StatusCode(500, "JWT key is not configured.");
        }

        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        SigningCredentials credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token =
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        _configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials);

        string tokenString =
            new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            user = userDto
        });
    }
}