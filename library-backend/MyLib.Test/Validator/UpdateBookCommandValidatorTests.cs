using MyLib.Application.Handlers.Books.Commands.UpdateBook;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator
{
    public class UpdateBookCommandValidatorTests
    {
        private readonly UpdateBookCommandValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            var command = new UpdateBookCommand { Id = Guid.Empty };
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id");
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var command = new UpdateBookCommand
            {
                Id = Guid.NewGuid(),
                Title = "Valid",
                Author = "Author",
                ISBN = "1234567890123",
                Gender = "Fiction",
                Publisher = "Pub",
                PublishedYear = 2020,
                Synopsis = "Synopsis"
            };
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }
    }
}