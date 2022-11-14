using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;
using UtilsSharp.OptionConfig;

namespace UtilsSharp.ElasticSearch7
{
    /// <summary>
    /// Es客户端
    /// </summary>
    public static class EsClientProvider
    {
        /// <summary>
        /// es客服端
        /// </summary>
        internal static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ElasticClient>> ClientDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, ElasticClient>>();

        /// <summary>
        /// es表结构映射
        /// </summary>
        internal static readonly ConcurrentDictionary<string, string> MappingDictionary = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// es别名索引
        /// </summary>
        internal static readonly ConcurrentDictionary<string, List<string>> AliasIndexBindDictionary = new ConcurrentDictionary<string, List<string>>();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="setting">Es配置信息</param>
        internal static ElasticClient Init(ElasticSearchSetting setting)
        {
            try
            {
                var settings = GetSettings(setting);
                return new ElasticClient(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"EsClientProvider.Register:{ex.Message}");
            }
        }

        /// <summary>
        /// 获取Es链接设置
        /// </summary>
        /// <returns></returns>
        private static ConnectionSettings GetSettings(ElasticSearchSetting setting)
        {
            var urls = setting.EsHttpAddress.Split(';').Select(s => new Uri(s));
            //链接池
            var pool = new StaticConnectionPool(urls);
            var settings = new ConnectionSettings(pool).DefaultIndex(setting.EsDefaultIndex);
            if (!string.IsNullOrEmpty(setting.UserName) && !string.IsNullOrEmpty(setting.Password))
            {
                settings.BasicAuthentication(setting.UserName, setting.Password);
            }
            //网络代理
            if (!string.IsNullOrEmpty(setting.EsNetworkProxy))
            {
                settings.Proxy(new Uri(setting.EsNetworkProxy), "", "");
            }
            //连接数限制
            if (setting.EsConnectionLimit > 0)
            {
                settings.ConnectionLimit(setting.EsConnectionLimit);
            }
            return settings;
        }
    }
}
