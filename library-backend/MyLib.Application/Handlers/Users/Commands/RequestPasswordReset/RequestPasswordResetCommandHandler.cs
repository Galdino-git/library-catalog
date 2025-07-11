using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;
using MyLib.Application.Services;
using Microsoft.Extensions.Configuration;

namespace MyLib.Application.Handlers.Users.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration configuration) : IRequestHandler<RequestPasswordResetCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<Unit> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return Unit.Value;

        var token = Guid.NewGuid().ToString();
        var expiration = DateTime.UtcNow.AddHours(24);

        var passwordResetRequest = new PasswordResetRequest
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = token,
            Expiration = expiration,
            Used = false
        };

        await _unitOfWork.PasswordResetRepository.AddAsync(passwordResetRequest);
        await _unitOfWork.SaveChangesAsync();

        // Enviar e-mail com link de redefinição
        var frontendUrl = _configuration["Frontend:ResetPasswordUrl"] ?? "http://localhost:4200/reset-password";
        var resetLink = $"{frontendUrl}?token={token}";
        var subject = "Recuperação de senha";
        var htmlContent = $"<p>Para redefinir sua senha, clique no link abaixo:</p><p><a href='{resetLink}'>Redefinir senha</a></p>";
        await _emailService.SendEmailAsync(user.Email, subject, htmlContent);

        return Unit.Value;
    }
}