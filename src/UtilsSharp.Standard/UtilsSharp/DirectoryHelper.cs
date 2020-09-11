using System;
using System.IO;

namespace UtilsSharp
{
    /// <summary>
    /// 文件夹帮助类
    /// </summary>
    public class DirectoryHelper
    {

        /// <summary>
        /// 获取桌面目录路径
        /// </summary>
        /// <returns></returns>
        public static string GetDesktopDirectory()
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            return deskDir;
        }

        /// <summary>
        /// 得到绝对目录路径
        /// </summary>
        /// <param name="dirPath">目录路径如：dir/{DateTime.Now.Date:yyyy-MM-dd}/ </param>
        /// <returns></returns>
        public static string ConvertToDirectory(string dirPath)
        {
            string dir;
            if (dirPath.Contains(":\\"))
            {
                dir = dirPath;
            }
            else
            {
                dirPath = dirPath.Replace("/", "\\");
                if (!dirPath.StartsWith("\\"))
                {
                    dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dirPath);
                }
                else
                {
                    dirPath = dirPath.Substring(dirPath.IndexOf('\\', 0)).TrimStart('\\');
                    dirPath = dirPath.TrimStart('\\');
                    dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dirPath);
                }   
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
