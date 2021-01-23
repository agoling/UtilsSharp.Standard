using System;
using System.Collections.Concurrent;
using System.Linq;
using Elasticsearch.Net;
using Nest;
using OptionConfig;

namespace ElasticSearch
{
    /// <summary>
    /// Es客户端
    /// </summary>
    public class EsClientProvider
    {
        /// <summary>
        /// es客服端
        /// </summary>
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ElasticClient>> ClientDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, ElasticClient>>();

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="setting">Es配置信息</param>
        /// <returns></returns>
        internal static ElasticClient GetClient(ElasticSearchSetting setting)
        {
            if (!ClientDictionary.ContainsKey(setting.EsHttpAddress))
            {
                var currentIndexClientDictionary = new ConcurrentDictionary<string, ElasticClient>();
                ClientDictionary.TryAdd(setting.EsHttpAddress, currentIndexClientDictionary);
            }
            if (ClientDictionary[setting.EsHttpAddress].ContainsKey(setting.EsDefaultIndex))
            {
                return ClientDictionary[setting.EsHttpAddress][setting.EsDefaultIndex];
            }
            var client = Init(setting);
            ClientDictionary[setting.EsHttpAddress].TryAdd(setting.EsDefaultIndex, client);
            return client;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="setting">Es配置信息</param>
        public static ElasticClient Init(ElasticSearchSetting setting)
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


        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="esCreateIndexSettings">创建索引配置</param>
        /// <param name="mappingHandle">映射处理</param>
        internal static void CreateIndex(EsCreateIndexSettings esCreateIndexSettings, Action<ElasticClient, string> mappingHandle = null)
        {
            var aliasIndex = esCreateIndexSettings.AliasIndex;
            var numberOfShards = esCreateIndexSettings.NumberOfShards;
            var index = esCreateIndexSettings.Setting.EsDefaultIndex;
            var currClient = GetClient(esCreateIndexSettings.Setting);
            if (string.IsNullOrEmpty(aliasIndex))
            {
                aliasIndex = esCreateIndexSettings.Setting.EsDefaultIndex;
            }

            //验证索引是否存在
            if (!currClient.IndexExists(index).Exists)
            {
                IIndexState indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = 0,
                        NumberOfShards = numberOfShards
                    }
                };
                //按别名创建索引
                if (!string.IsNullOrEmpty(aliasIndex) && !aliasIndex.Equals(index))
                {
                    currClient.CreateIndex(index, c => c.InitializeUsing(indexState).Aliases(a => a.Alias(aliasIndex)));
                }
                else
                {
                    currClient.CreateIndex(index, c => c.InitializeUsing(indexState));
                }
            }
            mappingHandle?.Invoke(currClient, index);
        }
    }
}
