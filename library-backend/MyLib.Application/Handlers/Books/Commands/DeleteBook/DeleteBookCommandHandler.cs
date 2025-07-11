using MyLib.Domain.Entities;
using MyLib.Domain.IRepositories;

namespace MyLib.Application.Handlers.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteBookCommandHandler> logger) : IRequestHandler<DeleteBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteBookCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting book deletion for ID: {BookId}", request.Id);

            Book? existingBook = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
            if (existingBook == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found for deletion", request.Id);
                throw new ApplicationException($"Book with ID {request.Id} not found.");
            }

            await _unitOfWork.BookRepository.DeleteAsync(request.Id);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                _logger.LogError("Failed to delete book. No rows affected. ID: {BookId}", request.Id);
                throw new ApplicationException("Failed to delete the book. No rows affected.");
            }

            _logger.LogInformation("Book deleted successfully. ID: {BookId}, Title: {Title}", existingBook.Id, existingBook.Title);
        }
    }
}