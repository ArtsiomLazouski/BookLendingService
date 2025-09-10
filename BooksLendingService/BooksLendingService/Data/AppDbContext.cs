using BooksLendingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksLendingService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
}
