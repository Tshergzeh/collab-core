using CollabCore.Core.Entities;

namespace CollabCore.Core.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task AddProjectMemberAsync(Guid projectId, Guid userId);
        Task RemoveProjectMemberAsync(Guid projectId, Guid userId);
        Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId);
        Task<IEnumerable<User>> GetProjectMembersAsync(Guid projectId);
    }
}