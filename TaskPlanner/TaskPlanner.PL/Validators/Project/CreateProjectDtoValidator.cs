using FluentValidation;
using TaskPlanner.BLL.DTO.Project;

namespace TaskPlanner.PL.Validators.Project
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Project name should not be empty!")
                .MaximumLength(50)
                .WithMessage("Project name should have less then 50 characters!");
        }
    }
}
