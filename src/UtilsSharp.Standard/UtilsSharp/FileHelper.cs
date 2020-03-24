using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UtilsSharp
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 读文件到byte[]
        /// </summary>
        /// <param name="fileName">硬盘文件路径</param>
        /// <returns></returns>
        public static byte[] ReadFileToBytes(string fileName)
        {
            FileStream pFileStream = null;
            byte[] bytes = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                bytes = r.ReadBytes((int)r.BaseStream.Length);
                return bytes;
            }
            catch
            {
                return bytes;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 读文件到Stream
        /// </summary>
        /// <param name="fileName">硬盘文件路径</param>
        /// <returns></returns>
        public static Stream ReadFileToStream(string fileName)
        {
            byte[] bytes = ReadFileToBytes(fileName);
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 写byte[]到fileName
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <param name="fileName">保存至硬盘路径</param>
        /// <returns></returns>
        public static bool WriteBytesToFile(byte[] bytes, string fileName)
        {
            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(bytes, 0, bytes.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            return true;
        }

        /// <summary>
        /// stream转为byte[]
        /// </summary>
        /// <param name="stream">参数</param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// byte[] 转为stream
        /// </summary>
        /// <param name="bytes">参数</param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }


        /// <summary>
        /// 得到绝对路径
        /// </summary>
        /// <param name="strPath">目录路径如：dir/{DateTime.Now.Date:yyyy-MM-dd}/ </param>
        /// <returns></returns>
        public static string MapPath(string strPath)
        {
            string dir;
            strPath = strPath.Replace("/", "\\");
            if (!strPath.StartsWith("\\"))
            {
                dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
            else
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 0)).TrimStart('\\');
                strPath = strPath.TrimStart('\\');
                dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
