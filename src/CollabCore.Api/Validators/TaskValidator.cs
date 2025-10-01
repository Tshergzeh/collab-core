using FluentValidation;
using CollabCore.Contracts.Requests;

namespace CollabCore.Api.Validators
{
    public class TaskCreateValidator : AbstractValidator<TaskCreateDto>
    {
        public TaskCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required")
                .MaximumLength(200);

            RuleFor(x => x.Status)
                .Must(s => new[] { "Todo", "InProgress", "Done" }.Contains(s))
                .WithMessage("Invalid status");

            RuleFor(x => x.Priority)
                .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
                .WithMessage("Invalid priority");
        }
    }

    public class TaskUpdateValidator : AbstractValidator<TaskUpdateDto>
    {
        public TaskUpdateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required")
                .MaximumLength(200);
        }
    }
}