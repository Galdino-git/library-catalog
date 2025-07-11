using MyLib.Application.Services;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IUnitOfWork unitOfWork, IPasswordService passwordService, IJwtService jwtService) : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.UsernameOrEmail)
                   ?? await _unitOfWork.UserRepository.GetByUsernameAsync(request.UsernameOrEmail);

        if (user == null)
            throw new ApplicationException("Invalid username/email or password");

        if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            throw new ApplicationException("Invalid username/email or password");

        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username, user.Email);
        return token;
    }
}