using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aliyun.OSS;

namespace UtilsSharp.OssHelper
{

    /// <summary>
    /// OssHelper
    /// </summary>
    public abstract class OssHelper : OssHelper<OssHelper> { }

    #region 同步
    /// <summary>
    /// Oss帮助类
    /// </summary>
    public abstract partial class OssHelper<TMark>
    {
        private static OssClient _instance;

        /// <summary>
        /// OssClient 静态实例，使用前请初始化
        /// OssHelper.Initialization(new OssClient())
        /// </summary>
        public static OssClient Instance
        {
            get
            {
                if (_instance == null) throw new Exception("使用前请初始化 OssHelper.Initialization(new OssClient());");
                return _instance;
            }
        }

        /// <summary>
        /// 初始化OssClient静态访问类
        /// OssHelper.Initialization(new OssClient())
        /// </summary>
        /// <param name="ossClient"></param>
        public static void Initialization(OssClient ossClient)
        {
            _instance = ossClient;
        }


        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public static List<string> GetObjects(string prefix, string reqOssEndpoint = "")
        {
            return Instance.GetObjects(prefix, reqOssEndpoint);
        }


        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public static OssResult<List<string>> GetObjectsResult(string prefix, string reqOssEndpoint = "")
        {
            return Instance.GetObjectsResult(prefix, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static bool SaveStr(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            return Instance.SaveStr(ossFilePath, content, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static bool SaveFile(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            return Instance.SaveFile(ossFilePath, content, reqOssEndpoint);
        }


        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static OssResult<PutObjectResult> SaveStrResult(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            return Instance.SaveStrResult(ossFilePath, content, reqOssEndpoint);
        }


        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static OssResult<PutObjectResult> SaveFileResult(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            return Instance.SaveFileResult(ossFilePath, content, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static bool SaveFileByUrl(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            return Instance.SaveFileByUrl(ossFilePath, url, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static OssResult<Tuple<PutObjectResult, byte[]>> SaveFileByUrlResult(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            return Instance.SaveFileByUrlResult(ossFilePath, url,  reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static string GetStr(string ossFilePath, string reqOssEndpoint = "")
        {
            return Instance.GetStr(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static byte[] GetObject(string ossFilePath, string reqOssEndpoint = "")
        {
            return Instance.GetObject(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static OssResult<string> GetStrResult(string ossFilePath, string reqOssEndpoint = "")
        {
            return Instance.GetStrResult(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static OssResult<byte[]> GetObjectResult(string ossFilePath, string reqOssEndpoint = "")
        {
            return Instance.GetObjectResult(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static string DeleteObject(string ossFilePath, string reqOssEndpoint = "")
        {
            return Instance.DeleteObject(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static OssResult<string> DeleteObjectResult(string ossFilePath, string reqOssEndpoint = "")
        {
            return Instance.DeleteObjectResult(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取服务端直传签名（policy和callback）
        /// </summary>
        /// <param name="ossCallbackUrl">OSS往这个机器发送的url请求</param>
        /// <param name="ossCallbackHost">OSS发送这个请求时，请求头部所带的Host头</param>
        /// <param name="tips">提示信息</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <param name="ossDir">上传目录 默认："tools/webUpload/"</param>
        /// <param name="fileName">文件名 默认为空</param>
        /// <param name="expireTime">过期时间(默认30秒)</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetSign(string ossCallbackUrl, string ossCallbackHost, out string tips, string reqOssEndpoint = "", string ossDir = "tools/webUpload/", string fileName = "", long expireTime = 30)
        {
            return Instance.GetSign(ossCallbackUrl, ossCallbackHost, out tips, reqOssEndpoint, ossDir, fileName, expireTime);
        }
    }
    #endregion

    #region 异步
    /// <summary>
    /// Oss帮助类
    /// </summary>
    public abstract partial class OssHelper<TMark>
    {
        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public static async Task<List<string>> GetObjectsAsync(string prefix, string reqOssEndpoint = "")
        {
            return await Instance.GetObjectsAsync(prefix, reqOssEndpoint);
        }


        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public static async Task<OssResult<List<string>>> GetObjectsResultAsync(string prefix, string reqOssEndpoint = "")
        {
            return await Instance.GetObjectsResultAsync(prefix, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<bool> SaveStrAsync(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            return await Instance.SaveStrAsync(ossFilePath, content, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<bool> SaveFileAsync(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            return await Instance.SaveFileAsync(ossFilePath, content, reqOssEndpoint);
        }


        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<OssResult<PutObjectResult>> SaveStrResultAsync(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            return await Instance.SaveStrResultAsync(ossFilePath, content, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<OssResult<PutObjectResult>> SaveFileResultAsync(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            return await Instance.SaveFileResultAsync(ossFilePath, content, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<bool> SaveFileByUrlAsync(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            return await Instance.SaveFileByUrlAsync(ossFilePath, url,reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<OssResult<Tuple<PutObjectResult, byte[]>>> SaveFileByUrlResultAsync(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            return await Instance.SaveFileByUrlResultAsync(ossFilePath, url,  reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<string> GetStrAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            return await Instance.GetStrAsync(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<byte[]> GetObjectAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            return await Instance.GetObjectAsync(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<OssResult<string>> GetStrResultAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            return await Instance.GetStrResultAsync(ossFilePath, reqOssEndpoint);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public static async Task<OssResult<byte[]>> GetObjectResultAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            return await Instance.GetObjectResultAsync(ossFilePath, reqOssEndpoint);
        }
    }
    #endregion
}
