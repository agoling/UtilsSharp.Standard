using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using Newtonsoft.Json;
using OptionConfig;

namespace UtilsSharp.OssHelper
{

    #region 同步
    /// <summary>
    /// OssClient
    /// </summary>
    public partial class OssClient
    {
        /// <summary>
        /// 设置文件
        /// </summary>
        private readonly OssSetting _ossSetting;


        public OssClient(OssSetting ossSetting)
        {
            if (ossSetting == null)
            {
                throw new Exception("ossSetting cannot be null");
            }

            if (string.IsNullOrEmpty(ossSetting.OssAccessKeyId))
            {
                throw new Exception("OssAccessKeyId cannot be null or empty");
            }
            if (string.IsNullOrEmpty(ossSetting.OssAccessKeySecret))
            {
                throw new Exception("OssAccessKeySecret cannot be null or empty");
            }
            if (string.IsNullOrEmpty(ossSetting.OssBucketName))
            {
                throw new Exception("OssBucketName cannot be null or empty");
            }
            _ossSetting = ossSetting;
        }


        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public List<string> GetObjects(string prefix, string reqOssEndpoint = "")
        {
            return GetObjectsResult(prefix, reqOssEndpoint).Result;
        }

        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public OssResult<List<string>> GetObjectsResult(string prefix, string reqOssEndpoint = "")
        {
            var result = new OssResult<List<string>>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            var keys = new List<string>();
            try
            {
                ObjectListing r;
                var nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(_ossSetting.OssBucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100,
                        Prefix = prefix,
                    };
                    r = client.ListObjects(listObjectsRequest);
                    keys.AddRange(r.ObjectSummaries.Select(s => s.Key));
                    nextMarker = r.NextMarker;
                } while (r.IsTruncated);
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
            result.Code = 200;
            result.Result = keys;
            return result;
        }


        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public bool SaveStr(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            var binaryData = Encoding.UTF8.GetBytes(content);
            var requestContent = new MemoryStream(binaryData);
            return SaveFile(ossFilePath, requestContent, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public bool SaveFile(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            var r = SaveFileResult(ossFilePath, content, reqOssEndpoint);
            return r.Code == 200;
        }


        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public OssResult<PutObjectResult> SaveStrResult(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            var binaryData = Encoding.UTF8.GetBytes(content);
            var requestContent = new MemoryStream(binaryData);
            return SaveFileResult(ossFilePath, requestContent, reqOssEndpoint);
        }


        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public OssResult<PutObjectResult> SaveFileResult(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            var result = new OssResult<PutObjectResult>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                var r = client.PutObject(_ossSetting.OssBucketName, ossFilePath, content);
                result.Result = r;
                result.Code = Convert.ToInt32(r.HttpStatusCode);
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
        }

        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public bool SaveFileByUrl(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            var r = SaveFileByUrlResult(ossFilePath, url, reqOssEndpoint);
            r.Result = null;
            return r.Code == 200;
        }
        
        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public OssResult<Tuple<PutObjectResult, byte[]>> SaveFileByUrlResult(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            var result = new OssResult<Tuple<PutObjectResult,byte[]>>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                var wc = new WebClient();
                var bytes = wc.DownloadData(url);
                Stream stream = new MemoryStream(bytes);
                var r = client.PutObject(_ossSetting.OssBucketName, ossFilePath, stream);
                result.Result = new Tuple<PutObjectResult, byte[]>(r, bytes);
                result.Code = Convert.ToInt32(r.HttpStatusCode);
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
        }


        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public string GetStr(string ossFilePath, string reqOssEndpoint = "")
        {
            var bytes = GetObject(ossFilePath, reqOssEndpoint);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public byte[] GetObject(string ossFilePath, string reqOssEndpoint = "")
        {
            return GetObjectResult(ossFilePath, reqOssEndpoint).Result;
        }


        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public OssResult<string> GetStrResult(string ossFilePath, string reqOssEndpoint = "")
        {
            var result = new OssResult<string>();
            var r = GetObjectResult(ossFilePath, reqOssEndpoint);
            result.Code = r.Code;
            result.Msg = r.Msg;
            result.Result = Encoding.UTF8.GetString(r.Result);
            return result;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public OssResult<byte[]> GetObjectResult(string ossFilePath, string reqOssEndpoint = "")
        {
            var result = new OssResult<byte[]>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                OssObject file;
                if (ossFilePath.Contains("?"))
                {
                    var array = ossFilePath.Split('?');
                    ossFilePath = array[0];
                    var process = array[1].ToLower().Replace("x-oss-process=", "");
                    file = client.GetObject(new GetObjectRequest(_ossSetting.OssBucketName, ossFilePath, process));
                }
                else
                {
                    file = client.GetObject(_ossSetting.OssBucketName, ossFilePath);
                }
                var ms = new MemoryStream();
                using (var requestStream = file.Content)
                {
                    byte[] buf = new byte[1024];
                    var len = 0;
                    while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                    {
                        ms.Write(buf, 0, len);
                    }
                }
                ms.Close();
                result.Code = Convert.ToInt32(file.HttpStatusCode);
                result.Result = ms.ToArray();
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Result = new byte[] { };
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public string DeleteObject(string ossFilePath, string reqOssEndpoint = "")
        {
            return DeleteObjectResult(ossFilePath, reqOssEndpoint).Msg;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public OssResult<string> DeleteObjectResult(string ossFilePath, string reqOssEndpoint = "")
        {
            var result = new OssResult<string>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                client.DeleteObject(_ossSetting.OssBucketName, ossFilePath);
                result.Code = 200;
                result.Msg = "删除成功";
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"删除失败：Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }

        }


        /// <summary>
        /// 获取服务端直传签名（policy和callback）
        /// </summary>
        /// <param name="ossCallbackUrl">OSS往这个机器发送的url请求</param>
        /// <param name="ossCallbackHost">OSS发送这个请求时，请求头部所带的Host头</param>
        /// <param name="tips">提示信息</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <param name="ossDir">上传目录 默认："tools/webUpload/"</param>
        /// <param name="expireTime">过期时间(默认30秒)</param>
        /// <returns></returns>
        public Dictionary<string, string> GetSign(string ossCallbackUrl, string ossCallbackHost, out string tips, string reqOssEndpoint = "", string ossDir = "tools/webUpload/", long expireTime = 30)
        {
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            string host;
            if (string.IsNullOrEmpty(_ossSetting.OssHost))
            {
                host = _ossSetting.OssProtocol + _ossSetting.OssBucketName + "." + reqOssEndpoint;
            }
            else if (_ossSetting.OssHost.Contains("http://") || _ossSetting.OssHost.Contains("https://"))
            {
                host = _ossSetting.OssHost;
            }
            else
            {
                host = _ossSetting.OssProtocol + _ossSetting.OssHost;
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                var expiration = DateTime.Now.AddMilliseconds(expireTime * 1000);
                var policyConds = new PolicyConditions();
                policyConds.AddConditionItem(PolicyConditions.CondContentLengthRange, 0L, 1048576000L);
                policyConds.AddConditionItem(MatchMode.StartWith, PolicyConditions.CondKey, ossDir);
                var postPolicy = client.GeneratePostPolicy(expiration, policyConds);
                var binaryData = Encoding.UTF8.GetBytes(postPolicy);
                var encodedPolicy = Convert.ToBase64String(binaryData);
                var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(_ossSetting.OssAccessKeySecret));
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedPolicy));
                var postSignature = Convert.ToBase64String(hashBytes);
                var ts = expiration.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var randomFileName = "oss" + Guid.NewGuid().ToString().Replace("-", "");

                var signDic = new Dictionary<string, string>
                {
                    {"accessid", _ossSetting.OssAccessKeyId},
                    {"host", host},
                    {"policy", encodedPolicy},
                    {"signature", postSignature},
                    {"expire", Convert.ToInt64(ts.TotalSeconds).ToString()},
                    {"dir", ossDir},
                    {"filename",randomFileName}
                };
                var callback = JsonConvert.SerializeObject(new
                {
                    callbackUrl = ossCallbackUrl,
                    callbackHost = ossCallbackHost,
                    callbackBody = "filename=${object}&size=${size}&mimeType=${mimeType}&height=${imageInfo.height}&width=${imageInfo.width}",  //OSS请求时，发送给应用服务器的内容，可以包括文件的名字、大小、类型，如果是图片可以是图片的高度、宽度
                    callbackBodyType = "application/x-www-form-urlencoded"   //请求发送的Content-Type
                });
                callback = Convert.ToBase64String(Encoding.UTF8.GetBytes(callback));
                signDic.Add("callback", callback);
                tips = "获取成功";
                return signDic;
            }
            catch (Exception ex)
            {
                tips = ex.Message;
                return null;
            }
        }
    }
    #endregion

    #region 异步

    /// <summary>
    /// OssClient(异步)
    /// </summary>
    public partial class OssClient
    {

        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public async Task<List<string>> GetObjectsAsync(string prefix, string reqOssEndpoint = "")
        {
            var r = await GetObjectsResultAsync(prefix, reqOssEndpoint);
            return r.Result;
        }


        /// <summary>
        /// 通过指定目录路径列举该目录下的所有文件
        /// </summary>
        /// <param name="prefix">文件目录路径</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        public async Task<OssResult<List<string>>> GetObjectsResultAsync(string prefix, string reqOssEndpoint = "")
        {
            var result = new OssResult<List<string>>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            var keys = new List<string>();
            try
            {
                ObjectListing r;
                var nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(_ossSetting.OssBucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100,
                        Prefix = prefix,
                    };
                    r = await client.ListObjectsAsync(listObjectsRequest);
                    keys.AddRange(r.ObjectSummaries.Select(s => s.Key));
                    nextMarker = r.NextMarker;
                } while (r.IsTruncated);
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
            result.Code = 200;
            result.Result = keys;
            return result;
        }

        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<bool> SaveStrAsync(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            var binaryData = Encoding.UTF8.GetBytes(content);
            var requestContent = new MemoryStream(binaryData);
            return await SaveFileAsync(ossFilePath, requestContent, reqOssEndpoint);
        }

        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<bool> SaveFileAsync(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            var r = await SaveFileResultAsync(ossFilePath, content, reqOssEndpoint);
            return r.Code == 200;
        }


        /// <summary>
        /// 保存文件内容到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxx.jpg</param>
        /// <param name="content">文件内容</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<OssResult<PutObjectResult>> SaveStrResultAsync(string ossFilePath, string content, string reqOssEndpoint = "")
        {
            var binaryData = Encoding.UTF8.GetBytes(content);
            var requestContent = new MemoryStream(binaryData);
            return await SaveFileResultAsync(ossFilePath, requestContent, reqOssEndpoint);
        }


        /// <summary>
        /// 保存文件流到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="content">文件流</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<OssResult<PutObjectResult>> SaveFileResultAsync(string ossFilePath, Stream content, string reqOssEndpoint = "")
        {
            var result = new OssResult<PutObjectResult>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                var r = await client.PutObjectAsync(_ossSetting.OssBucketName, ossFilePath, content);
                result.Result = r;
                result.Code = Convert.ToInt32(r.HttpStatusCode);
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
        }


        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<bool> SaveFileByUrlAsync(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            var r = await SaveFileByUrlResultAsync(ossFilePath, url, reqOssEndpoint);
            r.Result = null;
            return r.Code == 200;
        }


        /// <summary>
        /// 保存文件到阿里对象存储(oss)
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="url">要上传的文件地址</param>
        /// <param name="bytes">bytes信息</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<OssResult<Tuple<PutObjectResult, byte[]>>> SaveFileByUrlResultAsync(string ossFilePath, string url, string reqOssEndpoint = "")
        {
            var result = new OssResult<Tuple<PutObjectResult, byte[]>>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                var wc = new WebClient();
                var bytes = wc.DownloadData(url);
                Stream stream = new MemoryStream(bytes);
                var r = await client.PutObjectAsync(_ossSetting.OssBucketName, ossFilePath, stream);
                result.Result = new Tuple<PutObjectResult, byte[]>(r, bytes);
                result.Code = Convert.ToInt32(r.HttpStatusCode);
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
        }


        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<string> GetStrAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            var bytes = await GetObjectAsync(ossFilePath, reqOssEndpoint);
            return Encoding.UTF8.GetString(bytes);
        }


        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<byte[]> GetObjectAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            var r = await GetObjectResultAsync(ossFilePath, reqOssEndpoint);
            return r.Result;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<OssResult<string>> GetStrResultAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            var result = new OssResult<string>();
            var r = await GetObjectResultAsync(ossFilePath, reqOssEndpoint);
            result.Code = r.Code;
            result.Msg = r.Msg;
            result.Result = Encoding.UTF8.GetString(r.Result);
            return result;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ossFilePath">阿里对象存储(oss)服务器文件路径如：tools/2017-03-24/xxxxxx.jpg</param>
        /// <param name="reqOssEndpoint">ossEndpoint(默认访问内网)</param>
        /// <returns></returns>
        public async Task<OssResult<byte[]>> GetObjectResultAsync(string ossFilePath, string reqOssEndpoint = "")
        {
            var result = new OssResult<byte[]>();
            if (_ossSetting == null)
            {
                throw new Exception("请先配置OssSetting");
            }
            if (string.IsNullOrEmpty(reqOssEndpoint))
            {
                reqOssEndpoint = _ossSetting.OssEndpointIn;//默认内网
            }
            var client = new Aliyun.OSS.OssClient(reqOssEndpoint, _ossSetting.OssAccessKeyId, _ossSetting.OssAccessKeySecret);
            try
            {
                OssObject file;
                if (ossFilePath.Contains("?"))
                {
                    var array = ossFilePath.Split('?');
                    ossFilePath = array[0];
                    var process = array[1].ToLower().Replace("x-oss-process=", "");
                    file = client.GetObject(new GetObjectRequest(_ossSetting.OssBucketName, ossFilePath, process));
                }
                else
                {
                    file = await client.GetObjectAsync(_ossSetting.OssBucketName, ossFilePath);
                }
                var ms = new MemoryStream();
                using (var requestStream = file.Content)
                {
                    byte[] buf = new byte[1024];
                    var len = 0;
                    while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                    {
                        ms.Write(buf, 0, len);
                    }
                }
                ms.Close();
                result.Code = Convert.ToInt32(file.HttpStatusCode);
                result.Result = ms.ToArray();
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 5000;
                result.Result = new byte[] { };
                result.Msg = $"Message:{ex.Message},StackTrace:{ex.StackTrace}";
                return result;
            }
        }

    }

    #endregion
}
