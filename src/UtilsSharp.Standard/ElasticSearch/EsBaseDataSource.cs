using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Entity;
using Nest;

namespace ElasticSearch
{
    /// <summary>
    /// Es基础数据源
    /// </summary>
    public abstract class EsBaseDataSource<T>: EsBaseDataAutoMapping<T>, IEsBaseDataSource<T> where T : class, new()
    {
        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IIndexResponse Save(T t, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.Index(t, i => i.Index(execIndex).Refresh(Elasticsearch.Net.Refresh.True));
            return r;
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IBulkResponse Save(List<T> entitys, string index = "")
        {
            if (entitys == null || entitys.Count == 0) return new BulkResponse();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.IndexMany(entitys, execIndex);
            esClient.Refresh(execIndex);
            return r;
        }

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IUpdateResponse<T> IncrementModify(string id, Dictionary<string, object> incrementModifyParams, string index = "")
        {
            IUpdateResponse<T> r = new UpdateResponse<T>();
            if (incrementModifyParams == null) return r;
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var updatePath = new DocumentPath<T>(id);
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            r = esClient.Update<T, object>(updatePath, u => u.Doc(incrementModifyParams).Index(execIndex).Refresh(Elasticsearch.Net.Refresh.True));
            return r;
        }


        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="index">索引</param>
        /// <returns>T</returns>
        public virtual T Get(string id, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var esResult = esClient.Get<T>(id, i => i.Index(execIndex));
            return esResult.Found ? esResult.Source : null;
        }

        /// <summary>
        /// 批量获取数据
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual List<T> Get(string[] ids, string index = "")
        {
            var datas = new List<T>();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var esResult = esClient.MultiGet(m => m.GetMany<T>(ids).Index(execIndex));
            if (esResult.Documents.Count <= 0) return null;
            datas.AddRange(from esResultHits in esResult.Documents where esResultHits.Found select (T)esResultHits.Source);
            return datas;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IBulkResponse Delete(string[] ids, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.Bulk(m => m.DeleteMany<T>(ids).Index(execIndex));
            esClient.Refresh(execIndex);
            return r;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys">参数</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IBulkResponse Delete(List<T> entitys, string index = "")
        {
            if (entitys == null || entitys.Count == 0) return new BulkResponse();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.DeleteMany(entitys, execIndex);
            esClient.Refresh(execIndex);
            return r;
        }

        /// <summary>
        /// 搜索查询
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual ISearchResponse<T> SearchQuery(EsSearchQueryRequest<T> request)
        {
            if (request == null)
            {
                throw new Exception("Parameter cannot be empty");
            }
            if (request.PageIndex == default)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize == default)
            {
                request.PageSize = 10;
            }
            if (request.MustQuerys == null)
            {
                request.MustQuerys = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
            }
            var execIndex = !string.IsNullOrEmpty(request.Index) ? request.Index : AliasIndex;
            var esClient = !string.IsNullOrEmpty(request.Index) ? EsClientByIndex(request.Index) : EsClient;
            var esResult = esClient.Search<T>(s =>
                s.Query(q =>
                        q.Bool(b =>
                            b.Filter(request.MustQuerys)))
                    .Size(request.Size)
                    .From(request.From)
                    .Sort(request.SortSelector)
                    .Source(request.SourceSelector)
                    .Aggregations(request.AggregationsSelector)
                    .Index(execIndex));
            return esResult;
        }

        /// <summary>
        /// 搜索查询(scroll)
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual ISearchResponse<T> SearchScroll(EsSearchScrollRequest<T> request)
        {
            if (request == null)
            {
                throw new Exception("Parameter cannot be empty");
            }
            if (string.IsNullOrEmpty(request.ScrollTime))
            {
                request.ScrollTime = "1m";
            }
            if (request.PageSize == default)
            {
                request.PageSize = 10;
            }
            if (request.MustQuerys == null)
            {
                request.MustQuerys = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
            }
            var execIndex = !string.IsNullOrEmpty(request.Index) ? request.Index : AliasIndex;
            var esClient = !string.IsNullOrEmpty(request.Index) ? EsClientByIndex(request.Index) : EsClient;
            ISearchResponse<T> esResult;
            if (string.IsNullOrEmpty(request.ScrollId))
            {
                esResult = esClient.Search<T>(s =>
                    s.Query(q =>
                            q.Bool(b =>
                                b.Filter(request.MustQuerys)))
                        .Size(request.Size)
                        .Scroll(request.ScrollTime)
                        .Sort(request.SortSelector)
                        .Source(request.SourceSelector)
                        .Aggregations(request.AggregationsSelector)
                        .Index(execIndex));
            }
            else
            {
                esResult = esClient.Scroll<T>(request.ScrollTime, request.ScrollId);
            }
            return esResult;
        }

        /// <summary>
        /// 清除游标
        /// </summary>
        /// <param name="scrollIds">游标id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IClearScrollResponse ClearScroll(string[] scrollIds, string index = "")
        {
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.ClearScroll(c => c.ScrollId(scrollIds));
            return r;
        }
    }
}
