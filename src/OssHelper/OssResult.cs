using System;
using System.Collections.Generic;
using System.Text;

namespace OssHelper
{
    /// <summary>
    /// Oss执行返回参数
    /// </summary>
    public class OssResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { set; get; }
        /// <summary>
        /// 结果
        /// </summary>
        public T Result { set; get; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { set; get; }
    }
}
