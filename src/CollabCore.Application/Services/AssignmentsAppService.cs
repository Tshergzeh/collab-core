using CollabCore.Contracts.Requests;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class AssignmentsAppService
    {
        private readonly IAssignmentsRepository _assignmentsRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public AssignmentsAppService(
            IAssignmentsRepository assignmentsRepository,
            IProjectMemberRepository projectMemberRepository,
            ITaskRepository taskRepository,
            IProjectRepository projectRepository,
            ICurrentUserService currentUser)
        {
            _assignmentsRepository = assignmentsRepository;
            _projectMemberRepository = projectMemberRepository;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task AssignTaskAsync(AssignTaskRequest request)
        {
            _ = await _taskRepository.GetByIdAsync(request.ProjectId, request.TaskId)
                ?? throw new Exception("Task not found");

            var project = await _projectRepository.GetByIdWithMembersAsync(request.ProjectId) 
                ?? throw new Exception("Project not found");

            var currentUserId = _currentUser.GetCurrentUserId();

            bool isOwner = project.OwnerId == currentUserId;
            bool isProjectManager = project.ProjectMembers
                .Any(projectMember => projectMember.UserId == currentUserId &&
                    projectMember.Role == "PM");

            if (!isOwner && !isProjectManager)
                throw new Exception(
                    "Only the project owner or project managers can assign tasks");

            bool targetIsMember = project.ProjectMembers.Any(projectMember =>
                projectMember.UserId == request.UserId);
            if (!targetIsMember)
                throw new Exception("Target user is not a member of this project");

            if (await _assignmentsRepository.ExistsAsync(request.TaskId, request.UserId))
                throw new Exception("This user is already assigned to this task");

            await _assignmentsRepository.AddAsync(new Assignment
            {
                TaskId = request.TaskId,
                UserId = request.UserId
            });
        }
    }
}