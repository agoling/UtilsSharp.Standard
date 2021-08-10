using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OptionConfig;
using RabbitMQ;
using RabbitMQ.Client;

namespace TestDemoApp.RabbitMQ
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class RabbitMqInit
    {
        public static void Init()
        {
            RabbitMqConfig.RabbitMqSetting = new RabbitMqSetting();
            RabbitMqConfig.RabbitMqSetting.RabbitMqConnection = "amqp://alimquser:sfayxgtxkmuh@192.168.0.144:5672";
            RabbitMqManager.Register();
        }


        public static void SendMessage()
        {
            string businessName = "test_mq";
            List<string> mqTests = new List<string>();
            mqTests.Add("测试1");
            mqTests.Add("测试2");
            mqTests.Add("测试3");
            RabbitMqHelper.Send(businessName, mqTests);
        }


        public static void GetMessage()
        {

            string businessName = "test_mq";
            //var r=RabbitMqHelper.GetMessage(businessName, true);
            //var r1 = RabbitMqHelper.GetMessage(businessName, true);

            //var r2 = RabbitMqHelper.GetMessage(businessName, false);
            //RabbitMqHelper.Received(businessName, CallBackFunc, "direct", CallBackErrorFunc);

            var r3 = RabbitMqHelper.GetMessage(businessName,false, (c, m) =>
            {
                var aa = m;
                c.BasicAck(m.DeliveryTag, false);
            });
            Console.WriteLine(r3.Message);
        }



        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="msg">消息</param>
        public static void CallBackFunc(string msg)
        {
           Console.WriteLine(msg);
        }

        /// <summary>
        /// 错误回调
        /// </summary>
        /// <param name="msg"></param>
        public static void CallBackErrorFunc(string msg)
        {
            Console.WriteLine($"Error:{msg}");
        }

    }
}
