using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;
using CollabCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabCore.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentsRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid taskId, Guid userId)
        {
            return await _context.Assignments.AnyAsync(assignment =>
                assignment.TaskId == taskId &&
                assignment.UserId == userId);
        }
    }
}