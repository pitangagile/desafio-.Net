using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Domains;
using StackExchange.Redis;

namespace Infrastructure
{
	public abstract class BaseServiceRedis<T>
	{
		protected string Name => this.Type.Name;
		protected PropertyInfo[] Properties => this.Type.GetProperties();
		protected Type Type => typeof(T);

		protected string GenerateKey(string key)
		{
			return string.Concat(key.ToLower(), ":", this.Name.ToLower());
		}

		protected HashEntry[] GenerateHash(T obj)
		{
			var props = this.Properties;
			var hash = new HashEntry[props.Length];

			for (var i = 0; i < props.Length; i++)
				hash[i] = new HashEntry(props[i].Name, props[i].GetValue(obj).ToString());

			return hash;
		}

		protected T MapFromHash(HashEntry[] hash)
		{
			var obj = (T)Activator.CreateInstance(this.Type);
			var props = this.Properties;

			for (var i = 0; i < props.Length; i++)
			{
				for (var j = 0; j < hash.Length; j++)
				{
					if (props[i].Name == hash[j].Name)
					{
						var val = hash[j].Value;
						var type = props[i].PropertyType;

						if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
							if (string.IsNullOrEmpty(val))
							{
								props[i].SetValue(obj, null);
							}
						props[i].SetValue(obj, Convert.ChangeType(val, type));
					}
				}
			}
			return obj;
		}
	}
}
