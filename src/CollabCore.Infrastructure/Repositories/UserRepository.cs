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
    }
}