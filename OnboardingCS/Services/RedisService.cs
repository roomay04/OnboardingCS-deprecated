using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnboardingCS.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Services
{
    internal class RedisService : IRedisService
    {
        private readonly ILogger _logger;

        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisService(IConfiguration configuration, ILogger<RedisService> logger)
        {
            string connectionString = configuration.GetConnectionString("RedisConnection");
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
            _logger = logger;
        }

        public ConnectionMultiplexer Connection => _lazyConnection.Value;

        private IDatabase RedisDb => Connection.GetDatabase();

        //TODO: Ini ngapain?
        private List<IServer> GetRedisServers() => Connection.GetEndPoints().Select(endpoint => Connection.GetServer(endpoint)).ToList();

        public async Task<bool> DeleteStringAsync(string key)
        {
            try
            {
                bool isSucceed = await RedisDb.KeyDeleteAsync(key);
                return isSucceed; //TODO Check if key not exists, what's the return?
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<T> GetStringAsync<T>(string key)
        {
            try
            {
                RedisValue stringValue = await RedisDb.StringGetAsync(key);
                if (stringValue.IsNullOrEmpty)
                {
                    return default(T);
                }
                else
                {
                    T objectValue = JsonConvert.DeserializeObject<T>(stringValue);
                    return objectValue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return default(T);
            }
        }

        public async Task<bool> SaveStringAsync<T>(string key, T value)
        {
            try
            {
                RedisValue stringValue = JsonConvert.SerializeObject(value);
                bool isSucceed = await RedisDb.StringSetAsync(key, stringValue);
                return isSucceed;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        //TODO how about other datatypes?


        internal static class RedisKeys
        {
            private const string LABEL_PREFIX = "label";
            private const string LABEL_WITH_TODOS_PREFIX = "labelWithTodos";

            public static string GetLabelWithTodosById(Guid id)
            {
                return $"{LABEL_WITH_TODOS_PREFIX}_labelId:{id}";
            }
        }
    }
}