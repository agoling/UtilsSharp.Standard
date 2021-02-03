using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using OptionConfig;

namespace ElasticSearch7
{
    /// <summary>
    /// Es基础实体
    /// </summary>
    public abstract class EsBaseDataMapping<T> where T : class, new()
    {
        /// <summary>
        /// 连接设置
        /// </summary>
        public virtual ElasticSearchSetting Setting { get; set; }

        /// <summary>
        /// 新索引别名
        /// </summary>
        public virtual string AliasIndex { get; set; } = "";

        /// <summary>
        /// 映射并创建索引类型
        /// </summary>
        public virtual EsMappingType EsMappingType { get; set; } = EsMappingType.Default;

        /// <summary>
        /// 分片数
        /// </summary>
        public virtual int NumberOfShards => 5;

        /// <summary>
        /// 当前索引
        /// </summary>
        public string CurrentIndex => GetIndex(DateTime.Now);

        /// <summary>
        /// Es客户端
        /// </summary>
        public ElasticClient EsClient => EsClientByIndex();

        /// <summary>
        /// Es客户端
        /// </summary>
        /// <param name="index">索引名称</param>
        public ElasticClient EsClientByIndex(string index="")
        {
            if (Setting == null)
            {
                if (ElasticSearchConfig.ElasticSearchSetting == null || ElasticSearchConfig.ElasticSearchSetting.EsHttpAddress == null)
                {
                    throw new Exception("esHttpAddress cannot be empty");
                }
                Setting = new ElasticSearchSetting()
                {
                    EsHttpAddress = ElasticSearchConfig.ElasticSearchSetting.EsHttpAddress,
                    UserName = ElasticSearchConfig.ElasticSearchSetting.UserName,
                    Password = ElasticSearchConfig.ElasticSearchSetting.Password,
                    EsDefaultIndex = ElasticSearchConfig.ElasticSearchSetting.EsDefaultIndex,
                    EsNetworkProxy = ElasticSearchConfig.ElasticSearchSetting.EsNetworkProxy,
                    EsConnectionLimit = ElasticSearchConfig.ElasticSearchSetting.EsConnectionLimit
                };
            }
            if (!string.IsNullOrWhiteSpace(index)&&index!=CurrentIndex)
            {
                Setting.EsDefaultIndex = index;
                var currClient = EsClientProvider.GetClient(Setting);
                var exists = currClient.Indices.Exists(index).Exists;
                if (exists) return currClient;
                throw new Exception($"Index:{index} does not exist");
            }
            else
            {
                Setting.EsDefaultIndex = CurrentIndex;
                var currClient = EsClientProvider.GetClient(Setting);
                var exists = currClient.Indices.Exists(CurrentIndex).Exists;
                if (exists) return currClient;
                var esMappingSettings = new EsCreateIndexSettings()
                {
                    Setting = Setting,
                    NumberOfShards = NumberOfShards,
                    AliasIndex = AliasIndex
                };
                EsClientProvider.CreateIndex(esMappingSettings, EntityMapping);
                return currClient;
            }
        }

        /// <summary>
        /// 获取指定时间索引
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public string GetIndex(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(AliasIndex))
            {
                EsMappingType = EsMappingType.Default;
            }
            else if (EsMappingType == EsMappingType.Default)
            {
                EsMappingType = EsMappingType.New;
            }
            switch (EsMappingType)
            {
                case EsMappingType.Default:
                    return ElasticSearchConfig.ElasticSearchSetting?.EsDefaultIndex;
                case EsMappingType.New:
                    return AliasIndex;
                case EsMappingType.Hour:
                    return $"{AliasIndex}_{dateTime:yyyyMMddHH}";
                case EsMappingType.Day:
                    return $"{AliasIndex}_{dateTime:yyyyMMdd}";
                case EsMappingType.Month:
                    return $"{AliasIndex}_{dateTime:yyyyMM}";
                case EsMappingType.Year:
                    return $"{AliasIndex}_{dateTime:yyyy}";
                default:
                    return ElasticSearchConfig.ElasticSearchSetting?.EsDefaultIndex;
            }
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public virtual void EntityMapping(ElasticClient client, string index)
        {
            client.Map<T>(m => m.AutoMap().Index(index));
        }
    }

    /// <summary>
    /// 映射并创建索引类型
    /// </summary>
    public enum EsMappingType
    {
        /// <summary>
        /// 默认索引
        /// </summary>
        Default,
        /// <summary>
        /// 新创建索引
        /// </summary>
        New,
        /// <summary>
        /// 按小时创建索引
        /// </summary>
        Hour,
        /// <summary>
        /// 按天创建索引
        /// </summary>
        Day,
        /// <summary>
        /// 按月创建索引
        /// </summary>
        Month,
        /// <summary>
        /// 按年创建索引
        /// </summary>
        Year,
    }
}
