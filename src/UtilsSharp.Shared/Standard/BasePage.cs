using UtilsSharp.Shared.Interface;

namespace UtilsSharp.Shared.Standard
{
    /// <summary>
    /// 基础分页参数
    /// </summary>
    public class BasePage: IProtobufEntity
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { set; get; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 10;

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { set; get; }

    }

    /// <summary>
    /// 基础分页参数(含排序参数)
    /// </summary>
    public class BaseSortPage: BasePage
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 排序类型：desc,asc
        /// </summary>
        public string SortType { get; set; }
    }
}
