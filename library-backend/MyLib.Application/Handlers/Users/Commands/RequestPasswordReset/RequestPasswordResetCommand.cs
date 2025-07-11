namespace MyLib.Application.Handlers.Users.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommand : IRequest<Unit>
    {
        public string Email { get; set; } = string.Empty;
    }
}