using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Repositories.Intefaces;
using LibraryManagementSystem.Features.Services.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Services.Implementations
{
    public class BookServices : IBookServices
    {
        private readonly IBookRepository _repo;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public BookServices(IBookRepository repo, IDbContextFactory<AppDbContext> contextFactory)
        {
            _repo = repo;
            _contextFactory = contextFactory;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task CreateBookAsync(Book book)
        {
            await _repo.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _repo.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
        public async Task<List<Book>> GetBooksWithAuthorsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Books
                .Include(b => b.Author)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Book?> GetBookWithAuthorAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
