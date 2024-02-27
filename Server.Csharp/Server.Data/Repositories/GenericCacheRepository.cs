using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Server.Data.Entities;

namespace Server.Data.Repositories;

public class GenericCacheRepository<TEntity>:IGenericCacheRepository<TEntity> where TEntity : Entity
{
    private readonly IDistributedCache _distributedCache;

    public GenericCacheRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<TEntity?> GetByKeyAsync(string key)
    {
        string itemKey = GetKey(key);
        // try get json value from cache
        string? json = await _distributedCache.GetStringAsync(itemKey);

        // if value is empty return null
        if (string.IsNullOrEmpty(json)) return null;

        // otherwise try to convert
        return JsonConvert.DeserializeObject<TEntity>(json, 
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
    }

    public async Task SetAsync(string key, TEntity entity)
    {
        string itemKey = GetKey(key);
        // serialize object to json
        string json = JsonConvert.SerializeObject(entity);
        // set json value to key
        await _distributedCache.SetStringAsync(itemKey, json);
    }

    public async Task DeleteByKeyAsync(string key)
    {
        string itemKey = GetKey(key);
            
        // try to delete value by key
        await _distributedCache.RemoveAsync(key);
    }

    protected virtual string GetKey(object id) => $"{typeof(TEntity).Name}-{id}";
}