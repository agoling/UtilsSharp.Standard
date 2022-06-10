using System;
using System.Collections.Generic;
using System.Text;

namespace MsSql
{
    /// <summary>
    /// 基类
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
