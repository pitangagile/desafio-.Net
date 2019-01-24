using FluentValidation;

namespace Mapping
{
    public class PhoneDtoValidator : AbstractValidator<PhoneDto>
    {
        public PhoneDtoValidator()
        {
            RuleFor(v => v.Area_code).GreaterThan(0).WithMessage("Missing fields").WithErrorCode("422");
            RuleFor(v => v.Country_code).NotEmpty().WithMessage("Missing fields").WithErrorCode("422");
            RuleFor(v => v.Number).GreaterThan(0).WithMessage("Missing fields").WithErrorCode("422");
        }
    }
}
