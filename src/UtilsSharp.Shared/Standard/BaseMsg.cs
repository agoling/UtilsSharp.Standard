using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Shared.Standard
{
    /// <summary>
    /// 提示信息模板
    /// </summary>
    public static class BaseMsg
    {
        /// <summary>
        /// 2000|apiError|接口异常
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgApiError(this string logId)
        {
            return $"接口请求异常,请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 3000|networkError|网络异常
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgNetworkError(this string logId)
        {
            return $"网络异常,请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 4000|notLogin|未登录
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgNotLogin(this string logId)
        {
            return $"登入已过期,请重新登入！错误码：{logId}";
        }

        /// <summary>
        /// 4010|authExpire|授权过期
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgAuthExpire(this string logId)
        {
            return $"授权已过期,请重新授权！错误码：{logId}";
        }

        /// <summary>
        /// 5000|exception|TryCatch异常错误
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgException(this string logId)
        {
            return $"Exception异常错误,请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 6000|dataNotFound|数据找不到
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgDataNotFound(this string logId)
        {
            return $"暂无数据！错误码：{logId}";
        }

        /// <summary>
        /// 6010|dataNotValid|数据验证不通过
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgDataNotValid(this string logId)
        {
            return $"数据验证不通过，请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 7000|businessError|默认业务性异常
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgBusinessError(this string logId)
        {
            return $"业务异常，请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 7010|parameterCannotBeEmpty|参数不能为空
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgParameterCannotBeEmpty(this string logId)
        {
            return $"参数不能为空！错误码：{logId}";
        }

        /// <summary>
        /// 7020|invalidParameter|非法参数
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgInvalidParameter(this string logId)
        {
            return $"非法参数！错误码：{logId}";
        }

        /// <summary>
        /// 8000|dbError|数据库异常
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgDbError(this string logId)
        {
            return $"数据库异常，请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 9000|SystemError|系统错误
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <returns></returns>
        public static string ToMsgSystemError(this string logId)
        {
            return $"系统错误，请稍后再试！错误码：{logId}";
        }

        /// <summary>
        /// 获取提示信息
        /// </summary>
        /// <param name="code">日志Id</param>
        /// <param name="logId"></param>
        /// <returns></returns>
        public static string ToMsg(this int code, string logId)
        {
            switch (code)
            {
                case 2000:
                    return logId.ToMsgApiError();
                case 3000:
                    return logId.ToMsgNetworkError();
                case 4000:
                    return logId.ToMsgNotLogin();
                case 4010:
                    return logId.ToMsgAuthExpire();
                case 5000:
                    return logId.ToMsgException();
                case 6000:
                    return logId.ToMsgDataNotFound();
                case 6010:
                    return logId.ToMsgDataNotValid();
                case 7000:
                    return logId.ToMsgBusinessError();
                case 7010:
                    return logId.ToMsgParameterCannotBeEmpty();
                case 7020:
                    return logId.ToMsgInvalidParameter();
                case 8000:
                    return logId.ToMsgDbError();
                case 9000:
                    return logId.ToMsgSystemError();
                 default:
                   return logId.ToMsgSystemError();
            }
        }
    }
}
