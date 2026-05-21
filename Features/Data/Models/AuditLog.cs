using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Features.Data.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty; // Borrow, Return
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Details { get; set; } = string.Empty;
    }
}
