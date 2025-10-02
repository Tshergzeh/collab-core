using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;
using CollabCore.Core.Common;

namespace CollabCore.Application.Services
{
    public class ProjectAppService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectAppService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<(IEnumerable<ProjectResponse> Items, int TotalItems)> 
            GetProjects(QueryParameters query)
        {
            var domainQuery = new ProjectQuery
            {
                Search = query.Search,
                SortBy = query.SortBy,
                SortDesc = query.SortDesc,
                Page = query.Page,
                PageSize = query.PageSize
            };

            var items = await _projectRepository.GetPagedAsync(domainQuery);
            var total = await _projectRepository.CountAsync(domainQuery);
            
            return (items.Select(project => new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            }), total);
        }

        public async Task<ProjectResponse?> GetProject(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            };
        }

        public async Task<ProjectResponse> CreateProject(ProjectCreateDto dto, Guid userId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = userId
            };

            await _projectRepository.AddAsync(project);

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            };
        }

        public async Task<ProjectResponse?> UpdateProject(Guid id, ProjectUpdateDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.ModifiedAt = DateTime.UtcNow;

            await _projectRepository.UpdateAsync(project);

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            };
        }

        public async Task<bool> DeleteProject(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return false;

            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}