using System;
using System.Reflection;
using Infrastructure;
using Newtonsoft.Json;

namespace Infrastructure
{
	public abstract class BaseServiceRedis<TEntity> where TEntity : class
	{
		private string Name => this._type.Name;
		private PropertyInfo[] Properties => this._type.GetProperties();
		private Type _type => typeof(TEntity);
		protected readonly IRedisConnectionFactory _connectionFactory;
		internal readonly IDatabase _dBRedis;

		public BaseServiceRedis(IRedisConnectionFactory connectionFactory)
		{
			this._connectionFactory = connectionFactory;
			this._dBRedis = this._connectionFactory.Connection().GetDatabase();
		}

		/// <summary>
		/// Apply cryptography and descryptography in mapfromhash and generatehash
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private string GenerateKey(string key)
		{
			return string.Concat(key.ToLower(), ":", this.Name.ToLower());
		}

		private string GenerateHash(TEntity obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
		}

		private TEntity MapFromHash(string hash)
		{
			var obj = JsonConvert.DeserializeObject<TEntity>(hash, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

			return obj;
		}

		public void DeleteCache(string key)
		{
			if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

			key = this.GenerateKey(key);
			_dBRedis.KeyDelete(key);
		}

		public object GetCache(string key)
		{
			key = this.GenerateKey(key);
			var hash = _dBRedis.StringGet(key);

			if (hash.IsNull)
			{
				return null;
			}
			return this.MapFromHash(hash);
		}

		public void SaveCache(string key, TEntity obj)
		{
			if (obj != null)
			{
				var hash = this.GenerateHash(obj);
				key = this.GenerateKey(key);

				if (_dBRedis.HashLength(key) == 0)
				{
					_dBRedis.StringSet(key, hash);
				}
				else
				{
					var props = this.Properties;
					foreach (var item in props)
					{
						if (_dBRedis.HashExists(key, item.Name))
						{
							_dBRedis.HashIncrement(key, item.Name, Convert.ToInt32(item.GetValue(obj)));
						}
					}
				}

			}
		}

		public void UpdateCache(string key, TEntity obj)
		{
			DeleteCache(key);
			SaveCache(key, obj);
		}

	}
}