using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Entity
{
    #region 公共模型

    /// <summary>
    /// Markdown消息内容类
    /// </summary>
    public class MarkdownMessage
    {
        /// <summary>
        /// 内容坐标索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 内容类型(文字、图片、链接)
        /// </summary>
        public MarkdownType MarkdownType { get; set; }

        /// <summary>
        /// 内容（注：文字类型的内容中禁止字符["#"、"*"、"["、"]"、"!"]；
        /// 图片类型和链接类型的内容传可访问的http地址即可）
        /// </summary>
        public Text Text { get; set; }

        /// <summary>
        /// 是否换行
        /// </summary>
        public bool IsLineFeed { get; set; }
    }

    /// <summary>
    /// 内容
    /// </summary>
    public class Text
    {
        /// <summary>
        /// 文本或链接显示文字
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 超链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文本类型
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// 文本等级
        /// </summary>
        public TitleType ContentGrade { get; set; }
    }

    /// <summary>
    /// 超链接按钮
    /// </summary>
    public class Btn
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标题对应的超链接
        /// </summary>
        public string ActionUrl { get; set; }
    }

    /// <summary>
    /// 超链接按钮带图片
    /// </summary>
    public class Link
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 超链接
        /// </summary>
        public string MessageUrl { get; set; }

        /// <summary>
        /// 图片超链接
        /// </summary>
        public string PicUrl { get; set; }
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 文本类型
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 默认
        /// </summary>
        默认 = 0,
        /// <summary>
        /// 加粗
        /// </summary>
        加粗 = 1,
        /// <summary>
        /// 斜体
        /// </summary>
        斜体 = 2
    }

    /// <summary>
    /// Markdown消息内容类型
    /// </summary>
    public enum MarkdownType
    {
        /// <summary>
        /// 文本
        /// </summary>
        文本 = 1,
        /// <summary>
        /// 图片
        /// </summary>
        图片 = 2,
        /// <summary>
        /// 链接
        /// </summary>
        链接 = 3,
    }

    /// <summary>
    /// 标题(文本)类型等级
    /// </summary>
    public enum TitleType
    {
        /// <summary>
        /// 默认
        /// </summary>
        默认 = 0,
        /// <summary>
        /// 一级
        /// </summary>
        一级 = 1,
        /// <summary>
        /// 二级
        /// </summary>
        二级 = 2,
        /// <summary>
        /// 三级
        /// </summary>
        三级 = 3,
        /// <summary>
        /// 四级
        /// </summary>
        四级 = 4,
        /// <summary>
        /// 五级
        /// </summary>
        五级 = 5,
        /// <summary>
        /// 六级
        /// </summary>
        六级 = 6,
    }

    #endregion
}
