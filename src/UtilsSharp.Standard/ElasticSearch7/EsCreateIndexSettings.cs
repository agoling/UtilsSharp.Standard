using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7
{
    /// <summary>
    /// 创建索引参数
    /// </summary>
    internal class EsCreateIndexSettings
    {
        /// <summary>
        /// 当前使用索引
        /// </summary>
        public string Index { set; get; }
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
