using StackExchange.Redis;
using System.Text.Json;

namespace Core.Lib.Infra.Cache;
public class RedisService
{
    private readonly IConnectionMultiplexer _redis;
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        Expiration exp = expiration ?? Expiration.Persist;

        var db = _redis.GetDatabase();

        var json = JsonSerializer.Serialize(value, _serializerOptions);
        await db.StringSetAsync(key, json, exp);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();

        var json = await db.StringGetAsync(key);

        if (json.IsNullOrEmpty) 
            return default;

        return JsonSerializer.Deserialize<T>(json.ToString(), _serializerOptions);
    }

    public async Task RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }

    public async Task<List<T>> GetByPrefixAsync<T>(string prefix)
    {
        List<T> results = [];

        var endpoint = _redis.GetEndPoints().First();
        var server = _redis.GetServer(endpoint);

        var keys = server.Keys(pattern: $"{prefix}*").ToArray();

        if (keys.Length == 0) 
            return results;

        var db = _redis.GetDatabase();
        var values = await db.StringGetAsync(keys);

        foreach (var val in values)
        {
            if (val.HasValue)
            {
                results.Add(JsonSerializer.Deserialize<T>(val.ToString(), _serializerOptions)!);
            }
        }

        return results;
    }
}
