namespace MyLib.Application.Handlers.Users.DTOs
{
    public class UserLoginDto
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}