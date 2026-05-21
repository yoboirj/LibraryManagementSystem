using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Services.Intefaces
{
    public interface IAuthorServices
    {
        Task<List<Author>> GetAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<Author?> GetAuthorByNameAsync(string name);
        Task CreateAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
        Task<List<Book>> GetBooksByAuthorAsync(int authorId);
        Task<Dictionary<string, int>> GetAuthorStatisticsAsync();
    }
}
