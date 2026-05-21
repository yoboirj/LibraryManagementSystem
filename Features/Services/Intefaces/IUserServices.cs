using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Services.Intefaces
{
    public interface IUserServices
    {
        Task<bool> RegisterAsync(User user, string? course = null);
        Task<User?> LoginAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);
        Task<List<User>> GetAllUsersAsync();
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User?> GetUserByIdAsync(int id);
    }
}
