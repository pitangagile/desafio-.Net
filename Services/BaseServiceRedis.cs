using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domains;
using Infrastructure;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Services
{
	public abstract class BaseServiceRedis<T> where T : IBaseDomain
	{
		private string Name => this.Type.Name;
		private PropertyInfo[] Properties => this.Type.GetProperties();
		private Type Type => typeof(T);
		protected readonly IRedisConnectionFactory _connectionFactory;
		internal readonly IDatabase _dB;

		public BaseServiceRedis(IRedisConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
			_dB = _connectionFactory.Connection().GetDatabase();
		}

		private string GenerateKey(string key)
		{
			return string.Concat(key.ToLower(), ":", this.Name.ToLower());
		}

		private string GenerateHash(T obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
		}

		private T MapFromHash(string hash)
		{
			var obj = JsonConvert.DeserializeObject<T>(hash, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

			return obj;
		}

		public void DeleteCache(string key)
		{
			if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

			key = this.GenerateKey(key);
			_dB.KeyDelete(key);
		}

		public object GetCache(string key)
		{
			key = this.GenerateKey(key);
			var hash = _dB.StringGet(key);

			if (hash.IsNull)
			{
				return null;
			}
			return this.MapFromHash(hash);
		}

		public void SaveCache(string key, T obj)
		{
			if (obj != null)
			{
				var hash = this.GenerateHash(obj);
				key = this.GenerateKey(key);

				if (_dB.HashLength(key) == 0)
				{
					_dB.StringSet(key, hash);
				}
				else
				{
					var props = this.Properties;
					foreach (var item in props)
					{
						if (_dB.HashExists(key, item.Name))
						{
							_dB.HashIncrement(key, item.Name, Convert.ToInt32(item.GetValue(obj)));
						}
					}
				}

			}
		}
	}
}
