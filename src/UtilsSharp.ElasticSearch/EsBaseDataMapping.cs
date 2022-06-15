using System;
using System.Collections.Concurrent;
using Nest;
using UtilsSharp.OptionConfig;

namespace UtilsSharp.ElasticSearch
{
    /// <summary>
    /// Es基础实体
    /// </summary>
    public abstract class EsBaseDataMapping
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EsBaseDataMapping()
        {
            EsClientByIndex();
        }

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object EsBaseDataMappingLock = new object();

        /// <summary>
        /// 表当前配置
        /// </summary>
        private ElasticSearchSetting CurSetting { get; set; }

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
        public abstract void EntityMapping(ElasticClient client, string index);

        /// <summary>
        /// 当前索引
        /// </summary>
        public string CurrentIndex => GetIndex(DateTime.Now);

        /// <summary>
        /// Es客户端
        /// </summary>
        public ElasticClient EsClient => EsClientByIndex();

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="setting">Es配置信息</param>
        /// <returns></returns>
        internal ElasticClient GetClient(ElasticSearchSetting setting)
        {
            if (!EsClientProvider.ClientDictionary.ContainsKey(setting.EsHttpAddress))
            {
                var currentIndexClientDictionary = new ConcurrentDictionary<string, ElasticClient>();
                EsClientProvider.ClientDictionary.TryAdd(setting.EsHttpAddress, currentIndexClientDictionary);
            }
            if (EsClientProvider.ClientDictionary[setting.EsHttpAddress].ContainsKey(setting.EsDefaultIndex))
            {
                return EsClientProvider.ClientDictionary[setting.EsHttpAddress][setting.EsDefaultIndex];
            }
            var client = EsClientProvider.Init(setting);
            EsClientProvider.ClientDictionary[setting.EsHttpAddress].TryAdd(setting.EsDefaultIndex, client);
            return client;
        }


        /// <summary>
        /// Es客户端
        /// </summary>
        /// <param name="index">索引名称</param>
        public ElasticClient EsClientByIndex(string index = "")
        {
            if (CurSetting == null)
            {
                if (Setting == null)
                {
                    if (ElasticSearchConfig.ElasticSearchSetting == null ||
                        ElasticSearchConfig.ElasticSearchSetting.EsHttpAddress == null)
                    {
                        throw new Exception("esHttpAddress cannot be empty");
                    }

                    CurSetting = new ElasticSearchSetting()
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
                    CurSetting = new ElasticSearchSetting()
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
                CurSetting.EsDefaultIndex = index;
                var curClient = GetClient(CurSetting);
                //索引是否已映射
                if (EsClientProvider.MappingDictionary.ContainsKey(CurSetting.EsDefaultIndex))
                {
                    return curClient;
                }
                //判断索引是否存在
                var exists = curClient.IndexExists(CurSetting.EsDefaultIndex).Exists;
                if (!exists) throw new Exception($"Index:{CurSetting.EsDefaultIndex} does not exist");
                RunEntityMapping(curClient, CurSetting.EsDefaultIndex);
                return curClient;
            }
            else
            {
                //程序创建的索引
                CurSetting.EsDefaultIndex = CurrentIndex;
                var curClient = GetClient(CurSetting);
                //索引是否已映射
                if (EsClientProvider.MappingDictionary.ContainsKey(CurSetting.EsDefaultIndex))
                {
                    return curClient;
                }
                var aliasIndex = AliasIndex;
                if (string.IsNullOrEmpty(AliasIndex))
                {
                    aliasIndex = CurSetting.EsDefaultIndex;
                }
                IIndexState indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = 0,
                        NumberOfShards = NumberOfShards
                    }
                };

                //创建索引
                if (!string.IsNullOrEmpty(aliasIndex) && !aliasIndex.Equals(CurSetting.EsDefaultIndex))
                {
                    //绑定别名
                    curClient.CreateIndex(CurSetting.EsDefaultIndex, c => c.InitializeUsing(indexState).Aliases(a => a.Alias(aliasIndex)));
                }
                else
                {
                    curClient.CreateIndex(CurSetting.EsDefaultIndex, c => c.InitializeUsing(indexState));
                }
                RunEntityMapping(curClient, CurSetting.EsDefaultIndex);
                return curClient;
            }
        }


        /// <summary>
        /// 执行实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        private void RunEntityMapping(ElasticClient client, string index)
        {
            lock (EsBaseDataMappingLock)
            {
                EntityMapping(client, index);
                EsClientProvider.MappingDictionary.TryAdd(index, index);
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
