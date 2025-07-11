using MyLib.Application.Services;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RegisterUserCommandHandler> logger, IPasswordService passwordService) : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger = logger;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.UserRepository.UsernameExistsAsync(request.Username))
                throw new ApplicationException("Username already exists.");

            if (await _unitOfWork.UserRepository.EmailExistsAsync(request.Email))
                throw new ApplicationException("Email already exists.");

            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordService.HashPassword(request.Password);

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User registered: {UserId}", user.Id);
            return user.Id;
        }
    }
}