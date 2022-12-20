using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UtilsSharp.Shared.Enum
{
    /// <summary>
    /// 关系运算符
    /// </summary>
    public enum 关系运算符
    {
        /// <summary>
        /// 大于
        /// </summary>
        [Description(">")]
        大于 = 0,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description(">=")]
        大于等于 = 1,
        /// <summary>
        /// 等于
        /// </summary>
        [Description("=")]
        等于 = 2,
        /// <summary>
        /// 不等于
        /// </summary>
        [Description("!=")]
        不等于 = 3,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("<=")]
        小于等于 = 4,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("<")]
        小于 = 5,
    }

    /// <summary>
    /// 算数运算符
    /// </summary>
    public enum 算术运算符
    {
        /// <summary>
        /// 加
        /// </summary>
        [Description("+")]
        加 =0,
        /// <summary>
        /// 减
        /// </summary>
        [Description("-")]
        减 =1,
        /// <summary>
        /// 乘
        /// </summary>
        [Description("*")]
        乘 =2,
        /// <summary>
        /// 除
        /// </summary>
        [Description("/")]
        除 =3,
        /// <summary>
        /// 求余
        /// </summary>
        [Description("%")]
        求余 =4
    }

}
