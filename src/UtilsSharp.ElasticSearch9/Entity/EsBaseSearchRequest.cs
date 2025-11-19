using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.Fluent;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections.Generic;

namespace UtilsSharp.ElasticSearch9.Entity
{
    /// <summary>
    /// Es基础查询参数
    /// </summary>
    public class EsBaseSearchRequest<T> where T : class, new()
    {
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 10;
        /// <summary>
        /// Size
        /// </summary>
        public int Size => PageSize;
        /// <summary>
        /// 查询条件
        /// </summary>
        public List<Action<QueryDescriptor<T>>> MustQuerys { set; get; } = new List<Action<QueryDescriptor<T>>>();
        /// <summary>
        /// 排序
        /// </summary>
        public List<Action<SortOptionsDescriptor<T>>> SortSelector { set; get; } = new List<Action<SortOptionsDescriptor<T>>>();
        /// <summary>
        /// 选取返回的字段
        /// </summary>
        public SourceConfig SourceSelector { set; get; }
        /// <summary>
        /// 聚合查询
        /// </summary>
        public Action<FluentDictionaryOfStringAggregation<T>> AggregationsSelector { set; get; }
        /// <summary>
        /// 索引(不传则按别名来查询)
        /// </summary>
        public string Index { set; get; }
    }
}
