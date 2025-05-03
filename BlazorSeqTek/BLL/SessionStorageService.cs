using System.Text.Json;
using System.Threading.Tasks;

public class SessionStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionStorageService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task SetItemAsync<T>(string key, T value)
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetString(key, JsonSerializer.Serialize(value));
            }
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }

    public Task<T?> GetItemAsync<T>(string key)
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null && session.TryGetValue(key, out var value))
            {
                return Task.FromResult(JsonSerializer.Deserialize<T>(value));
            }
            return Task.FromResult(default(T?));
        }
        catch (Exception ex)
        {
            return Task.FromException<T?>(ex);
        }
    }

    public Task RemoveItemAsync(string key)
    {
        try
        {
            _httpContextAccessor.HttpContext?.Session.Remove(key);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }

    public Task ClearAsync()
    {
        try
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }
}