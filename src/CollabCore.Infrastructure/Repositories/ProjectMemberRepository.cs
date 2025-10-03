using CollabCore.Core.Interfaces;
using CollabCore.Infrastructure.Data;
using CollabCore.Core.Entities;
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

        public async Task AddProjectMemberAsync(Guid projectId, Guid userId)
        {
            if (!await _context.ProjectMembers
                .AnyAsync(projectMember => projectMember.ProjectId == projectId &&
                    projectMember.UserId == userId)
            )
            {
                _context.ProjectMembers.Add(new ProjectMember
                {
                    ProjectId = projectId,
                    UserId = userId
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

        public async Task<IEnumerable<User>> GetProjectMembersAsync(Guid projectId) =>
            await _context.ProjectMembers
                .Where(projectMember => projectMember.ProjectId == projectId)
                .Select(projectMember => projectMember.User)
                .ToListAsync();
    }
}