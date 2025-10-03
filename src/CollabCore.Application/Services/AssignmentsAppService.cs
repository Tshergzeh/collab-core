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

        public AssignmentsAppService(IAssignmentsRepository assignmentsRepository,
            IProjectMemberRepository projectMemberRepository,
            ITaskRepository taskRepository)
        {
            _assignmentsRepository = assignmentsRepository;
            _projectMemberRepository = projectMemberRepository;
            _taskRepository = taskRepository;
        }

        public async Task AssignTaskAsync(AssignTaskRequest request)
        {
            _ = await _taskRepository.GetByIdAsync(request.ProjectId, request.TaskId)
                ?? throw new Exception("Task not found");
            var isMember = await _projectMemberRepository.IsProjectMemberAsync(request.ProjectId,
                request.UserId);
            if (!isMember) throw new Exception("User is not a member of this project");

            var assignment = new Assignment
            {
                TaskId = request.TaskId,
                UserId = request.UserId
            };

            await _assignmentsRepository.AddAsync(assignment);
        }
    }
}