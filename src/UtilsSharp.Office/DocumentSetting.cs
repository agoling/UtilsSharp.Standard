using NPOI.HSSF.Record.AutoFilter;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Office
{
    /// <summary>
    /// 设置文档
    /// </summary>
    public class DocumentSetting
    {
        /// <summary>
        /// 文档类型，默认为A4纵向
        /// </summary>
        public PaperType PaperType { get; set; } = PaperType.A4_V;
        /// <summary>
        /// 保存地址，包含文件名
        /// </summary>
        public string SavePath { get; set; }
        /// <summary>
        /// 文档标题相关
        /// </summary>
        public ContentItemSetting TitleSetting { get; set; }
        /// <summary>
        /// 文档主要内容
        /// </summary>
        public ContentItemSetting MainContentSetting { get; set; }
    }
}
