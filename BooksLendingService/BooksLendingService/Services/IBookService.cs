using BooksLendingService.Models.Dtos.Requests;
using BooksLendingService.Models.Dtos.Responses;

namespace BooksLendingService.Services;

public interface IBookService
{
    Task<BookResponse> AddAsync(AddBookRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<BookResponse>> ListAsync(bool showAll, CancellationToken ct = default);
    Task<OperationResult<BookResponse>> CheckoutAsync(Guid id, CancellationToken ct = default);
    Task<OperationResult<BookResponse>> ReturnAsync(Guid id, CancellationToken ct = default);
}
