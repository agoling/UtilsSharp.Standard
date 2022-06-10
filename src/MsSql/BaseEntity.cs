using System;
using System.Collections.Generic;
using System.Text;

namespace MsSql
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
