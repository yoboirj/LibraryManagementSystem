using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Features.Data.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int BookId { get; set; }
        public DateTimeOffset BorrowedAt { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;  
        public int Quantity { get; set; }
        public int ReturnedQuantity { get; set; }
        public bool IsReturned { get; set; }
    }
}
