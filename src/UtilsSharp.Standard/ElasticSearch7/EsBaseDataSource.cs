using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch7.Entity;
using Nest;

namespace ElasticSearch7
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
            return refresh ? esClient.Index(t, i => i.Index(execIndex).Refresh(Elasticsearch.Net.Refresh.True)) : esClient.Index(t, i => i.Index(execIndex).Refresh(Elasticsearch.Net.Refresh.False));
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
                esClient.Indices.Refresh();
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
            var updatePath = new DocumentPath<T>(id);
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            return refresh ? esClient.Update<T, object>(updatePath, u => u.Doc(incrementModifyParams).Index(execIndex).Refresh(Elasticsearch.Net.Refresh.True)) : esClient.Update<T, object>(updatePath, u => u.Doc(incrementModifyParams).Index(execIndex).Refresh(Elasticsearch.Net.Refresh.False));
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
            var esResult = esClient.MultiGet(m => m.GetMany<T>(ids).Index(execIndex));
            if (esResult.Hits.Count <= 0) return null;
            lists.AddRange(from esResultHits in esResult.Hits where esResultHits.Found select (T)esResultHits.Source);
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
            var r = esClient.Bulk(m => m.DeleteMany<T>(ids).Index(execIndex));
            if (refresh)
            {
                esClient.Indices.Refresh();
            }
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
            var r = esClient.DeleteMany(entities, execIndex);
            if (refresh)
            {
                esClient.Indices.Refresh();
            }
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
                    .Index(execIndex)
                    .TrackTotalHits(true));
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
                        .Index(execIndex)
                        .TrackTotalHits(true));
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
            return refresh ? await esClient.IndexAsync(t, i => i.Index(execIndex).Refresh(Elasticsearch.Net.Refresh.True)) : await esClient.IndexAsync(t, i => i.Index(execIndex).Refresh(Elasticsearch.Net.Refresh.False));
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
                await esClient.Indices.RefreshAsync();
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
            var updatePath = new DocumentPath<T>(id);
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            return refresh ? await esClient.UpdateAsync<T, object>(updatePath, u => u.Doc(incrementModifyParams).Index(execIndex).Refresh(Elasticsearch.Net.Refresh.True)) : await esClient.UpdateAsync<T, object>(updatePath, u => u.Doc(incrementModifyParams).Index(execIndex).Refresh(Elasticsearch.Net.Refresh.False));
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
            var data = new List<T>();
            var execIndex = !string.IsNullOrEmpty(index) ? index : CurrentIndex;
            var esClient = !string.IsNullOrEmpty(index) ? EsClientByIndex(index) : EsClient;
            var esResult = await esClient.MultiGetAsync(m => m.GetMany<T>(ids).Index(execIndex));
            if (esResult.Hits.Count <= 0) return null;
            data.AddRange(from esResultHits in esResult.Hits where esResultHits.Found select (T)esResultHits.Source);
            return data;
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
            var r = await esClient.BulkAsync(m => m.DeleteMany<T>(ids).Index(execIndex));
            if (refresh)
            {
                await esClient.Indices.RefreshAsync();
            }
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
            var r = await esClient.DeleteManyAsync(entities, execIndex);
            if (refresh)
            {
                await esClient.Indices.RefreshAsync();
            }
            return r;
        }

        /// <summary>
        /// 搜索查询
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual async Task<ISearchResponse<T>> SearchQueryAsync(EsSearchQueryRequest<T> request)
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
            var esResult = await esClient.SearchAsync<T>(s =>
                 s.Query(q =>
                         q.Bool(b =>
                             b.Filter(request.MustQuerys)))
                     .Size(request.Size)
                     .From(request.From)
                     .Sort(request.SortSelector)
                     .Source(request.SourceSelector)
                     .Aggregations(request.AggregationsSelector)
                     .Index(execIndex)
                     .TrackTotalHits(true));
            return esResult;
        }

        /// <summary>
        /// 搜索查询(scroll)
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        public virtual async Task<ISearchResponse<T>> SearchScrollAsync(EsSearchScrollRequest<T> request)
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
                esResult = await esClient.SearchAsync<T>(s =>
                    s.Query(q =>
                            q.Bool(b =>
                                b.Filter(request.MustQuerys)))
                        .Size(request.Size)
                        .Scroll(request.ScrollTime)
                        .Sort(request.SortSelector)
                        .Source(request.SourceSelector)
                        .Aggregations(request.AggregationsSelector)
                        .Index(execIndex)
                        .TrackTotalHits(true));
            }
            else
            {
                esResult = await esClient.ScrollAsync<T>(request.ScrollTime, request.ScrollId);
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
