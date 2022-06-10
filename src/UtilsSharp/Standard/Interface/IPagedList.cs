using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard.Interface
{
    /// <summary>
    ///列表分页接口
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 是否存在上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 是否存在下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
