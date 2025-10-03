using CollabCore.Infrastructure.Data;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;
using CollabCore.Contracts.Requests;
using CollabCore.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace CollabCore.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetPagedAsync(ProjectQuery query)
        {
            var projects = _context.Projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                projects = projects.Where(project => project.Name.Contains(query.Search)
                    || (project.Description != null
                        && project.Description.Contains(query.Search)));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                projects = query.SortBy.ToLower() switch
                {
                    "name" => query.SortDesc
                        ? projects.OrderByDescending(p => p.Name)
                        : projects.OrderBy(p => p.Name),
                    "createdat" => query.SortDesc
                        ? projects.OrderByDescending(p => p.CreatedAt)
                        : projects.OrderBy(p => p.CreatedAt),
                    _ => projects.OrderBy(p => p.CreatedAt)
                };
            }
            else
            {
                projects = projects.OrderBy(p => p.CreatedAt);
            }

            return await projects
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<int> CountAsync(ProjectQuery query)
        {
            var projects = _context.Projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                projects = projects.Where(p => p.Name.Contains(query.Search) ||
                    (p.Description != null && p.Description.Contains(query.Search)));
            }

            return await projects.CountAsync();
        }

        public async Task<Project> AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsProjectOwnerAsync(Guid projectId, Guid userId)
        {
            return await _context.Projects.AnyAsync(project =>
                project.Id == projectId &&
                project.OwnerId == userId);
        }
    }
}