using System;
using System.Collections.Generic;
using System.Linq;

namespace Redis
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisCache
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireSeconds">缓存时间(秒)</param>
        /// <returns>是否成功</returns>
        public static bool Set(string key, object value, int expireSeconds = -1)
        {
            return RedisHelper.Set(key, value, expireSeconds);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expire">缓存时间戳</param>
        /// <returns>是否成功</returns>
        public static bool Set(string key, object value, TimeSpan expire)
        {
            return RedisHelper.Set(key, value, expire);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存keys</param>
        public static long Remove(params string[] keys)
        {
            return RedisHelper.Del(keys);
        }

        /// <summary>
        /// 移除缓存(pattern方式)
        /// </summary>
        /// <param name="pattern">如：test*</param>
        public static long RemoveByPattern(string pattern)
        {
            var keys = RedisHelper.Keys(pattern);
            var newKeys = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                newKeys.AddRange(from item in keys let index = item.IndexOf(":", StringComparison.Ordinal) select item.Substring(index + 1));
            }
            return RedisHelper.Del(newKeys.ToArray());
        }

        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns >是否存在</returns>
        public static bool IsExists(string key)
        {
            return RedisHelper.Exists(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public static string Get(string key)
        {
            return RedisHelper.Get(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        public static T Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }

        /// <summary>
        /// 获取缓存(pattern方式)
        /// </summary>
        /// <param name="pattern">如：test*</param>
        /// <returns>缓存数据</returns>
        public static string[] GetByPattern(string pattern)
        {
            var keys = RedisHelper.Keys(pattern);
            var newKeys = new List<string>();
            if (keys != null && keys.Length > 0)
            {
                newKeys.AddRange(from item in keys let index = item.IndexOf(":", StringComparison.Ordinal) select item.Substring(index + 1));
            }
            return newKeys.ToArray();
        }

    }
}
