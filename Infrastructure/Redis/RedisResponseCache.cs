using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Primitives;
using StackExchange.Redis;

namespace Infrastructure.Redis
{
	internal class RedisResponseCache : IResponseCache
	{
		//private ConnectionMultiplexer _redis;

		public RedisResponseCache(string redisConnectionMultiplexerConfiguration)
		{
			if (String.IsNullOrWhiteSpace(redisConnectionMultiplexerConfiguration))
			{
				throw new ArgumentNullException(nameof(redisConnectionMultiplexerConfiguration));
			}

			//_redis = ConnectionMultiplexer.Connect(redisConnectionMultiplexerConfiguration);
		}

		public IResponseCacheEntry Get(string key)
		{
			throw new NotImplementedException();
		}

		public async Task<IResponseCacheEntry> GetAsync(string key)
		{
			IResponseCacheEntry responseCacheEntry = null;

			//IDatabase redisDatabase = _redis.GetDatabase();

			//HashEntry[] hashEntries = await redisDatabase.HashGetAllAsync(key);

			//string type = hashEntries.First(e => e.Name == "Type").Value;
			//if (type == nameof(CachedResponse))
			//{
			//	HashEntry[] headersHashEntries = await redisDatabase.HashGetAllAsync(key + "_Headers");

			//	if ((headersHashEntries != null) && (headersHashEntries.Length > 0)
			//		&& (hashEntries != null) && (hashEntries.Length > 0))
			//	{
			//		CachedResponse cachedResponse = CachedResponseFromHashEntryArray(hashEntries);
			//		cachedResponse.Headers = HeaderDictionaryFromHashEntryArray(headersHashEntries);

			//		responseCacheEntry = cachedResponse;
			//	}
			//}
			//else if (type == nameof(CachedVaryByRules))
			//{
			//}

			return responseCacheEntry;
		}

		public void Set(string key, IResponseCacheEntry entry, TimeSpan validFor)
		{
			throw new NotImplementedException();
		}

		public async Task SetAsync(string key, IResponseCacheEntry entry, TimeSpan validFor)
		{
			if (entry is CachedResponse cachedResponse)
			{
				string headersKey = key + "_Headers";

				//IDatabase redisDatabase = _redis.GetDatabase();

				//await redisDatabase.HashSetAsync(key, CachedResponseToHashEntryArray(cachedResponse));
				//await redisDatabase.HashSetAsync(headersKey, HeaderDictionaryToHashEntryArray(cachedResponse.Headers));

				//await redisDatabase.KeyExpireAsync(headersKey, validFor);
				//await redisDatabase.KeyExpireAsync(key, validFor);
			}
			else if (entry is CachedVaryByRules cachedVaryByRules)
			{
			}
		}

		//private HashEntry[] CachedResponseToHashEntryArray(CachedResponse cachedResponse)
		//{
		//	MemoryStream bodyStream = new MemoryStream();
		//	cachedResponse.Body.CopyTo(bodyStream);

		//	return new HashEntry[]
		//	{
		//	new HashEntry("Type", nameof(CachedResponse)),
		//	new HashEntry(nameof(cachedResponse.Created), cachedResponse.Created.ToUnixTimeMilliseconds()),
		//	new HashEntry(nameof(cachedResponse.StatusCode), cachedResponse.StatusCode),
		//	new HashEntry(nameof(cachedResponse.Body), bodyStream.ToArray())
		//	};
		//}

		//private HashEntry[] HeaderDictionaryToHashEntryArray(IHeaderDictionary headerDictionary)
		//{
		//	HashEntry[] headersHashEntries = new HashEntry[headerDictionary.Count];

		//	int headersHashEntriesIndex = 0;
		//	foreach (KeyValuePair<string, StringValues> header in headerDictionary)
		//	{
		//		headersHashEntries[headersHashEntriesIndex++] = new HashEntry(header.Key, (string)header.Value);
		//	}

		//	return headersHashEntries;
		//}

		//private CachedResponse CachedResponseFromHashEntryArray(HashEntry[] hashEntries)
		//{
		//	CachedResponse cachedResponse = new CachedResponse();

		//	foreach (HashEntry hashEntry in hashEntries)
		//	{
		//		switch (hashEntry.Name)
		//		{
		//			case nameof(cachedResponse.Created):
		//				cachedResponse.Created = DateTimeOffset.FromUnixTimeMilliseconds((long)hashEntry.Value);
		//				break;
		//			case nameof(cachedResponse.StatusCode):
		//				cachedResponse.StatusCode = (int)hashEntry.Value;
		//				break;
		//			case nameof(cachedResponse.Body):
		//				cachedResponse.Body = new MemoryStream(hashEntry.Value);
		//				break;
		//		}
		//	}

		//	return cachedResponse;
		//}

		//private IHeaderDictionary HeaderDictionaryFromHashEntryArray(HashEntry[] headersHashEntries)
		//{
		//	IHeaderDictionary headerDictionary = new HeaderDictionary();

		//	foreach (HashEntry headersHashEntry in headersHashEntries)
		//	{
		//		headerDictionary.Add(headersHashEntry.Name, (string)headersHashEntry.Value);
		//	}

		//	return headerDictionary;
		//}
	}
}
