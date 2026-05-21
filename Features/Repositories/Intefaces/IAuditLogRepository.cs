using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Repositories.Intefaces
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditLog log);
        Task<List<AuditLog>> GetAllAsync();
    }
}
