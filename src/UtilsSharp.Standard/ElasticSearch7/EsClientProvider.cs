using System;
using System.Collections.Concurrent;
using Nest;
using OptionConfig;

namespace ElasticSearch7
{
    /// <summary>
    /// Es客户端
    /// </summary>
    public class EsClientProvider
    {
        /// <summary>
        /// es客服端
        /// </summary>
        private static readonly ConcurrentDictionary<string, ElasticClient>  ClientDictionary=new ConcurrentDictionary<string, ElasticClient>();
        /// <summary>
        /// Es配置
        /// </summary>
        internal static BaseEsConnectionSettings BaseEsConnectionSettings;

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="currentIndex">当前索引</param>
        /// <returns></returns>
        internal static ElasticClient GetClient(string currentIndex)
        {
            if (ClientDictionary.ContainsKey(currentIndex))
            {
                return ClientDictionary[currentIndex];
            }
            var client= Init(currentIndex);
            ClientDictionary.TryAdd(currentIndex, client);
            return client;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="currentIndex">当前索引</param>
        public static ElasticClient Init(string currentIndex)
        {
            try
            {
                var defaultIndex = !string.IsNullOrEmpty(currentIndex) ? currentIndex : ElasticSearchConfig.EsDefaultIndex;
                BaseEsConnectionSettings = new BaseEsConnectionSettings
                {
                    EsHttpAddress = ElasticSearchConfig.EsHttpAddress,
                    UserName = ElasticSearchConfig.UserName,
                    Password = ElasticSearchConfig.Password,
                    EsDefaultIndex = defaultIndex,
                    EsNetworkProxy = ElasticSearchConfig.EsNetworkProxy,
                    EsConnectionLimit = ElasticSearchConfig.EsConnectionLimit
                };
                var settings = BaseEsConnectionSettings.GetSettings();
                return new ElasticClient(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"EsClientProvider.Register:{ex.Message}");
            }
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
            var index = esCreateIndexSettings.Index;
            var currClient = GetClient(index);
            if (string.IsNullOrEmpty(aliasIndex))
            {
                aliasIndex = EsClientProvider.BaseEsConnectionSettings.EsDefaultIndex;
            }

            //验证索引是否存在
            if (!currClient.Indices.Exists(index).Exists)
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
                    currClient.Indices.Create(index,c => c.InitializeUsing(indexState).Aliases(a => a.Alias(aliasIndex)));
                }
                else
                {
                    currClient.Indices.Create(index, c => c.InitializeUsing(indexState));
                }
            }
            mappingHandle?.Invoke(currClient, index);
        }
    }
}
