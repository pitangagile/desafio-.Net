using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mapping
{
    public class SigninDtoValidator: AbstractValidator<SigninDto>
    {
        public SigninDtoValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Missing fields").WithErrorCode("422").EmailAddress().WithMessage("Invalid fields").WithErrorCode("422");
            RuleFor(v => v.Password).NotEmpty().WithMessage("Missing fields").WithErrorCode("422");
        }
    }
}
