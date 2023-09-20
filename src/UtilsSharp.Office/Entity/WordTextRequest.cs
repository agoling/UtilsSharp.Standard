using NPOI.XWPF.UserModel;

namespace UtilsSharp.Office.Entity
{
    /// <summary>
    /// word文本参数
    /// </summary>
    public class WordTextRequest
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { set; get; }
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName { set; get; } = "Arial";
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { set; get; } = 20;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public string Color { set; get; } = "000000";
        /// <summary>
        /// 是否粗体
        /// </summary>
        public bool IsBold { set; get; } = true;
        /// <summary>
        /// 是否斜体
        /// </summary>
        public bool IsItalic { set; get; } = false;
        /// <summary>
        /// 下划线类型
        /// </summary>
        public UnderlinePatterns Underline { set; get; } = UnderlinePatterns.None;

    }
}
