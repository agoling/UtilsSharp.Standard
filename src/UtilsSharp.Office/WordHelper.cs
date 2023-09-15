using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UtilsSharp.Office
{
    /// <summary>
    /// Word帮助类
    /// </summary>
    public class WordHelper
    {
        /// <summary>
        /// 创建文档
        /// </summary>
        /// <param name="setting"></param>
        public static void ExportDocument(DocumentSetting setting)
        {
            XWPFDocument docx = new XWPFDocument();
            MemoryStream ms = new MemoryStream();

            //设置文档
            docx.Document.body.sectPr = new CT_SectPr();
            CT_SectPr setPr = docx.Document.body.sectPr;
            //获取页面大小
            Tuple<int, int> size = GetPaperSize(setting.PaperType);
            setPr.pgSz.w = (ulong)size.Item1;
            setPr.pgSz.h = (ulong)size.Item2;
            //创建一个段落
            CT_P p = docx.Document.body.AddNewP();
            //段落水平居中
            p.AddNewPPr().AddNewJc().val = ST_Jc.center;
            XWPFParagraph gp = new XWPFParagraph(p, docx);

            XWPFRun gr = gp.CreateRun();
            //创建标题
            if (!string.IsNullOrEmpty(setting.TitleSetting.Title))
            {
                gr.GetCTR().AddNewRPr().AddNewRFonts().ascii = setting.TitleSetting.FontName;
                gr.GetCTR().AddNewRPr().AddNewRFonts().eastAsia = setting.TitleSetting.FontName;
                gr.GetCTR().AddNewRPr().AddNewRFonts().hint = ST_Hint.eastAsia;
                gr.GetCTR().AddNewRPr().AddNewSz().val = (ulong)setting.TitleSetting.FontSize;//2号字体
                gr.GetCTR().AddNewRPr().AddNewSzCs().val = (ulong)setting.TitleSetting.FontSize;
                gr.GetCTR().AddNewRPr().AddNewB().val = setting.TitleSetting.HasBold; //加粗
                gr.GetCTR().AddNewRPr().AddNewColor().val = "black";//字体颜色
                gr.SetText(setting.TitleSetting.Title);
            }

            //创建文档主要内容
            if (!string.IsNullOrEmpty(setting.MainContentSetting.MainContent))
            {
                p = docx.Document.body.AddNewP();
                p.AddNewPPr().AddNewJc().val = ST_Jc.both;
                gp = new XWPFParagraph(p, docx)
                {
                    IndentationFirstLine = 2
                };

                //单倍为默认值（240）不需设置，1.5倍=240X1.5=360，2倍=240X2=480
                p.AddNewPPr().AddNewSpacing().line = "400";//固定20磅
                p.AddNewPPr().AddNewSpacing().lineRule = ST_LineSpacingRule.exact;

                gr = gp.CreateRun();
                CT_RPr rpr = gr.GetCTR().AddNewRPr();
                CT_Fonts rfonts = rpr.AddNewRFonts();
                rfonts.ascii = setting.MainContentSetting.FontName;
                rfonts.eastAsia = setting.MainContentSetting.FontName;
                rpr.AddNewSz().val = (ulong)setting.MainContentSetting.FontSize;//5号字体-21
                rpr.AddNewSzCs().val = (ulong)setting.MainContentSetting.FontSize;
                rpr.AddNewB().val = setting.MainContentSetting.HasBold;

                gr.SetText(setting.MainContentSetting.MainContent);
            }

            //开始写入
            docx.Write(ms);

            using (FileStream fs = new FileStream(setting.SavePath, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
            ms.Close();
        }

        #region 私有方法
        /// <summary>
        /// 获取纸张大小，单位：Twip
        /// <para>换算关系：1英寸=1440缇  1厘米=567缇  1磅=20缇  1像素=15缇</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Tuple<int, int> GetPaperSize(PaperType type)
        {
            Tuple<int, int> res = null;
            switch (type)
            {
                case PaperType.A4_V:
                    res = new Tuple<int, int>(11906, 16838);
                    break;
                case PaperType.A4_H:
                    res = new Tuple<int, int>(16838, 11906);
                    break;

                case PaperType.A5_V:
                    res = new Tuple<int, int>(8390, 11906);
                    break;
                case PaperType.A5_H:
                    res = new Tuple<int, int>(11906, 8390);
                    break;

                case PaperType.A6_V:
                    res = new Tuple<int, int>(5953, 8390);
                    break;
                case PaperType.A6_H:
                    res = new Tuple<int, int>(8390, 5953);
                    break;
            }
            return res;
        }
        #endregion
    }

}
