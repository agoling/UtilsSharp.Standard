using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.OptionConfig
{
    /// <summary>
    /// KafkaHelper配置
    /// </summary>
    public static class KafkaConfig
    {
        /// <summary>
        ///  Kafka设置
        /// </summary>
        public static KafkaSetting KafkaSetting { set; get; }

    }

    /// <summary>
    /// Kafka设置
    /// </summary>
    public class KafkaSetting
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string[] BootstrapServers { get; set; }

    }
}
