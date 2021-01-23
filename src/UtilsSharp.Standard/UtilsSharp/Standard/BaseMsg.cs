using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 提示信息模板
    /// </summary>
    public static class BaseMsg
    {
        /// <summary>
        /// 2000|apiError|接口异常
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgApiError(this string errorCode)
        {
            return $"接口请求异常,请稍后再试！错误码：{errorCode}";
        }

        /// <summary>
        /// 3000|networkError|网络异常
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgNetworkError(this string errorCode)
        {
            return $"网络异常,请稍后再试！错误码：{errorCode}";
        }

        /// <summary>
        /// 4000|notLogin|未登录
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgNotLogin(this string errorCode)
        {
            return $"登入已过期,请先登入！错误码：{errorCode}";
        }

        /// <summary>
        /// 4010|authExpire|授权过期
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgAuthExpire(this string errorCode)
        {
            return $"授权已过期,请先授权！错误码：{errorCode}";
        }

        /// <summary>
        /// 5000|exception|TryCatch异常错误
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgException(this string errorCode)
        {
            return $"Exception异常错误,请稍后再试！错误码：{errorCode}";
        }

        /// <summary>
        /// 6000|dataNotFound|数据找不到
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgDataNotFound(this string errorCode)
        {
            return $"暂无数据！错误码：{errorCode}";
        }

        /// <summary>
        /// 6010|dataNotValid|数据验证不通过
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgDataNotValid(this string errorCode)
        {
            return $"数据验证不通过，请稍后再试！错误码：{errorCode}";
        }

        /// <summary>
        /// 7000|businessError|默认业务性异常
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgBusinessError(this string errorCode)
        {
            return $"业务异常，请稍后再试！错误码：{errorCode}";
        }

        /// <summary>
        /// 7010|parameterCannotBeEmpty|参数不能为空
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgParameterCannotBeEmpty(this string errorCode)
        {
            return $"参数不能为空！错误码：{errorCode}";
        }

        /// <summary>
        /// 7020|invalidParameter|非法参数
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgInvalidParameter(this string errorCode)
        {
            return $"非法参数！错误码：{errorCode}";
        }

        /// <summary>
        /// 8000|dbError|数据库异常
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgDbError(this string errorCode)
        {
            return $"数据库异常，请稍后再试！错误码：{errorCode}";
        }

        /// <summary>
        /// 9000|SystemError|系统错误
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static string ToMsgSystemError(this string errorCode)
        {
            return $"系统错误，请稍后再试！错误码：{errorCode}";
        }
    }
}
