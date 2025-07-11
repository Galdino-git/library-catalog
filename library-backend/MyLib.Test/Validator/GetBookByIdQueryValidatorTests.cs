using MyLib.Application.Handlers.Books.Queries.GetBookById;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator
{
    public class GetBookByIdQueryValidatorTests
    {
        private readonly GetBookByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            var query = new GetBookByIdQuery { Id = Guid.Empty };
            var result = _validator.Validate(query);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id");
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var query = new GetBookByIdQuery { Id = Guid.NewGuid() };
            var result = _validator.Validate(query);
            Assert.True(result.IsValid);
        }
    }
}