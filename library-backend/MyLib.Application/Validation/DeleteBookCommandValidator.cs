using FluentValidation;
using MyLib.Application.Handlers.Books.Commands.DeleteBook;

namespace MyLib.Application.Validation
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Book ID is required.");
        }
    }
} 