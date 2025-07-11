using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyLib.Application.DTOs;
using MyLib.Domain.Entities;
using MyLib.Domain.Filter;
using MyLib.Domain.IRepositories;
using System.Linq;

namespace MyLib.Application.Handlers.Books.Queries.GetBooksList
{
    public class GetBooksListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetBooksListQueryHandler> logger) : IRequestHandler<GetBooksListQuery, PaginatedResult<BookDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<GetBooksListQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<PaginatedResult<BookDetailsDto>> Handle(GetBooksListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for books list. Page: {Page}, Size: {PageSize}", request.Page, request.PageSize);

            var filter = new BookFilter
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                Gender = request.Gender,
                Publisher = request.Publisher
            };

            var books = await _unitOfWork.BookRepository.GetByFilterAsync(filter, request.Page, request.PageSize);

            _logger.LogInformation("Found {Count} books", books.Count());

            // Use AutoMapper to map books to DTOs
            var bookDtos = _mapper.Map<IEnumerable<BookDetailsDto>>(books).ToList();

            return new PaginatedResult<BookDetailsDto>
            {
                Items = bookDtos,
                TotalCount = bookDtos.Count, // Simplified - ideally would have separate total count
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
} 