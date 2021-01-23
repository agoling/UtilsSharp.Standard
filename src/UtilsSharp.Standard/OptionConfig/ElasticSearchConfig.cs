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
        /// ElasticSearch设置
        /// </summary>
        public static ElasticSearchSetting ElasticSearchSetting { get; set; }
    }

    /// <summary>
    /// ElasticSearch设置
    /// </summary>
    public class ElasticSearchSetting
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string EsHttpAddress { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
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
    }
}
