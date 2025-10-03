using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;
using CollabCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabCore.Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetOwnedProjectsAsync(Guid id)
        {
            return await _context.Projects
                .Where(project => project.OwnerId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsAsync(Guid id)
        {
            return await _context.Assignments
                .Where(assignment => assignment.UserId == id)
                .Include(assignment => assignment.Task)
                .Select(assignment => new AssignmentResponse
                {
                    TaskId = assignment.TaskId,
                    TaskTitle = assignment.Task.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectMembershipResponse>> GetProjectMembershipsAsync(Guid id)
        {
            return await _context.ProjectMembers
                .Where(projectMember => projectMember.UserId == id)
                .Include(projectMember => projectMember.Project)
                .Select(projectMember => new ProjectMembershipResponse
                {
                    ProjectId = projectMember.ProjectId,
                    ProjectName = projectMember.Project.Name
                })
                .ToListAsync();
        }
    }
}