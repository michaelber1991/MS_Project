using FluentValidation;

namespace Clean.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is Mandatory");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("Invalid Email");
    }
}