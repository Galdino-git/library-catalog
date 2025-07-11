using MyLib.Application.Services;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordService passwordService) : IRequestHandler<ResetPasswordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordService _passwordService = passwordService;

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetRequest = await _unitOfWork.PasswordResetRepository.GetByTokenAsync(request.Token) ?? throw new ApplicationException("Invalid or expired reset token");
        if (resetRequest.Used)
            throw new ApplicationException("This reset token has already been used");

        if (resetRequest.Expiration < DateTime.UtcNow)
            throw new ApplicationException("This reset token has expired");

        var user = await _unitOfWork.UserRepository.GetByIdAsync(resetRequest.UserId) ?? throw new ApplicationException("User not found");

        user.PasswordHash = _passwordService.HashPassword(request.NewPassword);

        resetRequest.Used = true;
        resetRequest.UsedAt = DateTime.UtcNow;

        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.PasswordResetRepository.UpdateAsync(resetRequest);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}