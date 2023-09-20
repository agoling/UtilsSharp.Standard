using NPOI.Util;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UtilsSharp.Office.Entity;

namespace UtilsSharp.Office
{
    /// <summary>
    /// Word帮助类
    /// </summary>
    public class WordHelper
    {
        private XWPFDocument document;

        /// <summary>
        /// WordHelper
        /// </summary>
        public WordHelper()
        {
            document = new XWPFDocument();
        }

        /// <summary>
        /// 添加主标题
        /// </summary>
        /// <param name="text">文本</param>
        public void InsertTitle(string text)
        {
            var request = new WordTextRequest()
            {
                Text = text,
                FontName = "Arial",
                FontSize = 25,
                Color = "000000",
                IsBold = true,
                IsItalic = false,
                Underline = UnderlinePatterns.None
            };
            InsertText(request,0, ParagraphAlignment.CENTER);
        }

        /// <summary>
        /// 添加附标题
        /// </summary>
        /// <param name="text">文本</param>
        public void InsertSubTitle(string text)
        {
            var request = new WordTextRequest()
            {
                Text = text,
                FontName = "Arial",
                FontSize = 15,
                Color = "000000",
                IsBold = true,
                IsItalic = false,
                Underline = UnderlinePatterns.None
            };
            InsertText(request,0, ParagraphAlignment.LEFT);
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="text">文本</param>
        public void InsertContent(string text)
        {
            var request = new WordTextRequest()
            {
                Text = text,
                FontName = "Arial",
                FontSize = 12,
                Color = "000000",
                IsBold = false,
                IsItalic = false,
                Underline = UnderlinePatterns.None
            };
            InsertText(request,500, ParagraphAlignment.LEFT);
        }

        /// <summary>
        /// 添加换行
        /// </summary>
        public void InsertWrap()
        {
            InsertContent("\n");
        }

        /// <summary>
        /// 添加文本内容
        /// </summary>
        /// <param name="request">文本参数</param>
        /// <param name="indentationFirstLine">首行缩进</param>
        /// <param name="alignment">显示类型：居中,居左,居右</param>
        public void InsertText(WordTextRequest request, int indentationFirstLine= 0, ParagraphAlignment alignment= ParagraphAlignment.CENTER)
        {
            var paragraph = document.CreateParagraph();
            // 设置段落的缩进 首行缩进
            paragraph.IndentationFirstLine = indentationFirstLine;
            // 显示类型：居中,居左,居右
            paragraph.Alignment = alignment;

            var run = paragraph.CreateRun();
            run.SetText(request.Text);
            run.FontFamily = request.FontName;
            run.FontSize = request.FontSize;
            run.SetColor(request.Color);
            run.IsBold = request.IsBold;
            run.IsItalic = request.IsItalic;
            run.Underline = request.Underline;
        }

        /// <summary>
        /// 追加文本内容
        /// </summary>
        /// <param name="requests">文本参数</param>
        public void AppendContent(List<WordTextRequest> requests)
        {
            AppendText(requests,500, ParagraphAlignment.LEFT);
        }

        /// <summary>
        /// 追加文本内容
        /// </summary>
        /// <param name="requests">文本参数</param>
        /// <param name="indentationFirstLine">首行缩进</param>
        /// <param name="alignment">显示类型：居中,居左,居右</param>
        public void AppendText(List<WordTextRequest> requests, int indentationFirstLine = 0, ParagraphAlignment alignment = ParagraphAlignment.LEFT)
        {
            var paragraph = document.CreateParagraph();
            // 设置段落的缩进 首行缩进
            paragraph.IndentationFirstLine = indentationFirstLine;
            //居什么显示
            paragraph.Alignment = alignment;

            foreach(var request in requests)
            {
                var run = paragraph.CreateRun();
                run.AppendText(request.Text);
                run.FontFamily = request.FontName;
                run.FontSize = request.FontSize;
                run.SetColor(request.Color);
                run.IsBold = request.IsBold;
                run.IsItalic = request.IsItalic;
                run.Underline = request.Underline;
            }
        }

        /// <summary>
        /// 替换文本信息
        /// </summary>
        /// <param name="oldText">旧文本信息</param>
        /// <param name="newText">新文本信息</param>
        public void ReplaceText(string oldText, string newText)
        {
            foreach (XWPFParagraph paragraph in document.Paragraphs)
            {
                foreach (XWPFRun run in paragraph.Runs)
                {
                    if (run.Text.Contains(oldText))
                    {
                        run.SetText(run.Text.Replace(oldText, newText));
                    }
                }
            }
        }

        /// <summary>
        /// 删除含有文本的段落
        /// </summary>
        /// <param name="text">文本信息</param>
        public void DeleteParagraph(string text)
        {
            foreach (XWPFParagraph paragraph in document.Paragraphs)
            {
                for (int i = 0; i < paragraph.Runs.Count; i++)
                {
                    if (paragraph.Runs[i].Text.Contains(text))
                    {
                        paragraph.RemoveRun(i);
                        i--;
                    }
                }
            }
        }


        /// <summary>
        /// 插入在线图片
        /// </summary>
        /// <param name="imageUrl">图片地址</param>
        /// <param name="description">图片描述</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高</param>
        /// <param name="alignment">图片显示类型：居中,居左,居右</param>
        public void InsertOnlineImage(string imageUrl, string description,int width=400,int height=400, ParagraphAlignment alignment = ParagraphAlignment.CENTER)
        {
            var paragraph = document.CreateParagraph();
            paragraph.Alignment = alignment;
            var run = paragraph.CreateRun();

            byte[] imageBytes;
            // 下载图片并读取为字节数组
            using (WebClient webClient = new WebClient())
            {
                imageBytes = webClient.DownloadData(imageUrl);
            }
            // 向Run对象添加图片
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                run.AddPicture(ms, (int)PictureType.JPEG, description, Units.ToEMU(width), Units.ToEMU(height));
            }
        }


        /// <summary>
        /// 插入本地图片
        /// </summary>
        /// <param name="imagePath">图片文件地址</param>
        /// <param name="description">图片描述</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高</param>
        /// <param name="alignment">图片显示类型：居中,居左,居右</param>
        public void InsertLocalImage(string imagePath,string description, int width = 400, int height = 400, ParagraphAlignment alignment = ParagraphAlignment.CENTER)
        {
            var paragraph = document.CreateParagraph();
            paragraph.Alignment= alignment;
            var run = paragraph.CreateRun();

            // 向Run对象添加图片
            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                run.AddPicture(stream, (int)PictureType.JPEG, description, Units.ToEMU(width), Units.ToEMU(height));
            }
        }


        /// <summary>
        /// 插入表格
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="data">表格数据</param>
        public void InsertTable<T>(List<T> data)
        {
            var table = document.CreateTable(data.Count + 1, typeof(T).GetProperties().Length);

            //添加表头
            var headerRow = table.GetRow(0);
            var propertyNames = typeof(T).GetProperties();
            for (var colIndex = 0; colIndex < propertyNames.Length; colIndex++)
            {
                var columnHeader = headerRow.GetCell(colIndex);
                columnHeader.SetText(propertyNames[colIndex].Name);
            }

            //添加数据行
            for (var rowIndex = 1; rowIndex <= data.Count; rowIndex++)
            {
                var rowData = data[rowIndex - 1];
                var row = table.GetRow(rowIndex);

                for (var colIndex = 0; colIndex < propertyNames.Length; colIndex++)
                {
                    var cell = row.GetCell(colIndex);
                    var propertyValue = propertyNames[colIndex].GetValue(rowData)?.ToString();
                    cell.SetText(propertyValue);
                }
            }

            //添加列宽度
            foreach (var row in table.Rows)
            {
                foreach (var cell in row.GetTableCells())
                {
                    cell.GetCTTc().AddNewTcPr().AddNewTcW().w = "1200"; // Set column width
                }
            }

        }

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                document.Write(fileStream);
            }
        }
    }

}
