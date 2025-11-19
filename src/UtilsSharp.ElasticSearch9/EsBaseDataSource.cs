using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsSharp.ElasticSearch9.Entity;

namespace UtilsSharp.ElasticSearch9
{
    #region 同步

    /// <summary>
    /// Es基础数据源
    /// </summary>
    public abstract partial class EsBaseDataSource<T> : EsBaseDataAutoMapping<T>, IEsBaseDataSource<T> where T : class, new()
    {
        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual IndexResponse Save(T t, bool refresh = true, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            return esClient.Index(t, i => i.Index(execIndex).Refresh(refresh ? Refresh.True : Refresh.False));
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entities">参数</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual BulkResponse Save(List<T> entities, bool refresh = true, string index = "")
        {
            if (entities == null || entities.Count == 0) return new BulkResponse();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.IndexMany(entities, execIndex);
            if (refresh)
            {
                esClient.Indices.Refresh(execIndex);
            }
            return r;
        }

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual UpdateResponse<T> IncrementModify(string id, Dictionary<string, object> incrementModifyParams, bool refresh = true, string index = "")
        {
            var r = new UpdateResponse<T>();
            if (incrementModifyParams == null) return r;
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            return esClient.Update<T, object>(execIndex, id, u => u.Doc(incrementModifyParams).Refresh(refresh ? Refresh.True : Refresh.False));
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
            var lists = new List<T>();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var esResult = esClient.MultiGet<T>(m => m.Index(execIndex).Ids(ids));
            if (esResult.Docs.Count <= 0) return null;
            lists.AddRange(from esResultDocs in esResult.Docs where esResultDocs.Value1 != null && esResultDocs.Value1.Found select (T)esResultDocs.Value1.Source);
            return lists;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual BulkResponse Delete(string[] ids, bool refresh = true, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var deleteIds = ids.Select(x => new Id(x));
            var r = esClient.Bulk(b => b
                .DeleteMany(execIndex, deleteIds)
                .Refresh(refresh ? Refresh.True : Refresh.False)
            );
            return r;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">参数</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual BulkResponse Delete(List<T> entities, bool refresh = true, string index = "")
        {
            if (entities == null || entities.Count == 0) return new BulkResponse();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.Bulk(execIndex, b => b
                .Index(execIndex)
                .DeleteMany(entities)
                .Refresh(refresh ? Refresh.True : Refresh.False)
            );
            return r;
        }

        /// <summary>
        /// 搜索查询
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual SearchResponse<T> SearchQuery(EsSearchQueryRequest<T> request)
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
                request.MustQuerys = new List<Action<QueryDescriptor<T>>>();
            }
            var execIndex = !string.IsNullOrEmpty(request.Index) ? request.Index : AliasIndex;
            var esClient = !string.IsNullOrEmpty(request.Index) ? EsClientByIndex(request.Index) : EsClient;
            var esResult = esClient.Search<T>(execIndex, s =>
                s.Query(q => q.Bool(b => b.Filter(request.MustQuerys.ToArray())))
                    .Size(request.Size)
                    .From(request.From)
                    .Sort(request.SortSelector.ToArray())
                    .Source(request.SourceSelector)
                    .Aggregations(request.AggregationsSelector)
                    .IgnoreUnavailable()
                    .TrackTotalHits(true));
            return esResult;
        }

        /// <summary>
        /// 搜索查询(scroll)
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual object SearchScroll(EsSearchScrollRequest<T> request)
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
                request.MustQuerys = new List<Action<QueryDescriptor<T>>>();
            }
            var execIndex = !string.IsNullOrEmpty(request.Index) ? request.Index : AliasIndex;
            var esClient = !string.IsNullOrEmpty(request.Index) ? EsClientByIndex(request.Index) : EsClient;
            object esResult;
            if (string.IsNullOrEmpty(request.ScrollId))
            {
                esResult = esClient.Search<T>(execIndex, s =>
                    s.Query(q => q.Bool(b => b.Filter(request.MustQuerys.ToArray())))
                        .Size(request.Size)
                        .Scroll(request.ScrollTime)
                        .Sort(request.SortSelector.ToArray())
                        .Source(request.SourceSelector)
                        .Aggregations(request.AggregationsSelector)
                        .IgnoreUnavailable()
                        .TrackTotalHits(true));
            }
            else
            {
                esResult = esClient.Scroll<T>(s => s
                    .Scroll(request.ScrollTime)
                    .ScrollId(request.ScrollId)
                );
            }
            return esResult;
        }

        /// <summary>
        /// 清除游标
        /// </summary>
        /// <param name="scrollIds">游标id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual ClearScrollResponse ClearScroll(string[] scrollIds, string index = "")
        {
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = esClient.ClearScroll(c => c.ScrollId(scrollIds));
            return r;
        }

    }

    #endregion

    #region 异步

    /// <summary>
    /// Es基础数据源
    /// </summary>
    public abstract partial class EsBaseDataSource<T> where T : class, new()
    {
        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<IndexResponse> SaveAsync(T t, bool refresh = true, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            return await esClient.IndexAsync(t, i => i.Index(execIndex).Refresh(refresh ? Refresh.True : Refresh.False));
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entities">参数</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<BulkResponse> SaveAsync(List<T> entities, bool refresh = true, string index = "")
        {
            if (entities == null || entities.Count == 0) return new BulkResponse();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = await esClient.IndexManyAsync(entities, execIndex);
            if (refresh)
            {
                await esClient.Indices.RefreshAsync(execIndex);
            }
            return r;
        }

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<UpdateResponse<T>> IncrementModifyAsync(string id, Dictionary<string, object> incrementModifyParams, bool refresh = true, string index = "")
        {
            var r = new UpdateResponse<T>();
            if (incrementModifyParams == null) return r;
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            return await esClient.UpdateAsync<T, object>(execIndex, id, u => u.Doc(incrementModifyParams).Refresh(refresh ? Refresh.True : Refresh.False));
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="index">索引</param>
        /// <returns>T</returns>
        public virtual async Task<T> GetAsync(string id, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var esResult = await esClient.GetAsync<T>(id, i => i.Index(execIndex));
            return esResult.Found ? esResult.Source : null;
        }

        /// <summary>
        /// 批量获取数据
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAsync(string[] ids, string index = "")
        {
            var lists = new List<T>();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var esResult = await esClient.MultiGetAsync<T>(m => m.Index(execIndex).Ids(ids));
            if (esResult.Docs.Count <= 0) return null;
            lists.AddRange(from esResultDocs in esResult.Docs where esResultDocs.Value1 != null && esResultDocs.Value1.Found select (T)esResultDocs.Value1.Source);
            return lists;
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<BulkResponse> DeleteAsync(string[] ids, bool refresh = true, string index = "")
        {
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var deleteIds = ids.Select(x => new Id(x));
            var r = await esClient.BulkAsync(b => b
                .DeleteMany(execIndex, deleteIds)
                .Refresh(refresh ? Refresh.True : Refresh.False)
            );
            return r;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">参数</param>
        /// <param name="refresh">是否刷新索引</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<BulkResponse> DeleteAsync(List<T> entities, bool refresh = true, string index = "")
        {
            if (entities == null || entities.Count == 0) return new BulkResponse();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = await esClient.BulkAsync(execIndex, b => b
                .Index(execIndex)
                .DeleteMany(entities)
                .Refresh(refresh ? Refresh.True : Refresh.False)
            );
            return r;
        }

        /// <summary>
        /// 搜索查询
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual async Task<SearchResponse<T>> SearchQueryAsync(EsSearchQueryRequest<T> request)
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
                request.MustQuerys = new List<Action<QueryDescriptor<T>>>();
            }
            var execIndex = !string.IsNullOrEmpty(request.Index) ? request.Index : AliasIndex;
            var esClient = !string.IsNullOrEmpty(request.Index) ? EsClientByIndex(request.Index) : EsClient;
            var esResult = await esClient.SearchAsync<T>(execIndex, s =>
                s.Query(q => q.Bool(b => b.Filter(request.MustQuerys.ToArray())))
                    .Size(request.Size)
                    .From(request.From)
                    .Sort(request.SortSelector.ToArray())
                    .Source(request.SourceSelector)
                    .Aggregations(request.AggregationsSelector)
                    .IgnoreUnavailable()
                    .TrackTotalHits(true));
            return esResult;
        }

        /// <summary>
        /// 搜索查询(scroll)
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual async Task<object> SearchScrollAsync(EsSearchScrollRequest<T> request)
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
                request.MustQuerys = new List<Action<QueryDescriptor<T>>>();
            }
            var execIndex = !string.IsNullOrEmpty(request.Index) ? request.Index : AliasIndex;
            var esClient = !string.IsNullOrEmpty(request.Index) ? EsClientByIndex(request.Index) : EsClient;
            object esResult;
            if (string.IsNullOrEmpty(request.ScrollId))
            {
                esResult = await esClient.SearchAsync<T>(execIndex, s =>
                    s.Query(q => q.Bool(b => b.Filter(request.MustQuerys.ToArray())))
                        .Size(request.Size)
                        .Scroll(request.ScrollTime)
                        .Sort(request.SortSelector.ToArray())
                        .Source(request.SourceSelector)
                        .Aggregations(request.AggregationsSelector)
                        .IgnoreUnavailable()
                        .TrackTotalHits(true));
            }
            else
            {
                esResult = await esClient.ScrollAsync<T>(s => s
                    .Scroll(request.ScrollTime)
                    .ScrollId(request.ScrollId)
                );
            }
            return esResult;
        }

        /// <summary>
        /// 清除游标
        /// </summary>
        /// <param name="scrollIds">游标id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual async Task<ClearScrollResponse> ClearScrollAsync(string[] scrollIds, string index = "")
        {
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var r = await esClient.ClearScrollAsync(c => c.ScrollId(scrollIds));
            return r;
        }
    }

    #endregion
}
