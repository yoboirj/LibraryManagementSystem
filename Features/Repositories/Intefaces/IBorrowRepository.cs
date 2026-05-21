using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Repositories.Intefaces
{
    public interface IBorrowRepository
    {
        Task<List<Borrow>> GetAllAsync();
        Task<Borrow?> GetByIdAsync(int id);
        Task<List<Borrow>> GetByMemberNameAsync(string memberName);
        Task AddAsync(Borrow borrow);
        Task UpdateAsync(Borrow borrow);
        Task DeleteAsync(int id);
    }
}
