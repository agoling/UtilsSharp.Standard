using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace ElasticSearchHelper.Nest
{
    /// <summary>
    /// Es基础实体
    /// </summary>
    public abstract class EsBaseDataMapping<T> where T : class, new()
    {
        // ReSharper disable once StaticMemberInGenericType
        private static DateTime lastCreateIndexTime = DateTime.MinValue;   //索引最后创建时间

        // ReSharper disable once StaticMemberInGenericType
        private static string currentIndex = string.Empty;                 //当前索引

        /// <summary>
        /// 新索引别名
        /// </summary>
        public abstract string AliasIndex { get; set; }

        /// <summary>
        /// 映射并创建索引类型
        /// </summary>
        public abstract EsMappingType EsMappingType { get; set; }

        /// <summary>
        /// 分片数
        /// </summary>
        public virtual int NumberOfShards => 5;


        /// <summary>
        /// 当前Es客户端
        /// </summary>
        public ElasticClient EsClient
        {
            get
            {
                if (IsCreate) { IndexCreateAndMapping(); }
                return EsClientProvider.GetClient();
            }
        }

        /// <summary>
        /// 获取当前索引
        /// </summary>
        public string CurrentIndex => string.IsNullOrEmpty(currentIndex) ? GetIndex(DateTime.Now) : currentIndex;


        #region 获取指定时间索引
        /// <summary>
        /// 获取指定时间索引
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string GetIndex(DateTime dateTime)
        {
            switch (EsMappingType)
            {
                case EsMappingType.Default:
                    EsClientProvider.Init();
                    return EsClientProvider.BaseEsConnectionSettings.EsDefaultIndex;
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
                    return EsClientProvider.BaseEsConnectionSettings.EsDefaultIndex;
            }
        }
        #endregion

        #region 创建索引条件
        /// <summary>
        /// 根据条件是否创建索引
        /// </summary>
        private bool IsCreate
        {
            get
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
                        return lastCreateIndexTime == DateTime.MinValue;
                    case EsMappingType.New:
                        return lastCreateIndexTime == DateTime.MinValue;
                    case EsMappingType.Hour:
                        return lastCreateIndexTime.Hour < DateTime.Now.Hour;
                    case EsMappingType.Day:
                        return lastCreateIndexTime.Day < DateTime.Now.Day;
                    case EsMappingType.Month:
                        return lastCreateIndexTime.Month < DateTime.Now.Month;
                    case EsMappingType.Year:
                        return lastCreateIndexTime.Year < DateTime.Now.Year;
                    default:
                        return lastCreateIndexTime == DateTime.MinValue;
                }
            }
        }
        #endregion

        #region 创建索引并映射
        /// <summary>
        /// 创建索引
        /// </summary>
        private void IndexCreateAndMapping()
        {
            var esMappingSettings = new EsCreateIndexSettings()
            {
                NumberOfShards = NumberOfShards,
                AliasIndex = AliasIndex,
                Index = CurrentIndex
            };
            EsClientProvider.CreateIndex(esMappingSettings, EntityMapping);
            IndexCreateEnd();
        }

        #region 实体映射
        /// <summary>
        /// 实体映射
        /// </summary>
        /// <param name="client">es客户端</param>
        /// <param name="index">索引名称</param>
        public virtual void EntityMapping(ElasticClient client, string index)
        {
            client.Map<T>(m => m.AutoMap().AllField(a => a.Enabled(false)).Index(index));
        }
        #endregion
        #endregion

        /// <summary>
        /// 创建索引之后操作
        /// </summary>
        private void IndexCreateEnd()
        {
            lastCreateIndexTime = DateTime.Now;
            currentIndex = GetIndex(DateTime.Now);
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
