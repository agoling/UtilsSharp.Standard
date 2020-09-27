using System;
using System.Collections.Generic;
using System.Text;

namespace OptionConfig
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    public static class RabbitMQConfig
    {
        /// <summary>
        /// HostName
        /// </summary>
        public static string HostName { set; get; }
        /// <summary>
        /// 端口
        /// </summary>
        public static int Port { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password { set; get; }
    }
}
