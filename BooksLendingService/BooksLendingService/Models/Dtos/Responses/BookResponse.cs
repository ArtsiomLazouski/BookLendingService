namespace BooksLendingService.Models.Dtos.Responses;

public class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public bool IsAvailable { get; set; }

    public BookResponse() { }

    public BookResponse(Guid id, string title, string author, string? isbn, bool isAvailable)
    {
        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
        IsAvailable = isAvailable;
    }
}