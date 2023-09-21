using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UtilsSharp;
using UtilsSharp.Office;
using UtilsSharp.Office.Entity;

namespace TestDemoApp.Office
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class ExcelInit
    {

        public static void All()
        {
            Import();
            Export();
            Word();
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        public static void Import()
        {
            var list = new List<UserInfo>();
            list.Add(new UserInfo() { Name = "小A", Email = "11", Password = "12", Phone = "13", IsHappy = true, Age = 14, BirthTime = DateTime.Now.AddYears(-20) });
            list.Add(new UserInfo() { Name = "小B", Email = "21", Password = "22", Phone = "23", IsHappy = false, Age = 15, BirthTime = DateTime.Now.AddYears(-15) });
            list.Add(new UserInfo() { Name = "小C", Email = "这篇文章主要为大家详细介绍了C#使用NPOI导出Excel类封装", Password = "32", Phone = "33", IsHappy = true, Age = 16, BirthTime = DateTime.Now.AddYears(-30) });

            

            var filePath1 = "d:\\1.xlsx";
            var filePath2 = "d:\\2.xlsx";


            var dt = DataTableHelper.ToDataTable(list);
            //DataTable方式 导入
            ExcelHelper.DataTableToExcel(dt, filePath1);
            //List方式 导入
            ExcelHelper.ListToExcel(list, filePath2);
        }


        /// <summary>
        /// 数据导出
        /// </summary>
        public static void Export()
        {
            var filePath1 = "d:\\1.xlsx";
            var filePath2 = "d:\\2.xlsx";


            var dt = ExcelHelper.ExcelToDataTable(filePath1);
            var list = ExcelHelper.ExcelToList<UserInfo>(filePath2);
        }

        /// <summary>
        /// Word文档生成
        /// </summary>
        public static void Word()
        {
            //表格数据集
            var list = new List<UserInfo>();
            list.Add(new UserInfo() { Name = "小A", Email = "11", Password = "12", Phone = "13", IsHappy = true, Age = 14, BirthTime = DateTime.Now.AddYears(-20) });
            list.Add(new UserInfo() { Name = "小B", Email = "21", Password = "22", Phone = "23", IsHappy = false, Age = 15, BirthTime = DateTime.Now.AddYears(-15) });
            list.Add(new UserInfo() { Name = "小C", Email = "这篇文章主要为大家详细介绍了C#使用NPOI导出Excel类封装", Password = "32", Phone = "33", IsHappy = true, Age = 16, BirthTime = DateTime.Now.AddYears(-30) });
           
            WordHelper wordHelper = new WordHelper();
            //插入大标题
            wordHelper.InsertTitle("我是大标题");
            //插入副标题
            wordHelper.InsertSubTitle("我是附标题");
            //插入内容
            wordHelper.InsertContent("这篇文章主要为大家详细介绍了C#使用NPOI导出Excel类封装，文中示例代码介绍的非常详细，具有一定的参考价值，感兴趣的小伙伴们可以参考一下");
            //插入本地图片
            wordHelper.InsertLocalImage("d:\\21.jpg", "我的图片");
            wordHelper.InsertContent("NPOI是指构建在POI 3.x版本之上的一个程序，NPOI可以在没有安装Office的情况下对Word或Excel文档进行读写操作。 NPOI是一个开源的C#读写Excel、WORD等微软OLE2组件文档的项目。");
            //插入标题
            wordHelper.InsertTable(list);
            //换行
            wordHelper.InsertWrap();
            //插入在线图片
            wordHelper.InsertOnlineImage("https://cbu01.alicdn.com/img/ibank/O1CN01o7Vgv224q1zQmDfyP_!!2209334127441-0-cib.jpg", "我的图片");
            wordHelper.InsertContent("以下代码主要分3部分：通过实体类");
            var request1 = new WordTextRequest()
            {
                Text = "您好",
                FontName = "Arial",
                FontSize = 12,
                Color = "red",
                IsBold = false,
                IsItalic = false,
                Underline = UnderlinePatterns.None
            };
            var request2 = new WordTextRequest()
            {
                Text = "老师",
                FontName = "Arial",
                FontSize = 12,
                Color = "green",
                IsBold = false,
                IsItalic = false,
                Underline = UnderlinePatterns.WavyHeavy
            };

            var wordTextRequests = new List<WordTextRequest>();
            wordTextRequests.Add(request1);
            wordTextRequests.Add(request2);

            //追加方式插入内容，该种方式支持在一段话中加入不同的文字样式
            wordHelper.AppendContent(wordTextRequests);
            //删除含“构建”词语的段落
            wordHelper.DeleteParagraph("构建");
            //把文档中含有“标题”的字替换为“主题0591”
            wordHelper.ReplaceText("标题", "主题0591");
            //保存文档；注意：保存文档的时候，文档如果已经有内容会被覆盖，另外文档保存时，不能被另外程序使用，比如在wps打开等
            wordHelper.Save("d:\\3.doc");

        }
    }


    public class UserInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Description("姓名")]
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Description("Email")]
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        public string Phone { get; set; }
        /// <summary>
        /// 是否开心
        /// </summary>
        [Description("是否开心")]
        public bool IsHappy { get;set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [Description("年龄")]
        public int Age { get; set; }
        /// <summary>
        /// 出生时间
        /// </summary>
        [Description("出生时间")]
        public DateTime BirthTime { set; get; }

    }



}
