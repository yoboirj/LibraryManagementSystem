using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Features.Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        public int? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }

        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000")]
        public int Quantity { get; set; }

        [StringLength(20)]
        public string? ISBN { get; set; }

        [StringLength(50)]
        public string? Genre { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int? PublicationYear { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
