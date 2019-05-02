using Domains;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class ServiceCollectionExtension
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

		public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration Configuration)
		{
			//services.Configure<RedisConfiguration>(Configuration.GetSection("Redis"));

			//services.AddDistributedRedisCache(options =>
			//{
			//	options.InstanceName = Configuration.GetValue<string>("Redis:Name");
			//	options.Configuration = Configuration.GetValue<string>("Redis:Host");
			//});

			//services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();

			return services;
		}
	}
}
