namespace art_tattoo_be.Infrastructure.Cache;

public interface ICacheService
{
  Task<T?> Get<T>(string key);
  Task Set<T>(string key, T value);
  Task Set<T>(string key, T value, TimeSpan expiration);
  Task Remove(string key);
  Task<bool> Exists(string key);
  Task Clear();
  Task ClearWithPattern(string pattern);
}
