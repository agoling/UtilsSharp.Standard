using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ;
using RabbitMQ.Client;
using UtilsSharp;

namespace UnitTestProjectNetCore
{
    [TestClass]
    public class RabbitMQTest
    {
        [TestMethod]
        public void Send()
        {
            RabbitMQHelper mq = new RabbitMQHelper();

            const string queueName = "my_queue";
            const string exchangeName = "my_exchanged";
            const string routingKey = "my_route";

            List<string> mqTests = new List<string>();
            mqTests.Add("测试1");
            mqTests.Add("测试2");
            mqTests.Add("测试3");
            mq.Send(ExchangeType.Direct,exchangeName, routingKey, queueName, "测试数据");

            //mq.Send(ExchangeType.Direct, exchangeName, routingKey, queueName, "我是测试aab");

            //Action<string> action = new Action<string>(shoudao);
            //mq.Receive(action);
        }

        public void shoudao(string msg)
        {

        }

    }

}
