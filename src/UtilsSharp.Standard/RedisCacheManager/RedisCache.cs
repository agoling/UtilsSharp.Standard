using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using UtilsSharp.Standard.Interface;

namespace RedisCacheManager
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisCache: ICacheManager
    {
        /// <summary>
        /// 获取当前缓存实例类型名字
        /// </summary>
        /// <returns></returns>
        public string GetCacheTypeName()
        {
            return "redis";
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
            var currentKey = key;
            if (!string.IsNullOrEmpty(RedisManager.RootKey))
            {
                currentKey = $"{RedisManager.RootKey}:{key}";
            }
            return RedisHelper.Set(currentKey, value, expireSeconds);
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
            var currentKey = key;
            if (!string.IsNullOrEmpty(RedisManager.RootKey))
            {
                currentKey = $"{RedisManager.RootKey}:{key}";
            }
            return RedisHelper.Set(currentKey, value, expire);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存keys</param>
        public long Remove(params string[] keys)
        {
            var currentKeys=new List<string>();
            if (!string.IsNullOrEmpty(RedisManager.RootKey))
            {
                foreach (var key in keys)
                {
                    var currentKey = $"{RedisManager.RootKey}:{key}"; ;
                    currentKeys.Add(currentKey);
                }
            }
            else
            {
                currentKeys.AddRange(keys);
            }
            return RedisHelper.Del(currentKeys.ToArray());
        }

        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns >是否存在</returns>
        public bool IsExists(string key)
        {
            var currentKey = key;
            if (!string.IsNullOrEmpty(RedisManager.RootKey))
            {
                currentKey = $"{RedisManager.RootKey}:{key}";
            }
            return RedisHelper.Exists(currentKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public string Get(string key)
        {
            var currentKey = key;
            if (!string.IsNullOrEmpty(RedisManager.RootKey))
            {
                currentKey = $"{RedisManager.RootKey}:{key}";
            }
            return RedisHelper.Get(currentKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public T Get<T>(string key)
        {
            var currentKey = key;
            if (!string.IsNullOrEmpty(RedisManager.RootKey))
            {
                currentKey = $"{RedisManager.RootKey}:{key}";
            }
            return RedisHelper.Get<T>(currentKey);
        }
    }
}
