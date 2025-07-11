using MyLib.Application.DTOs;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetBookByIdQueryHandler> logger) : IRequestHandler<GetBookByIdQuery, BookDetailsDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<GetBookByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<BookDetailsDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for book with ID: {BookId}", request.Id);

            var book = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);

            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found", request.Id);
                throw new ApplicationException($"Book with ID {request.Id} not found.");
            }

            _logger.LogInformation("Book found successfully. ID: {BookId}, Title: {Title}", book.Id, book.Title);

            return _mapper.Map<BookDetailsDto>(book);
        }
    }
}