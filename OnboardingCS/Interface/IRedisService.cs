using System.Threading.Tasks;

namespace OnboardingCS.Interface
{
    public interface IRedisService
    {
        public Task<bool> DeleteStringAsync(string key);
        public Task<T> GetStringAsync<T>(string key);
        public Task<bool> SaveStringAsync<T>(string key, T value);
    }
}