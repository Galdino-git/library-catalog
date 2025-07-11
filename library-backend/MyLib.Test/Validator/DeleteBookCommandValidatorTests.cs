using MyLib.Application.Handlers.Books.Commands.DeleteBook;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator
{
    public class DeleteBookCommandValidatorTests
    {
        private readonly DeleteBookCommandValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            var command = new DeleteBookCommand { Id = Guid.Empty };
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id");
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var command = new DeleteBookCommand { Id = Guid.NewGuid() };
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }
    }
}