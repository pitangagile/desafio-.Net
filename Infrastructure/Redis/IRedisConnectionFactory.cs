using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Infrastructure
{
	public interface IRedisConnectionFactory
	{
		ConnectionMultiplexer Connection();
	}
}
