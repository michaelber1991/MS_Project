using AutoMapper;
using Clean.Application.Interfaces;
using Clean.Application.Models;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string Name, string Email) : IRequest<Result<User>>;

public class CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserCommand, Result<User>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return Result<User>.Ok(user, "User created");
    }
}