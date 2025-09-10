using BooksLendingService.Data;
using BooksLendingService.Models;
using BooksLendingService.Models.Dtos.Requests;
using BooksLendingService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BooksLendingService.UnitTests;

[TestClass]
public class BookServiceTests
{
    private SqliteConnection _conn = null!;
    private AppDbContext _db = null!;
    private IBookService _svc = null!;

    [TestInitialize]
    public void Setup()
    {
        _conn = new SqliteConnection("DataSource=:memory:");
        _conn.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_conn)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        _svc = new BookService(_db);
    }

    [TestCleanup]
    public void Teardown()
    {
        _db.Dispose();
        _conn.Dispose();
    }

    [TestMethod]
    public async Task AddAsync_CreatesBook_AsAvailableTrue()
    {
        var req = new AddBookRequest { Title = "The Hobbit", Author = "Tolkien", Isbn = "9780547928227" };

        var created = await _svc.AddAsync(req);

        created.Should().NotBeNull();
        created.Title.Should().Be("The Hobbit");
        created.IsAvailable.Should().BeTrue();

        var inDb = await _db.Books.FindAsync(created.Id);
        inDb.Should().NotBeNull();
        inDb!.IsAvailable.Should().BeTrue();
    }

    [TestMethod]
    public async Task ListAsync_ShowAllFalse_ReturnsOnlyAvailable()
    {
        _db.Books.AddRange(
            new Book { Title = "A", Author = "X", IsAvailable = true },
            new Book { Title = "B", Author = "Y", IsAvailable = false },
            new Book { Title = "C", Author = "Z", IsAvailable = true }
        );
        await _db.SaveChangesAsync();

        var onlyAvailable = await _svc.ListAsync(showAll: false);
        onlyAvailable.Should().OnlyContain(b => b.IsAvailable).And.HaveCount(2);

        var all = await _svc.ListAsync(showAll: true);
        all.Should().HaveCount(3);
    }

    [TestMethod]
    public async Task CheckoutAsync_NotFound_Returns404()
    {
        var result = await _svc.CheckoutAsync(Guid.NewGuid());
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [TestMethod]
    public async Task CheckoutAsync_WhenAvailable_SetsIsAvailableFalse()
    {
        var book = new Book { Title = "D", Author = "X", IsAvailable = true };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        var result = await _svc.CheckoutAsync(book.Id);
        result.IsSuccess.Should().BeTrue();
        result.Value!.IsAvailable.Should().BeFalse();

        var inDb = await _db.Books.FindAsync(book.Id);
        inDb!.IsAvailable.Should().BeFalse();
    }

    [TestMethod]
    public async Task CheckoutAsync_AlreadyCheckedOut_ReturnsConflict()
    {
        var book = new Book { Title = "E", Author = "X", IsAvailable = false };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        var result = await _svc.CheckoutAsync(book.Id);
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }

    [TestMethod]
    public async Task ReturnAsync_NotFound_Returns404()
    {
        var result = await _svc.ReturnAsync(Guid.NewGuid());
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [TestMethod]
    public async Task ReturnAsync_WhenCheckedOut_SetsIsAvailableTrue()
    {
        var book = new Book { Title = "F", Author = "X", IsAvailable = false };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        var result = await _svc.ReturnAsync(book.Id);
        result.IsSuccess.Should().BeTrue();
        result.Value!.IsAvailable.Should().BeTrue();

        var inDb = await _db.Books.FindAsync(book.Id);
        inDb!.IsAvailable.Should().BeTrue();
    }

    [TestMethod]
    public async Task ReturnAsync_AlreadyAvailable_ReturnsConflict()
    {
        var book = new Book { Title = "G", Author = "X", IsAvailable = true };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        var result = await _svc.ReturnAsync(book.Id);
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }
}
