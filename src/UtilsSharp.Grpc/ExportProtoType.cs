using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Grpc
{
    /// <summary>
    /// 导出Proto文件方式
    /// </summary>
    public enum ExportProtoType
    {
        /// <summary>
        /// 全部在一个文件中
        /// </summary>
        AllIn = 1,
        /// <summary>
        /// 按类型名称创建文件
        /// </summary>
        TypeName = 2
    }
}
