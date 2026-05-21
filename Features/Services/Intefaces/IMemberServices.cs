using LibraryManagementSystem.Features.Data.Models;

namespace LibraryManagementSystem.Features.Services.Intefaces
{
    public interface IMemberServices
    {
        Task<List<Member>> GetAllMembersAsync();
        Task<Member?> GetMemberByIdAsync(int id);
        Task UpdateMemberAsync(Member member);
        Task DeleteMemberAsync(int id);
    }
}
