using System;
using System.Collections.Generic;
using System.Text;
using OptionConfig;

namespace ElasticSearch7
{
    /// <summary>
    /// 创建索引参数
    /// </summary>
    internal class EsCreateIndexSettings
    {
        /// <summary>
        /// ElasticSearch设置
        /// </summary>
        public ElasticSearchSetting Setting { set; get; }
        /// <summary>
        /// 索引别名
        /// </summary>
        public string AliasIndex { set; get; }
        /// <summary>
        /// 分片数
        /// </summary>
        public int NumberOfShards { set; get; } = 5;
    }
}
