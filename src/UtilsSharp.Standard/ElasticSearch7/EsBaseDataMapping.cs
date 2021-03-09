using System;
using System.Collections.Concurrent;
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
        /// EsClientProvider
        /// </summary>
        private readonly EsClientProvider _esClientProvider = new EsClientProvider();

        /// <summary>
        /// es表结构映射
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _mappingDictionary = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 表当前配置
        /// </summary>
        private  ElasticSearchSetting CurrSetting { get; set; }

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
        /// 实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public virtual void EntityMapping(ElasticClient client, string index)
        {
            client.Map<T>(m => m.AutoMap().Index(index));
        }

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
        public ElasticClient EsClientByIndex(string index = "")
        {
            if (CurrSetting == null)
            {
                if (Setting == null)
                {
                    if (ElasticSearchConfig.ElasticSearchSetting == null ||
                        ElasticSearchConfig.ElasticSearchSetting.EsHttpAddress == null)
                    {
                        throw new Exception("esHttpAddress cannot be empty");
                    }

                    CurrSetting = new ElasticSearchSetting()
                    {
                        EsHttpAddress = ElasticSearchConfig.ElasticSearchSetting.EsHttpAddress,
                        UserName = ElasticSearchConfig.ElasticSearchSetting.UserName,
                        Password = ElasticSearchConfig.ElasticSearchSetting.Password,
                        EsDefaultIndex = ElasticSearchConfig.ElasticSearchSetting.EsDefaultIndex,
                        EsNetworkProxy = ElasticSearchConfig.ElasticSearchSetting.EsNetworkProxy,
                        EsConnectionLimit = ElasticSearchConfig.ElasticSearchSetting.EsConnectionLimit
                    };
                }
                else
                {
                    CurrSetting = new ElasticSearchSetting()
                    {
                        EsHttpAddress = Setting.EsHttpAddress,
                        UserName = Setting.UserName,
                        Password = Setting.Password,
                        EsDefaultIndex = Setting.EsDefaultIndex,
                        EsNetworkProxy = Setting.EsNetworkProxy,
                        EsConnectionLimit = Setting.EsConnectionLimit
                    };
                }
            }

            if (!string.IsNullOrWhiteSpace(index) && index != CurrentIndex)
            {
                //传参进来的索引
                CurrSetting.EsDefaultIndex = index;
                var currClient = _esClientProvider.GetClient(CurrSetting);
                var exists = currClient.Indices.Exists(CurrSetting.EsDefaultIndex).Exists;
                if (!exists) throw new Exception($"Index:{CurrSetting.EsDefaultIndex} does not exist");
                RunEntityMapping(currClient, CurrSetting.EsDefaultIndex);
                return currClient;
            }
            else
            {
                //程序创建的索引
                CurrSetting.EsDefaultIndex = CurrentIndex;
                var currClient = _esClientProvider.GetClient(CurrSetting);
                var exists = currClient.Indices.Exists(CurrSetting.EsDefaultIndex).Exists;
                if (exists)
                {
                    RunEntityMapping(currClient, CurrSetting.EsDefaultIndex);
                    return currClient;
                }

                var aliasIndex = AliasIndex;
                if (string.IsNullOrEmpty(AliasIndex))
                {
                    aliasIndex = CurrSetting.EsDefaultIndex;
                }

                IIndexState indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = 0,
                        NumberOfShards = NumberOfShards
                    }
                };
                //按别名创建索引
                if (!string.IsNullOrEmpty(aliasIndex) && !aliasIndex.Equals(CurrSetting.EsDefaultIndex))
                {
                    currClient.Indices.Create(CurrSetting.EsDefaultIndex,
                        c => c.InitializeUsing(indexState).Aliases(a => a.Alias(aliasIndex)));
                }
                else
                {
                    currClient.Indices.Create(CurrSetting.EsDefaultIndex, c => c.InitializeUsing(indexState));
                }

                RunEntityMapping(currClient, CurrSetting.EsDefaultIndex, true);
                return currClient;
            }
        }

        /// <summary>
        /// 执行实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        /// <param name="isNew">是否新创建表</param>
        private void RunEntityMapping(ElasticClient client, string index, bool isNew = false)
        {
            if (isNew)
            {
                EntityMapping(client, index);
                return;
            }
            if (_mappingDictionary.ContainsKey(index)) return;
            EntityMapping(client, index);
            _mappingDictionary.TryAdd(index, index);
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
