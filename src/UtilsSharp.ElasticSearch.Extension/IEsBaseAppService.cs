using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using UtilsSharp.ElasticSearch.Extension.Entity;
using UtilsSharp.Shared.Dependency;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp.ElasticSearch.Extension
{
    #region 同步

    /// <summary>
    /// 公共增删查改
    /// </summary>
    public partial interface IEsBaseAppService : IUnitOfWorkDependency
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFunc">保存方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<string>> Save<T1, T2>(T1 request, Func<T2, IIndexResponse> saveFunc) where T1 : EsBaseSaveRequest;

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFunc">保存方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> BulkSave<T1, T2>(List<T1> request, Func<List<T2>, IBulkResponse> saveFunc) where T1 : EsBaseSaveRequest;

        /// <summary>
        /// 批量增量修改
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="incrementModifyFunc">增量修改方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> IncrementModify<T>(EsBaseIncrementModifyRequest request, Func<string, Dictionary<string, object>, IUpdateResponse<T>> incrementModifyFunc) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="deleteFunc">删除方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> Delete(EsBaseIdsRequest request, Func<string[], IBulkResponse> deleteFunc);

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

    #endregion

    #region 异步
    /// <summary>
    /// 公共增删查改
    /// </summary>
    public partial interface IEsBaseAppService
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFuncAsync">保存方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<string>> SaveAsync<T1, T2>(T1 request, Func<T2, Task<IIndexResponse>> saveFuncAsync) where T1 : EsBaseSaveRequest;

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFuncAsync">保存方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> BulkSaveAsync<T1, T2>(List<T1> request, Func<List<T2>, Task<IBulkResponse>> saveFuncAsync) where T1 : EsBaseSaveRequest;


        /// <summary>
        /// 批量增量修改
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="incrementModifyFuncAsync">增量修改方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> IncrementModifyAsync<T>(EsBaseIncrementModifyRequest request, Func<string, Dictionary<string, object>, Task<IUpdateResponse<T>>> incrementModifyFuncAsync) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="deleteFuncAsync">删除方法</param>
        /// <returns></returns>
        ValueTask<BaseResult<int>> DeleteAsync(EsBaseIdsRequest request, Func<string[], Task<IBulkResponse>> deleteFuncAsync);

        ///  <summary>
        /// 单条获取
        ///  </summary>
        ///  <param name="request">参数</param>
        ///  <param name="getFuncAsync">单条获取方法</param>
        ///  <returns></returns>
        ValueTask<BaseResult<T2>> GetAsync<T1, T2>(EsBaseIdRequest request, Func<string, Task<T1>> getFuncAsync) where T1 : class where T2 : EsBaseSaveRequest;


        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="searchFuncAsync">搜索方法</param>
        /// <returns></returns>
        ValueTask<BasePagedResult<T3>> SearchAsync<T1, T2, T3>(T1 request, Func<T1, Task<BasePagedInfoResult<T2>>> searchFuncAsync) where T1 : BasePage where T2 : class where T3 : EsBaseSaveRequest;
    }
    #endregion
}
