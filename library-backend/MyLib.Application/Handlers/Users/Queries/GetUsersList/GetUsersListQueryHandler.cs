using MyLib.Application.DTOs;
using MyLib.Application.Handlers.Users.DTOs;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUsersListQuery, PaginatedResult<UserListDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedResult<UserListDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(request.Page, request.PageSize);
        var userDtos = _mapper.Map<List<UserListDto>>(users);

        return new PaginatedResult<UserListDto>
        {
            Items = userDtos,
            TotalCount = userDtos.Count,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}