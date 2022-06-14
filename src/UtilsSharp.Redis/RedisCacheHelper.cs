using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSRedis;

namespace UtilsSharp.Redis
{
    /// <summary>
    /// RedisCacheHelper
    /// </summary>
    public abstract class RedisCacheHelper : RedisCacheHelper<RedisCacheHelper> { }

    #region 同步

    /// <summary>
    /// Redis缓存帮助类
    /// </summary>
    public abstract partial class RedisCacheHelper<TMark>
    {
        private static CSRedisClient _instance;

        /// <summary>
        /// CSRedisClient 静态实例，使用前请初始化
        /// RedisHelper.Initialization(new CSRedis.CSRedisClient(\"127.0.0.1:6379,password=123,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍\"))
        /// </summary>
        public static CSRedisClient Instance
        {
            get
            {
                if (_instance == null) throw new Exception("使用前请初始化 RedisHelper.Initialization(new CSRedis.CSRedisClient(\"127.0.0.1:6379,password=123,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍\"));");
                return _instance;
            }
        }

        /// <summary>
        /// 初始化csredis静态访问类
        /// RedisHelper.Initialization(new CSRedis.CSRedisClient(\"127.0.0.1:6379,password=123,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍\"))
        /// </summary>
        /// <param name="csredis"></param>
        public static void Initialization(CSRedisClient csredis)
        {
            _instance = csredis;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireSeconds">缓存时间(秒)</param>
        /// <returns>是否成功</returns>
        public static bool Set(string key, object value, int expireSeconds = -1) => Instance.Set(key, value, expireSeconds);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expire">缓存时间戳</param>
        /// <returns>是否成功</returns>
        public static bool Set(string key, object value, TimeSpan expire) => Instance.Set(key, value, expire);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存keys</param>
        public static long Remove(params string[] keys) => Instance.Del(keys);

        /// <summary>
        /// 移除缓存(pattern方式)
        /// </summary>
        /// <param name="pattern">如：test*</param>
        public static long RemoveByPattern(string pattern)
        {
            var keys = Instance.Keys(pattern);
            var newKeys = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                newKeys.AddRange(from item in keys let index = item.IndexOf(":", StringComparison.Ordinal) select item.Substring(index + 1));
            }
            return Instance.Del(newKeys.ToArray());
        }

        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns >是否存在</returns>
        public static bool IsExists(string key) => Instance.Exists(key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public static string Get(string key) => Instance.Get(key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public static T Get<T>(string key) => Instance.Get<T>(key);

        /// <summary>
        /// 获取缓存(pattern方式)
        /// </summary>
        /// <param name="pattern">如：test*</param>
        /// <returns>缓存数据</returns>
        public static string[] GetByPattern(string pattern)
        {
            var keys = Instance.Keys(pattern);
            var newKeys = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                newKeys.AddRange(from item in keys let index = item.IndexOf(":", StringComparison.Ordinal) select item.Substring(index + 1));
            }
            return newKeys.ToArray();
        }

    }
    #endregion

    #region 异步

    /// <summary>
    /// Redis缓存帮助类
    /// </summary>
    public abstract partial class RedisCacheHelper<TMark>
    {
        
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireSeconds">缓存时间(秒)</param>
        /// <returns>是否成功</returns>
        public static async Task<bool> SetAsync(string key, object value, int expireSeconds = -1) =>await Instance.SetAsync(key, value, expireSeconds);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expire">缓存时间戳</param>
        /// <returns>是否成功</returns>
        public static async Task<bool> SetAsync(string key, object value, TimeSpan expire) =>await Instance.SetAsync(key, value, expire);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存keys</param>
        public static async Task<long> RemoveAsync(params string[] keys) =>await Instance.DelAsync(keys);

        /// <summary>
        /// 移除缓存(pattern方式)
        /// </summary>
        /// <param name="pattern">如：test*</param>
        public static async Task<long> RemoveByPatternAsync(string pattern)
        {
            var keys =await Instance.KeysAsync(pattern);
            var newKeys = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                newKeys.AddRange(from item in keys let index = item.IndexOf(":", StringComparison.Ordinal) select item.Substring(index + 1));
            }
            return await Instance.DelAsync(newKeys.ToArray());
        }

        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns >是否存在</returns>
        public static async Task<bool> IsExistsAsync(string key) =>await Instance.ExistsAsync(key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public static async Task<string> GetAsync(string key) =>await Instance.GetAsync(key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public static async Task<T> GetAsync<T>(string key) =>await Instance.GetAsync<T>(key);

        /// <summary>
        /// 获取缓存(pattern方式)
        /// </summary>
        /// <param name="pattern">如：test*</param>
        /// <returns>缓存数据</returns>
        public static async Task<string[]> GetByPatternAsync(string pattern)
        {
            var keys =await Instance.KeysAsync(pattern);
            var newKeys = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                newKeys.AddRange(from item in keys let index = item.IndexOf(":", StringComparison.Ordinal) select item.Substring(index + 1));
            }
            return newKeys.ToArray();
        }

    }

    #endregion


}
