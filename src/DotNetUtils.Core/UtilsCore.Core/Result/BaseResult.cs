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
    public class BaseResult<T> : BaseInfoResult
    {
        /// <summary>
        /// 返回对象结果
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// 返回结果模型
    /// </summary>
    /// <typeparam name="T">自定义结果模型</typeparam>
    public class BaseEntityResult<T> : BaseInfoResult where T : new()
    {
        /// <summary>
        /// 返回对象结果
        /// </summary>
        public T Result { get; set; } = new T();
    }

    /// <summary>
    /// 返回分页结果模型
    /// </summary>
    /// <typeparam name="T">自定义结果模型</typeparam>
    public class BasePagedResult<T> : BaseEntityResult<BasePagedInfoResult<T>>
    {

    }

    /// <summary>
    /// 基础结果信息
    /// </summary>
    public abstract class BaseInfoResult
    {
        /// <summary>
        /// 返回码	标识	说明
        ///200	success 请求成功
        ///999	defaultError 系统繁忙，此时请开发者稍候再试
        ///3000	nullData 未找到数据
        ///4000	notLogin 未登录
        ///5000	exception 异常
        ///5010	dataIsValid 数据验证不通过
        ///6000	dataExpire 数据过期
        ///7000	businessError 默认业务性异常
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg { get; set; } = "请求成功";

        /// <summary>
        /// 设置错误消息
        /// </summary>
        /// <param name="result">基础结果信息</param>
        public void SetError(BaseInfoResult result)
        {
            Code = result.Code;
            Msg = result.Msg;
        }

        /// <summary>
        /// 设置错误提示
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="code">错误码</param>
        public void SetError(string msg, int code)
        {
            Msg = msg;
            Code = code;
        }

        /// <summary>
        /// 设置错误提示
        /// </summary>
        /// <param name="msg">提示信息</param>
        public void SetError(string msg)
        {
            SetError(msg, 999);
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
        public int PageIndex { get; set; } = 1;

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
        public int TotalPages
        {
            get
            {
                int currTotalPages = TotalCount > 0 ? 1 : 0;
                if (PageSize > 0)
                {
                    currTotalPages = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
                }
                return currTotalPages;
            }
        }

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
}
