using Mapping;
using Xunit;

namespace Tests
{
    public class ValidatorTest
    {
        public ValidatorTest()
        {
        }

        [Fact(DisplayName = "Validação de ApplicationUserDto")]
        public void ApplicationUserDtoValidator()
        {
            var validatorPhone = new PhoneDtoValidator();
            var validator = new ApplicationUserDtoValidator(validatorPhone);

            ApplicationUserDto dto = new ApplicationUserDto { };
            var validated = validator.Validate(dto);

            Assert.False(validated.IsValid);
        }

        [Fact(DisplayName = "Validação de PhoneDto")]
        public void PhoneValidator()
        {
            var validator = new PhoneDtoValidator();
            PhoneDto dto = new PhoneDto
            {
                Area_code = 123,
                Country_code = "+55",
                Number = 123
            };
            var validated = validator.Validate(dto);

            Assert.True(validated.IsValid);
        }
    }
}
