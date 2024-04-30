using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ;
using UtilsSharp.OptionConfig;
using UtilsSharp.RabbitMq;
using UtilsSharp.RabbitMq.Extension;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class RabbitMQTest
    {
        [TestMethod]
        public void Send()
        {
            RabbitMqConfig.RabbitMqSetting=new RabbitMqSetting();
            RabbitMqConfig.RabbitMqSetting.RabbitMqConnection = "amqp://alimquser:sfayxgtxkmuh@192.168.0.144:5672";
            RabbitMqManager.Register();

            //发送消息到队列
            string businessName = "test_mq";
            List<string> mqTests = new List<string>();
            mqTests.Add("测试1");
            mqTests.Add("测试2");
            mqTests.Add("测试3");
            RabbitMqHelper.SendByBusiness(businessName, mqTests);

            //消费者
            xiaofeizhe xfz = new xiaofeizhe();
            //开50个线程，每个线程每次拉1000条数据
            xfz.Execute(businessName, 50, 1000, true);


        }


        [TestMethod]
        public void GetMessage()
        {
            RabbitMqConfig.RabbitMqSetting = new RabbitMqSetting();
            RabbitMqConfig.RabbitMqSetting.RabbitMqConnection = "amqp://alimquser:sfayxgtxkmuh@192.168.0.144:5672";
            RabbitMqManager.Register();

            string businessName = "test_mq";
            //var r=RabbitMqHelper.GetMessage(businessName, true);
            //var r1 = RabbitMqHelper.GetMessage(businessName, true);

            //var r2 = RabbitMqHelper.GetMessage(businessName, false);
            RabbitMqHelper.Received(businessName, fun,  (string e) =>
            {
                var cc =e;

            });


            //var r3 = RabbitMqHelper.GetMessage(businessName, false);

            //RabbitMqHelper.BasicAck(r3.DeliveryTag,true);
            Thread.Sleep(6000);
        }

        public void fun(string aa)
        {
            var bb = aa;
        }

    }


    /// <summary>
    /// 消费者
    /// </summary>
    public class xiaofeizhe : AbsConsumer<RabbitMqHelper>
    {
        protected override void ReceiveCallBack(List<string> contents)
        {
            Console.WriteLine("contents是队列里面取出来的数据");
        }
    }

}
