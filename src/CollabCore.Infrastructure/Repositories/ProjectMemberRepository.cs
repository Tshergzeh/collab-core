using CollabCore.Core.Interfaces;
using CollabCore.Infrastructure.Data;
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

        public async Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers
                .AnyAsync(projectMember =>
                    projectMember.ProjectId == projectId && projectMember.UserId == userId)
                || await _context.Projects
                    .AnyAsync(project => project.Id == projectId && project.OwnerId == userId);
        }
    }
}