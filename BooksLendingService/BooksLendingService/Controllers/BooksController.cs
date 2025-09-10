using BooksLendingService.Models.Dtos.Requests;
using BooksLendingService.Models.Dtos.Responses;
using BooksLendingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksLendingService.Controllers;

[ApiController]
[Route("books")]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddBookRequest request, CancellationToken cancellationToken)
    {
        var response = await bookService.AddAsync(request, cancellationToken);
        return Created($"/books/{response.Id}", response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] bool showAll = false, CancellationToken cancellationToken = default)
    {
        return Ok(await bookService.ListAsync(showAll, cancellationToken));
    }

    [HttpPost("{id:guid}/checkout")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Checkout(Guid id, CancellationToken cancellationToken)
    {
        var response = await bookService.CheckoutAsync(id, cancellationToken);
        if (!response.IsSuccess) return Problem(statusCode: response.StatusCode, title: response.Message);
        return Ok(response.Value);
    }

    [HttpPost("{id:guid}/return")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Return(Guid id, CancellationToken cancellationToken)
    {
        var result = await bookService.ReturnAsync(id, cancellationToken);
        if (!result.IsSuccess) return Problem(statusCode: result.StatusCode, title: result.Message);
        return Ok(result.Value);
    }
}
