using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ElasticSearch.Extension.Entity;
using Nest;
using UtilsSharp;
using UtilsSharp.Standard;

namespace ElasticSearch.Extension
{
    /// <summary>
    /// 公共增删查改
    /// </summary>
    public class EsBaseAppService: IEsBaseAppService
    {
        private readonly IMapper _mapper;

        public EsBaseAppService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFunc">保存方法</param>
        /// <returns></returns>
        public ValueTask<BaseResult<string>> Save<T1,T2>(T1 request,Action<bool,T2,BaseResult<string>> saveFunc) where T1:EsBaseSaveRequest
        {
            var result = new BaseResult<string>();
            var vr = ValidationHelper.IsValid(request);
            if (!vr.IsValid)
            {
                result.SetError(vr.ErrorMembers.First().ErrorMessage, BaseStateCode.非法参数);
                return new ValueTask<BaseResult<string>>(result);
            }
            request.UpdateTime = DateTime.Now;
            var isAdd = false;
            if (string.IsNullOrEmpty(request.Id))
            {
                isAdd = true;
                request.Id = Guid.NewGuid().ToString("N");
                request.CreateTime = DateTime.Now;
            }
            result.Result = request.Id;
            var t = _mapper.Map<T2>(request);
            saveFunc(isAdd,t, result);
            return new ValueTask<BaseResult<string>>(result);
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="saveFunc">保存方法</param>
        /// <returns></returns>
        public ValueTask<BaseResult<int>> BulkSave<T1, T2>(List<T1> request, Func<List<T2>, BulkResponse> saveFunc) where T1 : EsBaseSaveRequest
        {
            var result = new BaseResult<int>();

            foreach (var item in request)
            {
                var vr = ValidationHelper.IsValid(item);
                if (!vr.IsValid)
                {
                    result.SetError(vr.ErrorMembers.First().ErrorMessage, BaseStateCode.非法参数);
                    return new ValueTask<BaseResult<int>>(result);
                }
                item.UpdateTime = DateTime.Now;
                if (!string.IsNullOrEmpty(item.Id)) continue;
                item.Id = Guid.NewGuid().ToString("N");
                item.CreateTime = DateTime.Now;
            }
            var t = _mapper.Map<List<T2>>(request);
            var r = saveFunc(t);
            result.Result = r.Items.Count;
            if (r.IsValid)
            {
                result.SetOk("保存成功");
            }
            else
            {
                result.SetError("保存失败");
            }
            return new ValueTask<BaseResult<int>>(result);
        }


        /// <summary>
        /// 批量增量修改
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="incrementModifyFunc">增量修改方法</param>
        /// <returns></returns>
        public ValueTask<BaseResult<int>> IncrementModify<T>(EsBaseIncrementModifyRequest request, Func<string,Dictionary<string, object>, UpdateResponse<T>> incrementModifyFunc) where T: class
        {
            var result = new BaseResult<int>();
            var vr = ValidationHelper.IsValid(request);
            if (!vr.IsValid)
            {
                result.SetError(vr.ErrorMembers.First().ErrorMessage, BaseStateCode.非法参数);
                return new ValueTask<BaseResult<int>>(result);
            }
            var successCount = request.Ids.Distinct().Select(item => incrementModifyFunc(item, request.Data)).Count(r => r.IsValid);
            if (successCount > 0)
            {
                result.SetOkResult(successCount, "操作成功");
            }
            else
            {
                result.SetError("操作失败");
            }
            return new ValueTask<BaseResult<int>>(result);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="deleteFunc">删除方法</param>
        /// <returns></returns>
        public ValueTask<BaseResult<int>> Delete(EsBaseIdsRequest request,Func<string[], BulkResponse> deleteFunc)
        {
            var result = new BaseResult<int>();
            var vr = ValidationHelper.IsValid(request);
            if (!vr.IsValid)
            {
                result.SetError(vr.ErrorMembers.First().ErrorMessage, BaseStateCode.非法参数);
                return new ValueTask<BaseResult<int>>(result);
            }
            var ids = request.Ids.Distinct().ToArray();
            var r = deleteFunc(ids);
            if (r.IsValid)
            {
                result.SetOkResult(ids.Length, "删除成功");
            }
            else
            {
                result.SetError("删除失败");
            }
            return new ValueTask<BaseResult<int>>(result);
        }

        ///  <summary>
        /// 单条获取
        ///  </summary>
        ///  <param name="request">参数</param>
        ///  <param name="getFunc">单条获取方法</param>
        ///  <returns></returns>
        public ValueTask<BaseResult<T2>> Get<T1,T2>(EsBaseIdRequest request,Func<string,T1>getFunc) where T1 : class where T2 : EsBaseSaveRequest
        {
            var result = new BaseResult<T2>();
            var vr = ValidationHelper.IsValid(request);
            if (!vr.IsValid)
            {
                result.SetError(vr.ErrorMembers.First().ErrorMessage, BaseStateCode.非法参数);
                return new ValueTask<BaseResult<T2>>(result);
            }
            var r = getFunc(request.Id);
            if (r == null)
            {
                result.SetError("记录不存在!", BaseStateCode.数据找不到);
                return new ValueTask<BaseResult<T2>>(result);
            }
            var dto = _mapper.Map<T2>(r);
            result.SetOkResult(dto, "请求成功");
            return new ValueTask<BaseResult<T2>>(result);
        }


        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="searchFunc">搜索方法</param>
        /// <returns></returns>
        public ValueTask<BasePagedResult<T3>> Search<T1,T2, T3>(T1 request, Func<T1, BasePagedInfoResult<T2>> searchFunc)where T1:BasePage where T2 : class where T3 : EsBaseSaveRequest
        {
            var result = new BasePagedResult<T3>();
            var r = searchFunc(request);
            var data = _mapper.Map<BasePagedInfoResult<T3>>(r);
            result.SetOkResult(data, "请求成功!");
            return new ValueTask<BasePagedResult<T3>>(result);
        }

    }
}
