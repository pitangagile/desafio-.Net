using System;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
	public class RedisConnectionFactory : IRedisConnectionFactory
	{
		private readonly Lazy<ConnectionMultiplexer> _connection;
		
		public RedisConnectionFactory(IOptions<RedisConfiguration> redis)
		{
			this._connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(string.Format("{0}:{1},password={2},allowAdmin=true", redis.Value.Host, redis.Value.Port, redis.Value.Token)));
		}

		public ConnectionMultiplexer Connection()
		{
			return this._connection.Value;
		}
	}
}
