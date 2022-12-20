using System;
using System.IO;

namespace UtilsSharp
{
    /// <summary>
    /// 生成快捷方式
    /// </summary>
    public class ShortcutHelper
    {
        /// <summary>
        /// 创建Url桌面快捷方式
        /// </summary>
        /// <param name="linkName">链接名称</param>
        /// <param name="linkUrl">链接地址</param>
        public static void UrlToDesktop(string linkName, string linkUrl)
        {
            if (string.IsNullOrEmpty(linkName))
            {
                throw new Exception("linkName不能为空");
            }
            var deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            using StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url");
            writer.WriteLine("[InternetShortcut]");
            writer.WriteLine("URL=" + linkUrl);
            writer.Flush();
        }

        /// <summary>
        /// 创建应用桌面快捷方式
        /// </summary>
        /// <param name="linkName">链接名称</param>
        public static void AppToDesktop(string linkName)
        {
            if (string.IsNullOrEmpty(linkName))
            {
                throw new Exception("linkName不能为空");
            }
            var deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            using var writer = new StreamWriter(deskDir + "\\" + linkName + ".url");
            var app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            writer.WriteLine("[InternetShortcut]");
            writer.WriteLine("URL=file:///" + app);
            writer.WriteLine("IconIndex=0");
            var icon = app.Replace('\\', '/');
            writer.WriteLine("IconFile=" + icon);
            writer.Flush();
        }
    }
}
