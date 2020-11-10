using System;
using System.Collections.Generic;

namespace MsSql
{
    /// <summary>
    ///  MsSql基础数据源
    /// </summary>
    public interface IMsSqlBaseDataSource<T>
    {
        /// <summary>
        /// 单条保存
        /// </summary>
        /// <param name="t">参数</param>
        int Save(T t);

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="entitys">参数</param>
        void Save(List<T> entitys);

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T Get(long id);

        /// <summary>
        /// 单条删除
        /// </summary>
        /// <param name="t">模型</param>
        /// <returns></returns>
        int Delete(T t);

        /// <summary>
        /// 获取所有的列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetList();
    }
}
