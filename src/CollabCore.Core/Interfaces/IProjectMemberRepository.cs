using CollabCore.Contracts.Responses;

namespace CollabCore.Core.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task AddProjectMemberAsync(Guid projectId, Guid userId, string role);
        Task RemoveProjectMemberAsync(Guid projectId, Guid userId);
        Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId);
        Task<bool> IsProjectManagerAsync(Guid projectId, Guid userId);
        Task<IEnumerable<ProjectMemberResponse>> GetProjectMembersAsync(Guid projectId);
        Task UpdateMemberRoleAsync(
            Guid projectId,
            Guid userId,
            string role);
    }
}