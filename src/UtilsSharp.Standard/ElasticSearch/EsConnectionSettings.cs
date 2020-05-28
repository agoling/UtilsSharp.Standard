using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearch
{
    /// <summary>
    /// Es链接配置
    /// </summary>
    public class BaseEsConnectionSettings
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string EsHttpAddress { get; set; }
        /// <summary>
        /// 默认索引
        /// </summary>
        public string EsDefaultIndex { get; set; }
        /// <summary>
        /// 网络代理
        /// </summary>
        public string EsNetworkProxy { get; set; }
        /// <summary>
        /// 连接数限制
        /// </summary>
        public int EsConnectionLimit { get; set; }


        /// <summary>
        /// 获取Es链接设置
        /// </summary>
        /// <returns></returns>
        public ConnectionSettings GetSettings()
        {
            var urls = EsHttpAddress.Split(';').Select(s => new Uri(s));
            //链接池
            var pool = new StaticConnectionPool(urls);
            var settings = new ConnectionSettings(pool).DefaultIndex(EsDefaultIndex);
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
