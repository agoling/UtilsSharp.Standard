using System;
using System.Collections.Generic;
using System.Text;

namespace DapperHelper.MySql
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// 新增默认时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 新增 0
        /// </summary>
        public int Id { get; set; } = 0;
    }
}
