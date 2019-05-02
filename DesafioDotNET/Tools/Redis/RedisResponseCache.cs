using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Primitives;

namespace DesafioDotNET
{
	internal class RedisResponseCache : IResponseCache
	{
		public IResponseCacheEntry Get(string key)
		{
			throw new NotImplementedException();
		}

		public Task<IResponseCacheEntry> GetAsync(string key)
		{
			throw new NotImplementedException();
		}

		public void Set(string key, IResponseCacheEntry entry, TimeSpan validFor)
		{
			throw new NotImplementedException();
		}

		public Task SetAsync(string key, IResponseCacheEntry entry, TimeSpan validFor)
		{
			throw new NotImplementedException();
		}

		private HashEntry[] CachedResponseToHashEntryArray(CachedResponse cachedResponse)
		{
			MemoryStream bodyStream = new MemoryStream();
			cachedResponse.Body.CopyTo(bodyStream);

			return new HashEntry[]
			{
			new HashEntry("Type", nameof(CachedResponse)),
			new HashEntry(nameof(cachedResponse.Created), cachedResponse.Created.ToUnixTimeMilliseconds()),
			new HashEntry(nameof(cachedResponse.StatusCode), cachedResponse.StatusCode),
			new HashEntry(nameof(cachedResponse.Body), bodyStream.ToArray())
			};
		}

		private HashEntry[] HeaderDictionaryToHashEntryArray(IHeaderDictionary headerDictionary)
		{
			HashEntry[] headersHashEntries = new HashEntry[headerDictionary.Count];

			int headersHashEntriesIndex = 0;
			foreach (KeyValuePair<string, StringValues> header in headerDictionary)
			{
				headersHashEntries[headersHashEntriesIndex++] = new HashEntry(header.Key, (string)header.Value);
			}

			return headersHashEntries;
		}
	}
}
