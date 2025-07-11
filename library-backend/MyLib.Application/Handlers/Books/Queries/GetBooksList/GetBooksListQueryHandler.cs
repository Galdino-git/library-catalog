using MyLib.Application.DTOs;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Books.Queries.GetBooksList
{
    public class GetBooksListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetBooksListQueryHandler> logger) : IRequestHandler<GetBooksListQuery, PaginatedResult<BookDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<GetBooksListQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<PaginatedResult<BookDetailsDto>> Handle(GetBooksListQuery request, CancellationToken cancellationToken)
        {
            var (totalCount, books) = await _unitOfWork.BookRepository.GetByFilterAsync(request.Filter, request.Page, request.PageSize);

            _logger.LogInformation("Showing {PaginatedCount} from {Count} total books", books.Count(), totalCount);

            var bookDtos = _mapper.Map<IEnumerable<BookDetailsDto>>(books).ToList();

            return new PaginatedResult<BookDetailsDto>
            {
                Items = bookDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}