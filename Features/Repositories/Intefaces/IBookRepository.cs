using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Repositories.Intefaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }
}
