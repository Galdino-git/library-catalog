using MyLib.Application.Handlers.Users.DTOs;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
        if (user == null)
            return null!;

        return _mapper.Map<UserDetailsDto>(user);
    }
}