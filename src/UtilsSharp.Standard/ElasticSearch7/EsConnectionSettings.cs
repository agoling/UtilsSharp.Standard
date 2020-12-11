using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearch7
{
    /// <summary>
    /// Es链接配置
    /// </summary>
    internal class BaseEsConnectionSettings
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        internal string EsHttpAddress { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        internal string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        internal string Password { get;set; }
        /// <summary>
        /// 默认索引
        /// </summary>
        internal string EsDefaultIndex { get; set; }
        /// <summary>
        /// 网络代理
        /// </summary>
        internal string EsNetworkProxy { get; set; }
        /// <summary>
        /// 连接数限制
        /// </summary>
        internal int EsConnectionLimit { get; set; }


        /// <summary>
        /// 获取Es链接设置
        /// </summary>
        /// <returns></returns>
        internal ConnectionSettings GetSettings()
        {
            var urls = EsHttpAddress.Split(';').Select(s => new Uri(s));
            //链接池
            var pool = new StaticConnectionPool(urls);
            var settings = new ConnectionSettings(pool).DefaultIndex(EsDefaultIndex);
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                settings.BasicAuthentication(UserName, Password);
            }
            //网络代理
            if (!string.IsNullOrEmpty(EsNetworkProxy))
            {
                settings.Proxy(new Uri(EsNetworkProxy), "", "");
            }
            //连接数限制
            if (EsConnectionLimit > 0)
            {
                settings.ConnectionLimit(EsConnectionLimit);
            }
            return settings;
        }

    }

}
