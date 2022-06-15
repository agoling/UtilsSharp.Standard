using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ;
using UtilsSharp.OptionConfig;
using UtilsSharp.RabbitMq;

namespace UnitTestProjectNet
{
    [TestClass]
    public class RabbitMQTest
    {
        [TestMethod]
        public void Send()
        {
            RabbitMqConfig.RabbitMqSetting=new RabbitMqSetting();
            RabbitMqConfig.RabbitMqSetting.RabbitMqConnection = "amqp://admin:123456@192.168.0.144:5672";
            RabbitMqManager.Register();

            string businessName = "test_mq";
            List<string> mqTests = new List<string>();
            mqTests.Add("测试11");
            mqTests.Add("测试21");
            mqTests.Add("测试31");
            RabbitMqHelper.Send(businessName, mqTests);

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
            RabbitMqHelper.Received(businessName, fun, "direct", (string e) =>
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

}
