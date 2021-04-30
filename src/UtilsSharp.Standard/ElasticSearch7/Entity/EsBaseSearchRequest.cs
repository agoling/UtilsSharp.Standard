using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace ElasticSearch7.Entity
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
        public List<Func<QueryContainerDescriptor<T>, QueryContainer>> MustQuerys { set; get; }=new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
        /// <summary>
        /// 排序
        /// </summary>
        public Func<SortDescriptor<T>, IPromise<IList<ISort>>> SortSelector { set; get; } = s => s;
        /// <summary>
        /// 选取返回的字段
        /// </summary>
        public Func<SourceFilterDescriptor<T>, ISourceFilter> SourceSelector { set; get; } = s => s;
        /// <summary>
        /// 聚合查询
        /// </summary>
        public Func<AggregationContainerDescriptor<T>, IAggregationContainer> AggregationsSelector { set; get; } = s => s;
        /// <summary>
        /// 索引(不传则按别名来查询)
        /// </summary>
        public string Index { set; get; }

    }
}
