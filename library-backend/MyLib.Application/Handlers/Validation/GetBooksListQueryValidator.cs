using FluentValidation;
using MyLib.Application.Handlers.Books.Queries.GetBooksList;

namespace MyLib.Application.Handlers.Validation
{
    public class GetBooksListQueryValidator : AbstractValidator<GetBooksListQuery>
    {
        public GetBooksListQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");

            RuleFor(x => x.Title)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("Title filter must not exceed 200 characters.");

            RuleFor(x => x.Author)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Author))
                .WithMessage("Author filter must not exceed 100 characters.");

            RuleFor(x => x.ISBN)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.ISBN))
                .WithMessage("ISBN filter must not exceed 13 characters.");

            RuleFor(x => x.Publisher)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Publisher))
                .WithMessage("Publisher filter must not exceed 100 characters.");
        }
    }
} 