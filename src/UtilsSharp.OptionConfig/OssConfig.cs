using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.OptionConfig
{
    /// <summary>
    /// OssHelper存储配置
    /// </summary>
    public static class OssConfig
    {
        /// <summary>
        /// Oss存储设置
        /// </summary>
        public static OssSetting OssSetting { set; get; }
    }

    /// <summary>
    /// Oss存储设置
    /// </summary>
    public class OssSetting
    {
        /// <summary>
        /// OssEndpoint 外网
        /// </summary>
        public string OssEndpoint { set; get; }
        /// <summary>
        /// OssEndpoint 内网
        /// </summary>
        public string OssEndpointIn { set; get; }
        /// <summary>
        /// OssAccessKeyId
        /// </summary>
        public string OssAccessKeyId { set; get; }
        /// <summary>
        /// OssAccessKeySecret
        /// </summary>
        public string OssAccessKeySecret { set; get; }
        /// <summary>
        /// OssBucketName
        /// </summary>
        public string OssBucketName { set; get; }
        /// <summary>
        /// Oss协议
        /// http://
        /// 或
        /// https://
        /// </summary>
        public string OssProtocol { set; get; } = "http://";
        /// <summary>
        /// OssHost
        /// </summary>
        public string OssHost { set; get; }
        /// <summary>
        /// Oss回调地址
        /// </summary>
        public string OssCallbackUrl { set; get; }
        /// <summary>
        /// Oss回调Host
        /// </summary>
        public string OssCallbackHost { set; get; }
        /// <summary>
        /// Oss回调过期时间
        /// </summary>
        public string OssCallbackExpireTime { set; get; }
        /// <summary>
        /// Oss文件根目录
        /// </summary>
        public string OssRootDir { set; get; }
        /// <summary>
        /// Oss临时文件根目录（oss可配置文件到期自动删除）
        /// </summary>
        public string OssRootTemporaryDir { set; get; }
    }
}
