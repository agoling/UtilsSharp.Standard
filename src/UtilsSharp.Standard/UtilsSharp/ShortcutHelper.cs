using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UtilsSharp.Standard;

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
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + linkUrl);
                writer.Flush();
            }
        }

        /// <summary>
        /// 创建应用桌面快捷方式
        /// </summary>
        /// <param name="linkName">链接名称</param>
        public static void AppToDesktop(string linkName)
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
            {
                string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }
    }
}
