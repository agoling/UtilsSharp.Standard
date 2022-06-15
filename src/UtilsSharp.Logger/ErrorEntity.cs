using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Logger
{
    /// <summary>
    /// 错误日志信息
    /// </summary>
    public class ErrorEntity:BaseLogEntity
    {
        /// <summary>
        /// 错误日志信息
        /// </summary>
        public Exception Ex { set; get; }
    }
}
