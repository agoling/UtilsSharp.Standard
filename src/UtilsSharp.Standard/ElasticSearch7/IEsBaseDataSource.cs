using System;
using System.Collections.Generic;
using ElasticSearch7.Entity;
using Nest;

namespace ElasticSearch7
{
    /// <summary>
    ///  Es基础数据源
    /// </summary>
    public interface IEsBaseDataSource<T> where T : class, new()
    {
        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        IndexResponse Save(T t, string index = "");

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        BulkResponse Save(List<T> entitys, string index = "");

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        UpdateResponse<T> IncrementModify(string id, Dictionary<string, object> incrementModifyParams, string index = "");

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="index">索引</param>
        /// <returns>T</returns>
        T Get(string id, string index = "");

        /// <summary>
        /// 批量获取数据
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        List<T> Get(string[] ids, string index = "");

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        BulkResponse Delete(string[] ids, string index = "");

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys">参数</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        BulkResponse Delete(List<T> entitys, string index = "");

        /// <summary>
        /// 搜索查询
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        ISearchResponse<T> SearchQuery(EsSearchQueryRequest<T> request);

        /// <summary>
        /// 搜索查询(scroll)
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        ISearchResponse<T> SearchScroll(EsSearchScrollRequest<T> request);

        /// <summary>
        /// 清除游标
        /// </summary>
        /// <param name="scrollIds">游标id集合</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        ClearScrollResponse ClearScroll(string[] scrollIds, string index = "");
    }
}
