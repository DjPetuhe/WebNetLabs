using FluentValidation;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.Validations;

namespace TaskPlanner.PL.Validators.Task
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .WithMessage("Task title should not be empty!")
                .MaximumLength(50)
                .WithMessage("Task title should have less then 50 characters!");

            RuleFor(t => t.Description)
                .MinimumLength(5)
                .WithMessage("Task description should have at least 5 characters!");

            RuleFor(t => t.ProjectID)
                .NotEmpty()
                .WithMessage("ID of project is required!");

            RuleFor(t => t.Deadline)
                .Must(time => FieldValidations.IsCorrectDate(time))
                .WithMessage("Invalid deadline!");
        }
    }
}
