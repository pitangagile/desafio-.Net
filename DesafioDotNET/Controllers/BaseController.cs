using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace DesafioDotNET
{
	[AllowAnonymous]
	[EnableCors("FrontEnd")]
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : Controller
    {
		protected readonly RedisConfiguration _redis;
		protected readonly IDistributedCache _cache;
		protected readonly IRedisConnectionFactory _fact;

		public BaseController(IOptions<RedisConfiguration> redis, IDistributedCache cache, IRedisConnectionFactory factory)
		{
			this._cache = cache;
			this._redis = redis.Value;
			this._fact = factory;
		}
    }
}