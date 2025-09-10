using System.ComponentModel.DataAnnotations;

namespace BooksLendingService.Models.Dtos.Requests;

public class AddBookRequest
{
    [Required, MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required, MaxLength(200)]
    public string Author { get; init; } = string.Empty;

    [MaxLength(32)]
    public string? Isbn { get; init; }
}