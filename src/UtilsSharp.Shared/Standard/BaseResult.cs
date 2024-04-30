using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UtilsSharp.Shared.Standard
{
    /// <summary>
    /// 返回结果模型
    /// </summary>
    /// <typeparam name="T">自定义结果模型</typeparam>
    [DataContract]
    public class BaseResult<T>
    {
        /// <summary>
        /// 返回码|标识|说明
        ///<para>200|success|请求成功</para>
        ///999|defaultTips|业务提示
        ///<para>2000|apiError|接口异常</para>
        ///3000|networkError|网络异常
        ///<para>4000|notLogin|未登录</para>
        ///4010|authExpire|授权过期
        ///<para>5000|exception|TryCatch异常错误</para>
        ///6000|dataNotFound|数据找不到
        ///<para>6010|dataNotValid|数据验证不通过</para>
        ///7000|businessError|默认业务性错误
        ///<para>7010|parameterCannotBeEmpty|参数不能为空</para>
        ///7020|invalidParameter|非法参数
        ///<para>8000|dbError|数据库异常</para>
        ///9000|SystemError|系统错误
        /// </summary>
        [DataMember(Order = 1)]
        public int Code { get; set; } = 200;

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember(Order = 2)]
        public string Msg { get; set; } = "请求成功";

        /// <summary>
        /// 返回对象结果
        /// </summary>
        [DataMember(Order = 3)]
        public T Result { get; set; }

        /// <summary>
        /// 执行成功
        /// </summary>
        public void SetOk()
        {
            SetOkResult(default, "");
        }

        #region 设置失败

        /// <summary>
        /// 设置错误提示
        /// </summary>
        /// <param name="msg">提示信息</param>
        public void SetError(string msg)
        {
            SetError(msg, 999);
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
        ///<para>5000|exception|TryCatch异常错误</para>
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
        #endregion

        #region 设置成功
        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="msg">成功提示信息</param>
        public void SetOk(string msg)
        {
            SetOkResult(default, msg);
        }

        /// <summary>
        /// 执行成功并返回结果
        /// </summary>
        /// <param name="result">返回对象结果</param>
        public void SetOkResult(T result)
        {
            SetOkResult(result, "");
        }

        /// <summary>
        /// 执行成功并返回结果
        /// </summary>
        /// <param name="result">返回对象结果</param>
        /// <param name="msg">成功提示信息</param>
        public void SetOkResult(T result, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Msg = msg;
            }
            if (!EqualityComparer<T>.Default.Equals(result, default))
            {
                Result = result;
            }
            Code = 200;
        }
        #endregion

        #region 设置异常
        /// <summary>
        /// 设置异常提示
        /// </summary>
        /// <param name="msg">异常信息</param>
        public void SetException(string msg)
        {
            SetError(msg, 5000);
        }

        /// <summary>
        /// 设置异常提示
        /// </summary>
        /// <param name="ex">Exception</param>
        public void SetException(Exception ex)
        {
            SetError(ex.Message+ex.StackTrace, 5000);
        }

        /// <summary>
        /// 设置异常提示
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="logId">日志Id</param>
        public void SetException(Exception ex,string logId)
        {
            var rules = BaseException.GetDefaultRegexRule();
            var r =ex.Regex(logId, rules);
            SetError(r.Msg, r.Code);
        }


        /// <summary>
        /// 设置异常提示
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="logId">日志Id</param>
        /// <param name="rules">匹配规则</param>
        public void SetException(Exception ex, string logId, List<ExceptionRegexRule> rules)
        {
            var r = ex.Regex(logId, rules);
            SetError(r.Msg, r.Code);
        }
        #endregion
    }

    /// <summary>
    /// 返回分页结果模型
    /// </summary>
    /// <typeparam name="T">自定义结果模型</typeparam>
    [DataContract]
    public class BasePagedResult<T>
    {
        /// <summary>
        /// 返回码|标识|说明
        ///<para>200|success|请求成功</para>
        ///999|defaultTips|业务提示
        ///<para>2000|apiError|接口异常</para>
        ///3000|networkError|网络异常
        ///<para>4000|notLogin|未登录</para>
        ///4010|authExpire|授权过期
        ///<para>5000|exception|TryCatch异常错误</para>
        ///6000|dataNotFound|数据找不到
        ///<para>6010|dataNotValid|数据验证不通过</para>
        ///7000|businessError|默认业务性错误
        ///<para>7010|parameterCannotBeEmpty|参数不能为空</para>
        ///7020|invalidParameter|非法参数
        ///<para>8000|dbError|数据库异常</para>
        ///9000|SystemError|系统错误
        /// </summary>
        [DataMember(Order = 1)]
        public int Code { get; set; } = 200;

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember(Order = 2)]
        public string Msg { get; set; } = "请求成功";

        /// <summary>
        /// 返回对象结果
        /// </summary>
        [DataMember(Order = 3)]
        public BasePagedInfoResult<T> Result { get; set; } = new BasePagedInfoResult<T>();

        #region 设置失败

        /// <summary>
        /// 设置错误提示
        /// </summary>
        /// <param name="msg">提示信息</param>
        public void SetError(string msg)
        {
            SetError(msg, 999);
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
        ///<para>5000|exception|TryCatch异常错误</para>
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
        #endregion

        #region 设置成功
        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="msg">成功提示信息</param>
        public void SetOk(string msg)
        {
            SetOkResult(null, msg);
        }

        /// <summary>
        /// 执行成功并返回结果
        /// </summary>
        /// <param name="result">返回对象结果</param>
        public void SetOkResult(BasePagedInfoResult<T> result)
        {
            SetOkResult(result, "");
        }

        /// <summary>
        /// 执行成功并返回结果
        /// </summary>
        /// <param name="result">返回对象结果</param>
        /// <param name="msg">成功提示信息</param>
        public void SetOkResult(BasePagedInfoResult<T> result, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Msg = msg;
            }
            if (result != null)
            {
                Result = result;
            }
            Code = 200;
        }
        #endregion
        
    }

    /// <summary>
    /// 分页基础结果信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class BasePagedInfoResult<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember(Order = 1)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        [DataMember(Order = 3)]
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
        [DataMember(Order = 4)]
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
        [DataMember(Order = 5)]
        public List<T> List { get; set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        [DataMember(Order = 6)]
        public string Params { get; set; }

    }
}
