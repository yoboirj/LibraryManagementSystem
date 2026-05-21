using LibraryManagementSystem.Features.Repositories.Intefaces;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Data;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Features.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public BookRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Books.AsNoTracking().ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            using var context = _contextFactory.CreateDbContext();
            var trackedEntity = context.Books.Local.FirstOrDefault(b => b.Id == book.Id);
            if (trackedEntity != null)
            {
                context.Entry(trackedEntity).State = EntityState.Detached;
            }

            context.Entry(book).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var book = await context.Books.FindAsync(id);
            if (book != null)
            {
                context.Books.Remove(book);
                await context.SaveChangesAsync();
            }
        }
    }
}
