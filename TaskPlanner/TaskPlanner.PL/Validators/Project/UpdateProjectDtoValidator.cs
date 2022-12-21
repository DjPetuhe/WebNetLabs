using FluentValidation;
using TaskPlanner.BLL.DTO.Project;

namespace TaskPlanner.PL.Validators.Project
{
    public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Project name should not be empty!")
                .MaximumLength(50)
                .WithMessage("Project name should have less then 50 characters!");

            RuleFor(p => p.ProjectID)
                .NotEmpty()
                .WithMessage("ID of project is required!");
        }
    }
}
