using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Books.Commands.RegisterBook
{
    public class RegisterBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RegisterBookCommandHandler> logger) : IRequestHandler<RegisterBookCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<RegisterBookCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<Guid> Handle(RegisterBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting book registration for user {UserId}", request.RegisteredByUserId);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.RegisteredByUserId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found when trying to register book", request.RegisteredByUserId);
                throw new ApplicationException($"User with ID {request.RegisteredByUserId} not found.");
            }

            var book = _mapper.Map<Book>(request);
            await _unitOfWork.BookRepository.AddAsync(book);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                logger.LogError("Failed to register book {BookId} for user {UserId}. No rows affected.", book.Id, request.RegisteredByUserId);
                throw new ApplicationException("Failed to register the book. No rows affected.");
            }

            _logger.LogInformation("Book registered successfully. ID: {BookId}, Title: {Title}", book.Id, book.Title);
            return book.Id;
        }
    }
}
