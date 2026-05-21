using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Repositories.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Repositories.Implementations
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public BorrowRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Borrow>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Borrows.AsNoTracking().ToListAsync();
        }

        public async Task<Borrow?> GetByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Borrows.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Borrow>> GetByMemberNameAsync(string memberName)
        {
            using var context = _contextFactory.CreateDbContext();
            var member = await context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Name == memberName);
            if (member == null) return new List<Borrow>();

            return await context.Borrows.AsNoTracking()
                .Where(b => b.MemberId == member.Id)
                .ToListAsync();
        }

        public async Task AddAsync(Borrow borrow)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Borrows.Add(borrow);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Borrow borrow)
        {
            using var context = _contextFactory.CreateDbContext();
            var trackedEntity = context.Borrows.Local.FirstOrDefault(b => b.Id == borrow.Id);
            if (trackedEntity != null)
            {
                context.Entry(trackedEntity).State = EntityState.Detached;
            }

            context.Entry(borrow).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var borrow = await context.Borrows.FindAsync(id);
            if (borrow != null)
            {
                context.Borrows.Remove(borrow);
                await context.SaveChangesAsync();
            }
        }
    }
}
