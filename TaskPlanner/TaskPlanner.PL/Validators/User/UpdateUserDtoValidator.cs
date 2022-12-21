using FluentValidation;
using TaskPlanner.BLL.DTO.User;
using TaskPlanner.BLL.Validations;

namespace TaskPlanner.PL.Validators.User
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(u => u.UserID)
                .NotEmpty()
                .WithMessage("ID of user is required!");

            RuleFor(u => u.FirstName)
                .MinimumLength(3)
                .WithMessage("First name should have at least 3 characters!")
                .MaximumLength(50)
                .WithMessage("First name should have less than 50 characters!")
                .Must(f => FieldValidations.IsCorrectName(f))
                .WithMessage("Invalid first name!");

            RuleFor(u => u.FirstName)
                .MinimumLength(3)
                .WithMessage("Last name should have at least 3 characters!")
                .MaximumLength(50)
                .WithMessage("Last name should have less than 50 characters!")
                .Must(l => FieldValidations.IsCorrectName(l))
                .WithMessage("Invalid last name!");

            RuleFor(u => u.UserName)
                .MinimumLength(6)
                .WithMessage("Username shoud have at least 6 characters!")
                .MaximumLength(50)
                .WithMessage("Username should have less than 50 characters!")
                .Must(u => FieldValidations.IsCorrectUserName(u))
                .WithMessage("Invalid username!");

            RuleFor(u => u.Passwords)
                .MinimumLength(8)
                .WithMessage("Password shoud have at least 8 characters!")
                .MaximumLength(100)
                .WithMessage("Password should have less than 100 characters!")
                .Must(p => FieldValidations.IsCorrectPassword(p))
                .WithMessage("Invalid password!");
        }
    }
}
