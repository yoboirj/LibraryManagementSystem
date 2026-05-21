using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Data.Models;
using LibraryManagementSystem.Features.Repositories.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Features.Repositories.Implementations
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public AuditLogRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(AuditLog log)
        {
            using var context = _contextFactory.CreateDbContext();
            context.AuditLogs.Add(log);
            await context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.AuditLogs.AsNoTracking().OrderByDescending(l => l.TransactionDate).ToListAsync();
        }
    }
}
