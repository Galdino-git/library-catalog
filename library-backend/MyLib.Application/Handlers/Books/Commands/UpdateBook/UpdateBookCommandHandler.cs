using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateBookCommandHandler> logger) : IRequestHandler<UpdateBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<UpdateBookCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting book update for ID: {BookId}", request.Id);

            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
            if (existingBook == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found for update", request.Id);
                throw new ApplicationException($"Book with ID {request.Id} not found.");
            }

            _mapper.Map(request, existingBook);
            existingBook.UpdateTimestamp();

            await _unitOfWork.BookRepository.UpdateAsync(existingBook);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                _logger.LogError("Failed to update book. No rows affected. ID: {BookId}", request.Id);
                throw new ApplicationException("Failed to update the book. No rows affected.");
            }

            _logger.LogInformation("Book updated successfully. ID: {BookId}, Title: {Title}", existingBook.Id, existingBook.Title);
        }
    }
}