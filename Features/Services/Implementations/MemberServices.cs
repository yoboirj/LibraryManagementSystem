using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Services.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Services.Implementations
{
    public class MemberServices : IMemberServices
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public MemberServices(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Members.AsNoTracking().ToListAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateMemberAsync(Member member)
        {
            using var context = _contextFactory.CreateDbContext();
            var trackedEntity = context.Members.Local.FirstOrDefault(m => m.Id == member.Id);
            if (trackedEntity != null)
            {
                context.Entry(trackedEntity).State = EntityState.Detached;
            }
            context.Entry(member).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteMemberAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var member = await context.Members.FindAsync(id);
            if (member != null)
            {
                context.Members.Remove(member);
                await context.SaveChangesAsync();
            }
        }
    }
}
