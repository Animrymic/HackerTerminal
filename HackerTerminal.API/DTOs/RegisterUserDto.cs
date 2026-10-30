using System.ComponentModel.DataAnnotations;

namespace HackerTerminal.API.DTOs;

public class RegisterUserDto
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    [RegularExpression(
        @"^[a-zA-Z0-9_-]+$",
        ErrorMessage = "Username may contain only letters, numbers, underscores, and hyphens.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}