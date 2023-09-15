using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Office
{
    /// <summary>
    /// 文档内容相关
    /// </summary>
    public class ContentItemSetting
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 主要内容
        /// </summary>
        public string MainContent { get; set; }
        /// <summary>
        /// 使用字体
        /// </summary>
        public string FontName { get; set; } = "宋体";
        /// <summary>
        /// 字体大小，默认2号字体
        /// </summary>
        public int FontSize { get; set; } = 44;
        /// <summary>
        /// 是否加粗，默认不加粗
        /// </summary>
        public bool HasBold { get; set; } = false;
    }
}
