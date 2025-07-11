using MyLib.Application.Handlers.Books.Queries.GetBooksList;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator
{
    public class GetBooksListQueryValidatorTests
    {
        private readonly GetBooksListQueryValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Page_Is_Less_Than_One()
        {
            var query = new GetBooksListQuery { Page = 0, PageSize = 10 };
            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Page");
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var query = new GetBooksListQuery { Page = 1, PageSize = 10 };
            var result = _validator.Validate(query);
            Assert.True(result.IsValid);
        }
    }
}