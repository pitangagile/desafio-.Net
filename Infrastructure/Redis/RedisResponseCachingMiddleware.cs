using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Redis
{
	internal class RedisResponseCachingMiddleware : ResponseCachingMiddleware
	{
		private RedisResponseCache Cache
		{
			set
			{
				FieldInfo cacheFieldInfo = typeof(ResponseCachingMiddleware)
					.GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);

				cacheFieldInfo.SetValue(this, value);
			}
		}

		public RedisResponseCachingMiddleware(RequestDelegate next, IOptions<RedisResponseOptions> options,
			ILoggerFactory loggerFactory, IResponseCachingPolicyProvider policyProvider,
			IResponseCachingKeyProvider keyProvider)
			: base(next, options, loggerFactory, policyProvider, keyProvider)
		{
			Cache = new RedisResponseCache(options.Value.Host);
		}
	}
}
