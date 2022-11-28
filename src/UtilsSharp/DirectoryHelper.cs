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
        /// 创建文件夹，如果不存在
        /// </summary>
        /// <param name="dirPath">要创建的文件夹路径</param>
        public static void CreateIfNotExists(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// 递归复制文件夹及文件夹/文件
        /// </summary>
        /// <param name="sourcePath"> 源文件夹路径 </param>
        /// <param name="targetPath"> 目的文件夹路径 </param>
        /// <param name="searchPatterns"> 要复制的文件扩展名数组 </param>
        public static void Copy(string sourcePath, string targetPath, string[] searchPatterns = null)
        {
            if (string.IsNullOrWhiteSpace(sourcePath)) return;
            if (string.IsNullOrWhiteSpace(targetPath)) return;

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException("递归复制文件夹时源目录不存在。");
            }
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            if (dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    Copy(dir, targetPath + dir.Substring(dir.LastIndexOf("\\", StringComparison.Ordinal)));
                }
            }
            if (searchPatterns != null && searchPatterns.Length > 0)
            {
                foreach (string searchPattern in searchPatterns)
                {
                    string[] files = Directory.GetFiles(sourcePath, searchPattern);
                    if (files.Length <= 0)
                    {
                        continue;
                    }
                    foreach (string file in files)
                    {
                        File.Copy(file, targetPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)));
                    }
                }
            }
            else
            {
                string[] files = Directory.GetFiles(sourcePath);
                if (files.Length <= 0)
                {
                    return;
                }
                foreach (string file in files)
                {
                    File.Copy(file, targetPath + file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)));
                }
            }
        }

        /// <summary>
        /// 递归删除目录
        /// </summary>
        /// <param name="dirPath"> 目录路径 </param>
        /// <param name="isDeleteRoot"> 是否删除根目录 </param>
        /// <returns> 是否成功 </returns>
        public static bool Delete(string dirPath, bool isDeleteRoot = true)
        {
            if (string.IsNullOrWhiteSpace(dirPath)) return false;
            bool flag = false;
            DirectoryInfo dirPathInfo = new DirectoryInfo(dirPath);
            if (dirPathInfo.Exists)
            {
                //删除目录下所有文件
                foreach (FileInfo fileInfo in dirPathInfo.GetFiles())
                {
                    fileInfo.Delete();
                }
                //递归删除所有子目录
                foreach (DirectoryInfo subDirectory in dirPathInfo.GetDirectories())
                {
                    Delete(subDirectory.FullName);
                }
                //删除目录
                if (isDeleteRoot)
                {
                    dirPathInfo.Attributes = FileAttributes.Normal;
                    dirPathInfo.Delete();
                }
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 设置或取消目录的<see cref="FileAttributes"/>属性。
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="attribute">要设置的目录属性</param>
        /// <param name="isSet">true为设置，false为取消</param>
        public static void SetAttributes(string dirPath, FileAttributes attribute, bool isSet)
        {
            if (string.IsNullOrWhiteSpace(dirPath)) return;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            if (!di.Exists)
            {
                throw new DirectoryNotFoundException("设置目录属性时指定文件夹不存在");
            }
            if (isSet)
            {
                di.Attributes = di.Attributes | attribute;
            }
            else
            {
                di.Attributes = di.Attributes & ~attribute;
            }
        }

        /// <summary>
        /// 获取程序根目录路径
        /// </summary>
        public static string GetRootDirectory()
        {
            return Path.GetDirectoryName(typeof(DirectoryHelper).Assembly.Location);
        }

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
