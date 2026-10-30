using System.ComponentModel.DataAnnotations;

namespace HackerTerminal.API.DTOs;

public class SubmitAnswerDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Answer { get; set; } = string.Empty;
}