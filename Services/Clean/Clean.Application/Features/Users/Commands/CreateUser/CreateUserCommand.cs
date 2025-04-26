using AutoMapper;
using Clean.Application.Features.Users.Events;
using Clean.Application.Interfaces;
using Clean.Application.Models;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string Name, string Email) : IRequest<Result<User>>;

public class CreateUserHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
    : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IMediator _mediator = mediator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        await _mediator.Publish(new UsersCreatedEvent(5), cancellationToken);

        return Result<User>.Ok(user, "User created");
    }
}