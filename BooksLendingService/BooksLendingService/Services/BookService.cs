using BooksLendingService.Data;
using BooksLendingService.Models;
using BooksLendingService.Models.Dtos.Requests;
using BooksLendingService.Models.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace BooksLendingService.Services;

public class BookService(AppDbContext db) : IBookService
{
    public async Task<BookResponse> AddAsync(AddBookRequest request, CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            Isbn = string.IsNullOrWhiteSpace(request.Isbn) ? null : request.Isbn.Trim(),
            IsAvailable = true
        };

        db.Books.Add(book);
        await db.SaveChangesAsync(cancellationToken);

        return new BookResponse(book.Id, book.Title, book.Author, book.Isbn, book.IsAvailable);
    }

    public async Task<IReadOnlyList<BookResponse>> ListAsync(bool showAll, CancellationToken cancellationToken = default)
    {
        var query = db.Books.AsNoTracking();
        if (!showAll) query = query.Where(b => b.IsAvailable);

        return await query
            .OrderBy(b => b.Title)
            .Select(b => new BookResponse(b.Id, b.Title, b.Author, b.Isbn, b.IsAvailable))
            .ToListAsync(cancellationToken);
    }

    public async Task<OperationResult<BookResponse>> CheckoutAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (book is null) return OperationResult<BookResponse>.NotFound("Book not found");

        if (!book.IsAvailable)
            return OperationResult<BookResponse>.Conflict("Book is already checked out");

        book.IsAvailable = false;
        await db.SaveChangesAsync(cancellationToken);

        return OperationResult<BookResponse>.Ok(new BookResponse(book.Id, book.Title, book.Author, book.Isbn, book.IsAvailable));
    }

    public async Task<OperationResult<BookResponse>> ReturnAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (book is null) return OperationResult<BookResponse>.NotFound("Book not found");

        if (book.IsAvailable)
            return OperationResult<BookResponse>.Conflict("Book is already available");

        book.IsAvailable = true;
        await db.SaveChangesAsync(cancellationToken);

        return OperationResult<BookResponse>.Ok(new BookResponse(book.Id, book.Title, book.Author, book.Isbn, book.IsAvailable));
    }
}
