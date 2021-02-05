using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace LoggerHelper
{
    /// <summary>
    /// 日志模型
    /// </summary>
    public class LogEntity
    {
        /// <summary>
        /// 日志标识
        /// </summary>
        public string LogId { set; get; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel Level { set; get; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { set; get; }
        /// <summary>
        /// 日志标题信息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 异常
        /// </summary>
        public Exception Ex { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { set; get; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Params { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Func { get; set; }
    }
}
