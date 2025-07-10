using MediatR;
using Microsoft.Extensions.Logging;
using MyLib.Application.DTOs;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetBookByIdQueryHandler> logger) : IRequestHandler<GetBookByIdQuery, BookDetailsDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Gender = book.Gender,
                Publisher = book.Publisher,
                PublishedYear = book.PublishedYear,
                Synopsis = book.Synopsis,
                CoverImage = book.CoverImage,
                CoverImageUrl = book.CoverImageUrl,
                RegisteredByUserName = book.RegisteredByUser?.Username ?? string.Empty,
                LastUpdated = book.UpdatedAt ?? book.CreatedAt
            };
        }
    }
}