namespace MyLib.Application.Handlers.Users.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<string>
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}