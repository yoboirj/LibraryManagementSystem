using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Repositories.Intefaces;
using LibraryManagementSystem.Features.Services.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Services.Implementations
{
    public class AuthorServices : IAuthorServices
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public AuthorServices(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author?> GetAuthorByNameAsync(string name)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task CreateAuthorAsync(Author author)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            // Check if author already exists
            var existingAuthor = await GetAuthorByNameAsync(author.Name);
            if (existingAuthor != null)
                throw new Exception($"Author '{author.Name}' already exists.");

            author.CreatedAt = DateTime.Now;
            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var existingAuthor = await context.Authors.FindAsync(author.Id);

            if (existingAuthor == null)
                throw new Exception("Author not found.");

            existingAuthor.Name = author.Name;
            existingAuthor.Biography = author.Biography;
            existingAuthor.Nationality = author.Nationality;
            existingAuthor.BirthDate = author.BirthDate;

            context.Authors.Update(existingAuthor);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var author = await context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                throw new Exception("Author not found.");

            if (author.Books.Any())
                throw new Exception($"Cannot delete author '{author.Name}' because they have {author.Books.Count} book(s) associated.");

            context.Authors.Remove(author);
            await context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(int authorId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Books
                .Where(b => b.AuthorId == authorId)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetAuthorStatisticsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Authors
                .Include(a => a.Books)
                .Select(a => new { a.Name, BookCount = a.Books.Count })
                .ToDictionaryAsync(k => k.Name, v => v.BookCount);
        }
    }
}
