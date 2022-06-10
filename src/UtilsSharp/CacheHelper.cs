using System;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using UtilsSharp.Standard.Interface;

namespace UtilsSharp
{
    /// <summary>
    /// 服务器缓存帮助类
    /// </summary>
    public class CacheHelper:ICacheManager
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        private static readonly IMemoryCache MemoryCache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 获取当前缓存类型
        /// </summary>
        /// <returns></returns>
        public string GetCacheTypeName()
        {
            return "MemoryCache";
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireSeconds">缓存时间(秒)</param>
        /// <returns>是否成功</returns>
        public bool Set(string key, object value, int expireSeconds = -1)
        {
            if (value == null) return false;
            MemoryCache.Set(key, value, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expireSeconds)));
            return true;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expire">缓存时间戳</param>
        /// <returns>是否成功</returns>
        public bool Set(string key, object value, TimeSpan expire)
        {
            if (value == null) return false;
            MemoryCache.Set(key, value, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expire));
            return true;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存keys</param>
        public long Remove(params string[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return 0;
            }
            foreach (var key in keys)
            {
                MemoryCache.Remove(key);
            }
            return keys.Length;
        }

        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns >是否存在</returns>
        public bool IsExists(string key)
        {
            var obj = MemoryCache.Get(key);
            return obj != null;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public string Get(string key)
        {
            var obj=MemoryCache.Get(key);
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public T Get<T>(string key)
        {
            var obj = MemoryCache.Get(key);
            var str=JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
