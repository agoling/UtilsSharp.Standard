using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch.Entity
{
    /// <summary>
    /// Es搜索查询参数
    /// </summary>
    public class EsSearchQueryRequest<T>: EsBaseSearchRequest<T> where T : class, new()
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { set; get; } = 1;
        /// <summary>
        /// From
        /// </summary>
        public int From => PageSize * (PageIndex - 1);
    }
}
