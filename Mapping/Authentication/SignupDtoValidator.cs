using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mapping
{
    public class SignupDtoValidator: AbstractValidator<SignupDto>
    {
        public SignupDtoValidator(IValidator<PhoneDto> validator)
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Missing fields").WithErrorCode("422").EmailAddress().WithMessage("Invalid fields").WithErrorCode("422");
            RuleFor(v => v.FirstName).NotEmpty().WithMessage("Missing fields").WithErrorCode("422");
            RuleFor(v => v.LastName).NotEmpty().WithMessage("Missing fields").WithErrorCode("422");
            RuleFor(v => v.Phones).NotNull().WithMessage("Missing fields").WithErrorCode("422").NotEmpty().WithMessage("Missing fields").WithErrorCode("422");
            RuleForEach(v => v.Phones).SetValidator(validator);
        }
    }
}
