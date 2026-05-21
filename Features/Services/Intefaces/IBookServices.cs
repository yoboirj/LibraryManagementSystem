using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Services.Intefaces
{
    public interface IBookServices
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<List<Book>> GetBooksWithAuthorsAsync();
        Task<Book?> GetBookWithAuthorAsync(int id);
    }
}
