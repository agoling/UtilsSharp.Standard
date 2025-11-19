using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Serialization;
using Elastic.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using UtilsSharp.OptionConfig;

namespace UtilsSharp.ElasticSearch9
{
    /// <summary>
    /// Es客户端
    /// </summary>
    public static class EsClientProvider
    {
        /// <summary>
        /// es客户端
        /// </summary>
        internal static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ElasticsearchClient>> ClientDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, ElasticsearchClient>>();

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
        internal static ElasticsearchClient Init(ElasticSearchSetting setting)
        {
            try
            {
                var settings = GetSettings(setting);
                return new ElasticsearchClient(settings);
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
        private static ElasticsearchClientSettings GetSettings(ElasticSearchSetting setting)
        {
            var urls = setting.EsHttpAddress.Split(';').Select(s => new Uri(s));
            //链接池
            var pool = new StaticNodePool(urls);
            var settings = new ElasticsearchClientSettings(
                pool,
                sourceSerializer: (_, currentSettings) => new DefaultSourceSerializer(currentSettings, options =>
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    var stringEnumConverter = options.Converters.FirstOrDefault(c => c is JsonStringEnumConverter);
                    if (stringEnumConverter != null)
                    {
                        options.Converters.Remove(stringEnumConverter);
                    }
                })
                ).DefaultIndex(setting.EsDefaultIndex);
            if (!string.IsNullOrEmpty(setting.UserName) && !string.IsNullOrEmpty(setting.Password))
            {
                settings.Authentication(new BasicAuthentication(setting.UserName, setting.Password));
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
            //是否关闭自动Id推理
            settings.DefaultDisableIdInference(setting.DisableIdInference);
            return settings;
        }
    }
}
