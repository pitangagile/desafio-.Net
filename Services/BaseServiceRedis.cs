using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domains;
using Infrastructure;
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

		private HashEntry[] GenerateHash(T obj)
		{
			var hash = new List<HashEntry>();

			foreach (PropertyInfo info in this.Properties)
			{
				if (!info.GetValue(obj).IsNull())
				{
					hash.Add(new HashEntry(info.Name, info.GetValue(obj).ToString()));
				}
			}

			return hash.ToArray();
		}

		private T MapFromHash(HashEntry[] hash)
		{
			var obj = (T)Activator.CreateInstance(this.Type);
			var props = this.Properties;

			foreach (PropertyInfo info in props)
			{
				var propertyFinded = hash.FirstOrDefault(h => h.Name == info.Name && h.Value.IsNull() == false);
				if (!propertyFinded.Value.IsNull)
				{
					if (!info.PropertyType.IsConstructedGenericType) {
						var insertedValue = hash.FirstOrDefault(h => h.Name == info.Name).Value;
						info.SetValue(obj, Convert.ChangeType(insertedValue, info.PropertyType));
					}
				}
			}
			
			return obj;
		}

		public void DeleteCache(string key)
		{
			if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

			key = this.GenerateKey(key);
			_dB.KeyDelete(key);
		}

		public T GetCache(string key)
		{
			key = this.GenerateKey(key);
			var hash = _dB.HashGetAll(key);
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
					_dB.HashSet(key, hash);
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
