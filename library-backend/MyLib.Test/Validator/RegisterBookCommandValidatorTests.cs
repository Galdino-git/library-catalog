using MyLib.Application.Handlers.Books.Commands.RegisterBook;
using MyLib.Application.Validation;

namespace MyLib.Test.Validator
{
    public class RegisterBookCommandValidatorTests
    {
        private readonly RegisterBookCommandValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Title_Is_Empty()
        {
            var command = new RegisterBookCommand { Title = "" };
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Title");
        }

        [Fact]
        public void Should_Fail_When_ISBN_TooLong()
        {
            var command = new RegisterBookCommand
            {
                Title = "Valid",
                Author = "Valid",
                ISBN = new string('1', 14),
                Gender = "Fiction",
                Publisher = "Valid",
                PublishedYear = 2020,
                Synopsis = "Valid",
                RegisteredByUserId = Guid.NewGuid()
            };

            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "ISBN");
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var command = new RegisterBookCommand
            {
                Title = "Valid",
                Author = "Author",
                ISBN = "1234567890123",
                Gender = "Fiction",
                Publisher = "Pub",
                PublishedYear = 2020,
                Synopsis = "Synopsis",
                RegisteredByUserId = Guid.NewGuid()
            };
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }
    }
}