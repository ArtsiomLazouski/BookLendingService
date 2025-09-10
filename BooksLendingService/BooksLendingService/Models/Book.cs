using System.ComponentModel.DataAnnotations;

namespace BooksLendingService.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(32)]
        public string? Isbn { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
