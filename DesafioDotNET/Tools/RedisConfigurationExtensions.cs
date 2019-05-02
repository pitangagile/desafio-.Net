using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioDotNET
{
	public static class RedisConfigurationExtensions
	{
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
