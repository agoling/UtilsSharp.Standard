using System;
using System.Collections.Generic;
using System.Text;
using OptionConfig;

namespace RabbitMQ
{
    /// <summary>
    /// RabbitMq注册管理
    /// </summary>
    public class RabbitMqManager
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            var rabbitMqClient = new RabbitMqClient(RabbitMqConfig.RabbitMqSetting);
            //初始化 RabbitMqHelper
            RabbitMqHelper.Initialization(rabbitMqClient);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="rabbitMqClient">rabbitMqClient</param>
        public static void Register<T>(RabbitMqClient rabbitMqClient) where T : RabbitMqHelper<T>
        {
            //初始化 RabbitMqHelper
            RabbitMqHelper<T>.Initialization(rabbitMqClient); ;
        }
    }
}
