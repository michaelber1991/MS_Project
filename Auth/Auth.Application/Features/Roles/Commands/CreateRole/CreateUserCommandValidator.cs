using Auth.Application.Features.Roles.Commands.CreateRole;
using FluentValidation;

namespace Auth.Application.Features.Applications.Commands.CreateApplication;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is Mandatory");
    }
}