using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp.OptionConfig;

namespace UtilsSharp.Kafka
{
    /// <summary>
    /// Kafka注册管理
    /// </summary>
    public class KafkaManager
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            var kafkaClient = new KafkaClient(KafkaConfig.KafkaSetting);
            //初始化 KafkaHelper
            KafkaHelper.Initialization(kafkaClient);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="kafkaClient">kafkaClient</param>
        public static void Register<T>(KafkaClient kafkaClient)where T : KafkaHelper<T>
        {
            //初始化 KafkaHelper
            KafkaHelper<T>.Initialization(kafkaClient); ;
        }
    }
}
