using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Logger
{
    /// <summary>
    /// 日志模型
    /// </summary>
    public class LogEntity: BaseLogEntity
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel Level { set; get; }
        /// <summary>
        /// 异常
        /// </summary>
        public Exception Ex { get; set; }
    }
}
