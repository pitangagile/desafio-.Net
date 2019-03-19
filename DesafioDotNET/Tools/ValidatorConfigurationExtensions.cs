﻿using Domains;
using FluentValidation;
using Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioDotNET
{
    public static class ValidatorConfigurationExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<ApplicationUserDto>, ApplicationUserDtoValidator>();
            services.AddTransient<IValidator<SigninDto>, SigninDtoValidator>();
            services.AddTransient<IValidator<SignupDto>, SignupDtoValidator>();
            services.AddTransient<IValidator<PhoneDto>, PhoneDtoValidator>();
			services.AddTransient<IValidator<ApplicationUser>, ApplicationUserValidator>();
			services.AddTransient<IValidator<Phone>, PhoneValidator>();

			return services;
        }
    }
}
