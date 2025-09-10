namespace BooksLendingService.Services;

public sealed class OperationResult<T>
{
    public T? Value { get; }
    public int? StatusCode { get; }
    public string? Message { get; }
    public bool IsSuccess => StatusCode is null;

    private OperationResult(T? value, int? statusCode, string? message)
    {
        Value = value;
        StatusCode = statusCode;
        Message = message;
    }

    public static OperationResult<T> Ok(T value) =>
        new(value, null, null);

    public static OperationResult<T> NotFound(string message = "Not found") =>
        new(default, StatusCodes.Status404NotFound, message);

    public static OperationResult<T> Conflict(string message = "Conflict") =>
        new(default, StatusCodes.Status409Conflict, message);
}
