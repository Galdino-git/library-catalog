using MyLib.Application.Handlers.Users.DTOs;

namespace MyLib.Application.Handlers.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDetailsDto>
    {
        public Guid Id { get; set; }
    }
}