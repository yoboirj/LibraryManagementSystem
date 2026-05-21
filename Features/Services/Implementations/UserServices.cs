using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Services.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Services.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public UserServices(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> RegisterAsync(User user, string? course = null)
        {
            using var _context = _contextFactory.CreateDbContext();
            if (await UserExistsAsync(user.Username))
                return false;

            // In a real application, you should hash the password here!
            _context.Users.Add(user);

            // Automatically add to Members table if role is Member
            if (user.Role == "Member")
            {
                var member = new Member
                {
                    Name = user.Username,
                    Course = course ?? "Not Specified"
                };
                _context.Members.Add(member);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            using var _context = _contextFactory.CreateDbContext();
            // In a real application, you would compare the hashed password
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            using var _context = _contextFactory.CreateDbContext();
            var trackedEntity = _context.Users.Local.FirstOrDefault(u => u.Id == user.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            using var _context = _contextFactory.CreateDbContext();
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
