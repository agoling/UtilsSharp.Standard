using System;
using System.Collections.Generic;
using System.Linq;
using UtilsSharp.Standard.Interface;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 列表分页
    /// </summary>
    /// <typeparam name="T">分页项类型</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = source.Count();
            TotalCount = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="totalCount">总记录数</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; private set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// 是否存在上一页
        /// </summary>
        public bool HasPreviousPage => (PageIndex > 1);

        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool HasNextPage => (PageIndex < TotalPages);
    }
}
