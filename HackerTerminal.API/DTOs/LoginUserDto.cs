using System.ComponentModel.DataAnnotations;

namespace HackerTerminal.API.DTOs;

public class LoginUserDto
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;
}