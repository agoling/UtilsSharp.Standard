using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptionConfig;
using RabbitMQ;

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
            string businessName = "test_mq";
            List<string> mqTests = new List<string>();
            mqTests.Add("测试1");
            mqTests.Add("测试2");
            mqTests.Add("测试3");
            RabbitMqClient.Send(businessName, mqTests);

        }

    }

}
