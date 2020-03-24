using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard.Interface
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 获取或设置缓存数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存数据</returns>
        T Get<T>(string key);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="cacheTime">缓存时间（分钟）</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// 验证Key是否已经存在
        /// </summary>
        /// <param name="key">要验证的Key</param>
        /// <returns >是否存在</returns>
        bool IsSet(string key);

        /// <summary>
        /// 移除Key对应的缓存
        /// </summary>
        /// <param name="key">要移除的Key</param>
        void Remove(string key);

        /// <summary>
        /// 批量移除指定规则Key对应的缓存
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取当前缓存实例类型名字
        /// </summary>
        /// <returns></returns>
        string GetCacheTypeName();

    }
}
