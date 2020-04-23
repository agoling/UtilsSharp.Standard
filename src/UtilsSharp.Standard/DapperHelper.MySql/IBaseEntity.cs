using System;
using System.Collections.Generic;
using System.Text;

namespace DapperHelper.MySql
{
    /// <summary>
    /// 基类
    /// </summary>
    public interface IBaseEntity
    {
        int Id { get; set; }

        DateTime CreateTime { get; set; }
    }
}
