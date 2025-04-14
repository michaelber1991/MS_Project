using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Services;

namespace Auth.Application.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
}