using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 返回结果模型
    /// </summary>
    public class BaseResult : BaseResult<string>
    {

    }

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
        /// 返回码|标识|说明
        ///<para>200|success|请求成功</para>
        ///999|defaultTips|业务提示
        ///<para>2000|apiError|接口异常</para>
        ///3000|networkError|网络异常
        ///<para>4000|notLogin|未登录</para>
        ///4010|authExpire|授权过期
        ///<para>5000|exception|异常错误</para>
        ///6000|dataNotFound|数据找不到
        ///<para>6010|dataNotValid|数据验证不通过</para>
        ///7000|businessError|默认业务性异常
        ///<para>8000|dbError|数据库异常</para>
        ///9000|SystemError|系统错误
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg { get; set; } = "请求成功";

        /// <summary>
        /// 设置错误提示
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
        /// <param name="code">
        /// 返回码|标识|说明
        ///<para>200|success|请求成功</para>
        ///999|defaultTips|业务提示
        ///<para>2000|apiError|接口异常</para>
        ///3000|networkError|网络异常
        ///<para>4000|notLogin|未登录</para>
        ///4010|authExpire|授权过期
        ///<para>5000|exception|异常错误</para>
        ///6000|dataNotFound|数据找不到
        ///<para>6010|dataNotValid|数据验证不通过</para>
        ///7000|businessError|默认业务性异常
        ///<para>8000|dbError|数据库异常</para>
        ///9000|SystemError|系统错误
        /// </param>
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
                var currTotalPages = TotalCount > 0 ? 1 : 0;
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
