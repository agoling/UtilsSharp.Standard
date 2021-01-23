using System;

namespace OptionConfig
{
    /// <summary>
    /// RabbitMqHelper配置
    /// </summary>
    public static class RabbitMqConfig
    {
        /// <summary>
        /// RabbitMQ设置
        /// </summary>
        public static RabbitMqSetting RabbitMqSetting { set; get; }
    }

    /// <summary>
    /// RabbitMQ设置
    /// </summary>
    public class RabbitMqSetting
    {
        /// <summary>
        /// RabbitMQConnection
        /// </summary>
        public string RabbitMqConnection { set; get; }
        /// <summary>
        /// 请求连接超时设置
        /// </summary>
        public TimeSpan RequestedConnectionTimeout { set; get; }
        /// <summary>
        ///请求心跳设置
        /// </summary>
        public TimeSpan RequestedHeartbeat { set; get; }
        /// <summary>
        /// 是否自动恢复
        /// </summary>
        public bool? AutomaticRecoveryEnabled { set; get; }
    }


}
