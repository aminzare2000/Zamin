﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zamin.Extentions.Chaching.Abstractions;

namespace Zamin.Extensions.Caching.InMemory.Services;

public class InMemoryCacheAdapter : ICacheAdapter
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<InMemoryCacheAdapter> _logger;

    public InMemoryCacheAdapter(IMemoryCache memoryCache, ILogger<InMemoryCacheAdapter> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _logger.LogInformation("InMemoryCache Adapter Start working");
    }

    public void Add<TInput>(string key, TInput obj, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        _logger.LogInformation("InMemoryCache Adapter Cache {obj} with key : {key} " +
                               ", with data : {data} " +
                               ", with absoluteExpiration : {absoluteExpiration} " +
                               ", with slidingExpiration : {slidingExpiration}",
                               typeof(TInput),
                               key,
                               JsonSerializer.Serialize(obj),
                               absoluteExpiration.ToString(),
                               slidingExpiration.ToString());

        _memoryCache.Set(key, obj);
    }

    public TOutput Get<TOutput>(string key)
    {
        _logger.LogInformation("InMemoryCache Adapter Try Get Cache with key : {key}" , key);

        var result = _memoryCache.TryGetValue(key, out TOutput resultObject);

        if (result)
            _logger.LogInformation("InMemoryCache Adapter Successful Get Cache with key : {key}", key);
        else
            _logger.LogInformation("InMemoryCache Adapter Failed Get Cache with key : {key}", key);

        return resultObject;
    }

    public void RemoveCache(string key)
    {
        _logger.LogInformation("InMemoryCache Adapter Remove Cache with key : {key}", key);

        _memoryCache.Remove(key);
    }
}