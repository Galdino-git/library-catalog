using FluentValidation;
using MyLib.Application.Handlers.Books.Commands.UpdateBook;

namespace MyLib.Application.Validation
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100).WithMessage("Author must not exceed 100 characters.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .MaximumLength(20).WithMessage("ISBN must not exceed 20 characters.");

            RuleFor(x => x.Publisher)
                .NotEmpty().WithMessage("Publisher is required.")
                .MaximumLength(100).WithMessage("Publisher must not exceed 100 characters.");

            RuleFor(x => x.PublishedYear)
                .NotEmpty().WithMessage("Published Year is required.");

            RuleFor(x => x.Synopsis)
                .NotEmpty().WithMessage("Synopsis is required.")
                .MaximumLength(5000).WithMessage("Synopsis must not exceed 5000 characters.");

            RuleFor(x => x.CoverImageUrl)
                .MaximumLength(2048).WithMessage("Cover Image URL must not exceed 2048 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.");
        }
    }
} 