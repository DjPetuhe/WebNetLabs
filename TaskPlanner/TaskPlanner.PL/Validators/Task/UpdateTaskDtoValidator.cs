using FluentValidation;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.Validations;

namespace TaskPlanner.PL.Validators.Task
{
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .WithMessage("Task title should not be empty!")
                .MaximumLength(50)
                .WithMessage("Task title should have less then 50 characters!");

            RuleFor(t => t.Description)
                .MinimumLength(5)
                .WithMessage("Task description should have at least 5 characters!");

            RuleFor(t => t.TaskID)
                .NotEmpty()
                .WithMessage("ID of task is required!");

            RuleFor(t => t.Deadline)
                .Must(time => FieldValidations.IsCorrectDate(time))
                .WithMessage("Invalid deadline!");
        }
    }
}
