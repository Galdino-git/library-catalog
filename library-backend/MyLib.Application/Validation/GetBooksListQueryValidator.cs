using FluentValidation;
using MyLib.Application.Handlers.Books.Queries.GetBooksList;

namespace MyLib.Application.Validation
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

            RuleFor(x => x.Filter.Title)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Filter.Title))
                .WithMessage("Title filter must not exceed 200 characters.");

            RuleFor(x => x.Filter.Author)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Filter.Author))
                .WithMessage("Author filter must not exceed 100 characters.");

            RuleFor(x => x.Filter.ISBN)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.Filter.ISBN))
                .WithMessage("ISBN filter must not exceed 13 characters.");

            RuleFor(x => x.Filter.Publisher)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Filter.Publisher))
                .WithMessage("Publisher filter must not exceed 100 characters.");
        }
    }
}