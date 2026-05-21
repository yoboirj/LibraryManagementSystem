using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Features.Data.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Biography { get; set; }

        public string? Nationality { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
    
