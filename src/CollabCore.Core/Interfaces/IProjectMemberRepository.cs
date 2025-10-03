namespace CollabCore.Core.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId);
    }
}