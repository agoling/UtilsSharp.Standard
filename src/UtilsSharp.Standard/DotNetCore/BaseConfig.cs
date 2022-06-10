using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp;

namespace DotNetCore
{
    /// <summary>
    /// 基础配置信息
    /// </summary>
    public abstract class BaseConfig
    {
        /// <summary>
        /// MySql链接字符串
        /// </summary>
        public static string MySqlConnection { get; } = AppsettingsHelper.GetValue("MySqlConnection");

        /// <summary>
        /// MsSql链接字符串
        /// </summary>
        public static string MsSqlConnection { get; } = AppsettingsHelper.GetValue("MsSqlConnection");

        /// <summary>
        /// ClickHouse链接字符串
        /// </summary>
        public static string ClickHouseConnection { get; } = AppsettingsHelper.GetValue("ClickHouseConnection");
        
        /// <summary>
        /// Redis链接字符串
        /// </summary>
        public static string RedisConnection { get; } = AppsettingsHelper.GetValue("RedisConnection");

        /// <summary>
        /// rabbitMq链接字符串
        /// </summary>
        public static string RabbitMqConnection { get; } = AppsettingsHelper.GetValue("RabbitMqConnection");

        /// <summary>
        /// ElasticSearch EsSettingJson
        /// </summary>
        public static string EsSettingJson { get; } = AppsettingsHelper.GetValue("EsSettingJson");

        /// <summary>
        /// 阿里云Oss OssConfigJson
        /// </summary>
        public static string OssConfigJson { get; } = AppsettingsHelper.GetValue("OssConfigJson");

        /// <summary>
        /// 由线程池根据需要创建的新的最小工作程序线程数
        /// </summary>
        public static int MinWorkerThreadCount { get; } = !string.IsNullOrWhiteSpace(AppsettingsHelper.GetValue("MinWorkerThreadCount")) ? int.Parse(AppsettingsHelper.GetValue("MinWorkerThreadCount")) : 10;

        /// <summary>
        /// 由线程池根据需要创建的新的最小空闲异步 I/O 线程数
        /// </summary>
        public static int MinCompletionPortThreadCount { get; } = !string.IsNullOrWhiteSpace(AppsettingsHelper.GetValue("MinCompletionPortThreadCount")) ?  int.Parse(AppsettingsHelper.GetValue("MinCompletionPortThreadCount")) : 10;
    }
}
