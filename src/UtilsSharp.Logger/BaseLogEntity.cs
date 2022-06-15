using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Logger
{
    /// <summary>
    /// 基础日志信息
    /// </summary>
    public class BaseLogEntity
    {
        /// <summary>
        /// 日志标识
        /// </summary>
        public string LogId { set; get; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { set; get; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Params { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Func { get; set; }
        /// <summary>
        /// 日志标题信息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 日志详情信息
        /// </summary>
        public string DetailTrace { set; get; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { set; get; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }
    }
}
