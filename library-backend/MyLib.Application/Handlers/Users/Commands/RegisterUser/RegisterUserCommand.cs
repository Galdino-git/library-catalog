namespace MyLib.Application.Handlers.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}