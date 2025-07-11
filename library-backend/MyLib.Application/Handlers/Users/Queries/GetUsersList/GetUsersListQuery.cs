using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Users.DTOs;

namespace MyLib.Application.Handlers.Users.Queries.GetUsersList;

public class GetUsersListQuery : IRequest<PaginatedResult<UserListDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}