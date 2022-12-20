using System;
using UtilsSharp.Shared.Dependency;

namespace UtilsSharp.Shared.Interface
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager:ISingletonDependency
    {
        /// <summary>
        /// 获取当前缓存实例类型名字
        /// </summary>
        /// <returns></returns>
        string GetCacheTypeName();

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireSeconds">缓存时间(秒)</param>
        /// <returns>是否成功</returns>
        bool Set(string key, object value, int expireSeconds = -1);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expire">缓存时间戳</param>
        /// <returns>是否成功</returns>
        bool Set(string key, object value, TimeSpan expire);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存keys</param>
        long Remove(params string[] keys);

        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns >是否存在</returns>
        bool IsExists(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        string Get(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        T Get<T>(string key);
    }
}
