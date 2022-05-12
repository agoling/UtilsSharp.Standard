using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using UtilsSharp.Dependency;
using UtilsSharp.ElasticSearch7.Extension.Entity;
using UtilsSharp.Standard;

namespace UtilsSharp.ElasticSearch7.Extension
{
    /// <summary>
    /// 公共增删查改
    /// </summary>
    public interface IEsBaseAppService : IUnitOfWorkDependency
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFunc">保存方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<string>> Save<T1, T2>(T1 request, Action<bool, T2, BaseResult<string>> saveFunc) where T1 : EsBaseSaveRequest;

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFunc">保存方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> BulkSave<T1, T2>(List<T1> request, Func<List<T2>, BulkResponse> saveFunc) where T1 : EsBaseSaveRequest;

        /// <summary>
        /// 批量增量修改
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="incrementModifyFunc">增量修改方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> IncrementModify<T>(EsBaseIncrementModifyRequest request, Func<string, Dictionary<string, object>, UpdateResponse<T>> incrementModifyFunc) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="deleteFunc">删除方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> Delete(EsBaseIdsRequest request, Func<string[], BulkResponse> deleteFunc);

        ///  <summary>
        /// 单条获取
        ///  </summary>
        ///  <param name="request">参数</param>
        ///  <param name="getFunc">单条获取方法</param>
        ///  <returns></returns>
        ValueTask<BaseResult<T2>> Get<T1, T2>(EsBaseIdRequest request, Func<string, T1> getFunc) where T1 : class where T2 : EsBaseSaveRequest;

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="searchFunc">搜索方法</param>
        /// <returns></returns>
        ValueTask<BasePagedResult<T3>> Search<T1, T2, T3>(T1 request, Func<T1, BasePagedInfoResult<T2>> searchFunc) where T1 : BasePage where T2 : class where T3 : EsBaseSaveRequest;
    }
}
