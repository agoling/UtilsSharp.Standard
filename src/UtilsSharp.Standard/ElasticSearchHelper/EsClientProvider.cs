using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using Nest;

namespace ElasticSearchHelper.Nest
{
    /// <summary>
    /// Es客户端
    /// </summary>
    public class EsClientProvider
    {
        /// <summary>
        /// es客服端
        /// </summary>
        private static ElasticClient client;
        /// <summary>
        /// Es配置
        /// </summary>
        public static BaseEsConnectionSettings BaseEsConnectionSettings;

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        internal static ElasticClient GetClient()
        {
            if (client == null) Init();
            return client;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            try
            {
                BaseEsConnectionSettings = new BaseEsConnectionSettings
                {
                    EsHttpAddress = ConfigurationManager.AppSettings["EsHttpAddress"],
                    EsDefaultIndex = ConfigurationManager.AppSettings["EsDefaultIndex"],
                    EsNetworkProxy = ConfigurationManager.AppSettings["EsNetworkProxy"],
                    EsConnectionLimit = Convert.ToInt32(ConfigurationManager.AppSettings["EsConnectionLimit"])
                };
                var settings = BaseEsConnectionSettings.GetSettings();
                client = new ElasticClient(settings);
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
            var currClient = GetClient();
            if (string.IsNullOrEmpty(aliasIndex))
            {
                aliasIndex = EsClientProvider.BaseEsConnectionSettings.EsDefaultIndex;
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
