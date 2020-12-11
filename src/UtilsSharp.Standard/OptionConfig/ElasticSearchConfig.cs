using System;
using System.Collections.Generic;
using System.Text;

namespace OptionConfig
{
    /// <summary>
    /// ElasticSearch配置
    /// </summary>
    public static class ElasticSearchConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string EsHttpAddress { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password { get; set; }
        /// <summary>
        /// 默认索引
        /// </summary>
        public static string EsDefaultIndex { get; set; }
        /// <summary>
        /// 网络代理
        /// </summary>
        public static string EsNetworkProxy { get; set; }
        /// <summary>
        /// 连接数限制
        /// </summary>
        public static int EsConnectionLimit { get; set; }
    }
}
