using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ;
using RabbitMQ.Client;
using UtilsSharp.OptionConfig;
using UtilsSharp.RabbitMq;

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
            var businessName = "MyTest";

            var exchangeNameDelay = $"{businessName}.Delay.Exchange";//延迟交换机
            var routingKeyDelay = $"{businessName}.Delay.RoutingKey";//延迟路由
            var queueNameDelay = $"{businessName}.Delay.Queue";//延迟队列

            var exchangeNameDead = $"{businessName}.Dead.Exchange";//死信消息转向延迟交换机
            var routingKeyDead = $"{businessName}.Dead.RoutingKey";//死信消息转向路由

            var queueName = $"{businessName}.Queue";//过期后消息进入这个队列

            //申明交换机(延迟)
            RabbitMqHelper.ExchangeDeclare(exchangeNameDelay);
            //申明交换机(死信)
            RabbitMqHelper.ExchangeDeclare(exchangeNameDead);

            var dic = new Dictionary<string, object>();
            //dic.Add("x-expires", 30000);//队列过期时间30秒，队列30秒没有被访问，就自动删除
            dic.Add("x-message-ttl", 11000);//队列上消息过期时间，应小于队列过期时间  
            dic.Add("x-dead-letter-exchange", exchangeNameDead);//过期消息转向交换机
            dic.Add("x-dead-letter-routing-key", routingKeyDead);//过期消息转向路由相匹配routingkey  

            //申明队列(延迟)
            RabbitMqHelper.QueueDeclare(queueNameDelay, dic);
            //申明队列
            RabbitMqHelper.QueueDeclare(queueName);

            //绑定(延迟)
            RabbitMqHelper.QueueBind(queueNameDelay, exchangeNameDelay, routingKeyDelay);
            //绑定(死信)
            RabbitMqHelper.QueueBind(queueName, exchangeNameDead, routingKeyDead);


            List<string> mqTests1 = new List<string>();
            mqTests1.Add("测试1");
            mqTests1.Add("测试2");
            mqTests1.Add("测试3");
            RabbitMqHelper.Send(exchangeNameDelay, routingKeyDelay,mqTests1);
            Console.WriteLine("第一批完成");
            Thread.Sleep(10000);
            List<string> mqTests2 = new List<string>();
            mqTests2.Add("测试11");
            mqTests2.Add("测试22");
            mqTests2.Add("测试33");
            RabbitMqHelper.Send(exchangeNameDelay, routingKeyDelay, mqTests2);
            Console.WriteLine("第二批完成");
            Thread.Sleep(10000);
            List<string> mqTests3 = new List<string>();
            mqTests3.Add("测试111");
            mqTests3.Add("测试222");
            mqTests3.Add("测试333");
            RabbitMqHelper.Send(exchangeNameDelay, routingKeyDelay, mqTests3);
            Console.WriteLine("第三批完成");

            Console.ReadKey();
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
