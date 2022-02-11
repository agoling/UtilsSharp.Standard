using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 基础分页参数
    /// </summary>
    [DataContract]
    public class BasePage
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember(Order = 1)]
        public int PageIndex { set; get; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        [DataMember(Order = 2)]
        public int PageSize { set; get; } = 10;

        /// <summary>
        /// 搜索关键字
        /// </summary>
        [DataMember(Order = 3)]
        public string Keyword { set; get; }

    }

    /// <summary>
    /// 基础分页参数(含排序参数)
    /// </summary>
    [DataContract]
    public class BaseSortPage: BasePage
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember(Order = 1)]
        public string SortField { get; set; }
        /// <summary>
        /// 排序类型：desc,asc
        /// </summary>
        [DataMember(Order = 2)]
        public string SortType { get; set; }
    }
}
