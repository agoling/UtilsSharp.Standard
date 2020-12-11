using System;
using System.Collections.Generic;
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
        /// <returns></returns>
         IndexResponse Save(T t);

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        void Save(List<T> entitys);

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="incrementModifyParams">增量参数：key-字段,value-修改的值</param>
        /// <returns></returns>
        UpdateResponse<T> IncrementModify(string id, Dictionary<string, object> incrementModifyParams);

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T Get(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">Id</param>
        void Delete(string[] ids);
    }
}
