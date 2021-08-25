using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace UtilsSharp
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public static class AppsettingsHelper
    {
        private static readonly IConfiguration Config;
        static AppsettingsHelper()
        {
            var builder = new ConfigurationBuilder();//创建config的builder
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");//设置配置文件所在的路径加载配置文件信息
            Config = builder.Build();
        }

        /// <summary>
        /// 根据key获取对应的配置值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Config[key];
        }

        /// <summary>
        /// 获取ConnectionStrings下默认的配置连接字符串
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            return Config.GetConnectionString(key);
        }

        /// <summary>
        /// 根据key获取对应节点的配置值
        /// </summary>
        /// <param name="key">key:子节点冒号隔开,如aa:bb</param>
        /// <returns></returns>
        public static IConfigurationSection GetSection(string key)
        {
            return Config.GetSection(key);
        }

        /// <summary>
        /// 根据key获取对应节点的配置值
        /// </summary>
        /// <param name="key">key:子节点冒号隔开,如aa:bb</param>
        /// <returns></returns>
        public static T GetSection<T>(string key)
        {
            var section= Config.GetSection(key);
            return section.Get<T>();
        }

        /// <summary>
        /// 获取所有配置子节点
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IConfigurationSection> GetChildren()
        {
            return Config.GetChildren();
        }
    }
}
