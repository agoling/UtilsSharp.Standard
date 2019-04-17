using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCore.Core.Result
{
    /// <summary>
    /// 返回结果模型
    /// </summary>
    /// <typeparam name="T">自定义结果模型</typeparam>
    public class BaseResult<T>: BaseInfoResult
    {
        /// <summary>
        /// 返回对象结果
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 返回分页结果模型
    /// </summary>
    /// <typeparam name="T">自定义结果模型</typeparam>
    public class BasePagedResult<T>: BaseResult<BasePagedInfoResult<T>>
    {
       
    }

    /// <summary>
    /// 基础结果信息
    /// </summary>
    public abstract class BaseInfoResult
    {
        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 错误码:200是操作成功
        /// </summary>
        public Enum ErrorCode { get; set; } = BaseHttpCode.操作成功;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// 设置错误消息
        /// </summary>
        /// <param name="result">基础结果信息</param>
        public void SetError(BaseInfoResult result)
        {
            Success = false;
            Message = result.Message;
            ErrorCode = result.ErrorCode;
        }

        /// <summary>
        /// 设置错误提示
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="code">错误码</param>
        public void SetError(string message, Enum code)
        {
            Success = false;
            Message = message;
            ErrorCode = code;
        }

        /// <summary>
        /// 设置错误提示
        /// </summary>
        /// <param name="code">错误码</param>
        public void SetError(Enum code)
        {
            SetError(code?.ToString(), code);
        }

        /// <summary>
        /// 设置错误消息
        /// </summary>
        /// <param name="message">提示信息</param>
        public void SetError(string message)
        {
            SetError(message, BaseHttpCode.业务提示);
        }
    }

    /// <summary>
    /// 分页基础结果信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePagedInfoResult<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages => TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;

        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage => (PageIndex < TotalPages);

        /// <summary>
        /// 结果信息
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public object Params { get; set; }

    }

    /// <summary>
    /// 基础提示码
    /// </summary>
    public enum BaseHttpCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        操作成功 = 200,

        /// <summary>
        /// 业务提示
        /// </summary>
        业务提示 = 9999
    }
}
