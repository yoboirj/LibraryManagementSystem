using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Features.Data.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Course { get; set; } = string.Empty;
    }
}
