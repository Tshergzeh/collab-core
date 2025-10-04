using CollabCore.Core.Interfaces;
using CollabCore.Infrastructure.Data;
using CollabCore.Core.Entities;
using CollabCore.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace CollabCore.Infrastructure.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly AppDbContext _context;

        public ProjectMemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProjectMemberAsync(
            Guid projectId,
            Guid userId,
            string role = "Contributor")
        {
            if (!ProjectRoles.IsValid(role)) throw new ArgumentException(
                $"Invalid role '{role}'. Valid roles are: {string.Join(", ", ProjectRoles.All)}");
            if (!await _context.ProjectMembers
                .AnyAsync(projectMember => projectMember.ProjectId == projectId &&
                    projectMember.UserId == userId)
            )
            {
                _context.ProjectMembers.Add(new ProjectMember
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Role = role
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveProjectMemberAsync(Guid projectId, Guid userId)
        {
            var membership = await _context.ProjectMembers.FirstOrDefaultAsync(projectMember =>
                projectMember.ProjectId == projectId && projectMember.UserId == userId);

            if (membership != null)
            {
                _context.ProjectMembers.Remove(membership);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers
                .AnyAsync(projectMember =>
                    projectMember.ProjectId == projectId && projectMember.UserId == userId)
                || await _context.Projects
                    .AnyAsync(project => project.Id == projectId && project.OwnerId == userId);
        }

        public async Task<bool> IsProjectManagerAsync(Guid projectId, Guid userId)
        {
            var member = await _context.ProjectMembers.FirstOrDefaultAsync(projectMember =>
                projectMember.ProjectId == projectId &&
                projectMember.UserId == userId);

            return member != null && member.Role == "PM";
        }

        public async Task<IEnumerable<ProjectMemberResponse>> GetProjectMembersAsync(Guid projectId) =>
            await _context.ProjectMembers
                .Where(projectMember => projectMember.ProjectId == projectId)
                .Select(projectMember => new ProjectMemberResponse
                {
                    Id = projectMember.User.Id,
                    Username = projectMember.User.Username,
                    Name = projectMember.User.Name,
                    Role = projectMember.Role
                })
                .ToListAsync();

        public async Task UpdateMemberRoleAsync(
            Guid projectId,
            Guid userId,
            string role)
        {
            if (!ProjectRoles.IsValid(role)) throw new ArgumentException(
                $"Invalid role '{role}'. Valid roles are: {string.Join(", ", ProjectRoles.All)}");
                
            var membership = await _context.ProjectMembers.FirstOrDefaultAsync(projectMember =>
                projectMember.ProjectId == projectId &&
                projectMember.UserId == userId)
                    ?? throw new KeyNotFoundException("User is not a member of this project");

            membership.Role = role;
            _context.ProjectMembers.Update(membership);
            await _context.SaveChangesAsync();
        }
    }
}