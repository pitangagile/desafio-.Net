using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
	public interface IRedisService<T>
	{
		T Get(string key);
		void Save(string key, T obj);
		void Delete(string key);
	}
}
