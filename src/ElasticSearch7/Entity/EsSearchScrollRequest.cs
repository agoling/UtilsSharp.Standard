using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace ElasticSearch7.Entity
{
    /// <summary>
    /// Es搜索查询参数(scroll)
    /// </summary>
    public class EsSearchScrollRequest<T>: EsBaseSearchRequest<T> where T : class, new()
    {
        /// <summary>
        /// 游标存活时间
        /// </summary>
        public string ScrollTime { set; get; } = "1m";
        /// <summary>
        /// 游标Id
        /// </summary>
        public string ScrollId { set; get; }
    }
}
