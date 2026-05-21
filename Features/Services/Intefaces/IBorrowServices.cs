using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Services.Intefaces
{
    public interface IBorrowServices
    {
        Task<List<Borrow>> GetBorrowsAsync();
        Task<Borrow?> GetBorrowByIdAsync(int id);
        Task<List<Borrow>> GetBorrowsByMemberNameAsync(string memberName);
        Task CreateBorrowAsync(Borrow borrow);
        Task ReturnBorrowAsync(int borrowId, int quantityToReturn);
        Task UpdateBorrowAsync(Borrow borrow);
        Task DeleteBorrowAsync(int id);
        Task<List<Borrow>> GetOverdueBorrowsAsync();
        Task<Dictionary<string, int>> GetMostBorrowedBooksAsync(int count);
    }
}
