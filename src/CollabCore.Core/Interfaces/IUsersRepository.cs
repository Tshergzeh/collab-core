using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;

namespace CollabCore.Core.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Project>> GetOwnedProjectsAsync(Guid id);
        Task<IEnumerable<AssignmentResponse>> GetAssignmentsAsync(Guid id);
        Task<IEnumerable<ProjectMembershipResponse>> GetProjectMembershipsAsync(Guid id);
    }
}