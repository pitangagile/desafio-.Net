using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.ResponseCaching;

namespace Infrastructure
{
	public class RedisResponseOptions: ResponseCachingOptions
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Name { get; set; }
		public string Token { get; set; }
	}
}
