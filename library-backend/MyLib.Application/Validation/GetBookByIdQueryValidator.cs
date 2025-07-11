using FluentValidation;
using MyLib.Application.Handlers.Books.Queries.GetBookById;

namespace MyLib.Application.Validation
{
    public class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
    {
        public GetBookByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Book ID is required.");
        }
    }
} 