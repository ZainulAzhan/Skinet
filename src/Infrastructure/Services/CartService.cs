using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;

public class CartService(IConnectionMultiplexer redis) : ICartService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteCartAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _database.StringGetAsync(key);
        if (data.IsNullOrEmpty)
        {
            return null;
        }
        return JsonSerializer.Deserialize<ShoppingCart>(data!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var creaated = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart));
        if (!creaated)
        {
            return null;
        }
        return await GetCartAsync(cart.Id);
    }
}
