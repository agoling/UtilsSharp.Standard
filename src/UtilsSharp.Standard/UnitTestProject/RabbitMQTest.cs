using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptionConfig;
using RabbitMQ;

namespace UnitTestProjectNetCore
{
    [TestClass]
    public class RabbitMQTest
    {
        [TestMethod]
        public void Send()
        {

            RabbitMqHelperConfig.RabbitMqConfig =new RabbitMqConfig()
            {
                RabbitMqConnection= "amqp://alimquser:sfayxgtxkmuh@192.168.0.144:5672"
            };
            
            const string queueName = "my_queue";
            const string exchangeName = "my_exchanged";
            const string routingKey = "my_route";

            RabbitMqHelper mq1 = new RabbitMqHelper();
            string cc = "";
            mq1.Send(exchangeName, routingKey,cc);
            List<string> mqTests = new List<string>();
            mqTests.Add("测试1");
            mqTests.Add("测试2");
            mqTests.Add("测试3");
            mq1.Send(exchangeName, routingKey, mqTests);

            //RabbitMQHelper1.Init("amqp://alimquser:sfayxgtxkmuh@192.168.0.144:5672");
            //RabbitMQHelper1 mq = new RabbitMQHelper1();
           


           

            //mq.QueueDeclare(queueName);
            //mq.QueueBind(queueName, exchangeName, routingKey);
            

            //Action act = () =>
            //{
            //    mq.CreateBulkHandle(queueName, data =>
            //    {
            //        Action<List<string>> action = new Action<List<string>>(shoudao);
            //        action.Invoke(data);



            //        //handles[queue].ForEach(handle =>
            //        //{
            //        //    var handleObj = Activator.CreateInstance(handle) as IHandle; //创建一个obj对象
            //        //    try
            //        //    {
            //        //        handleObj?.Execute(data);
            //        //    }
            //        //    catch (Exception e)
            //        //    {

            //        //    }
            //        //});
            //    });
            //};

            for (int i = 1; i <=1000; i++)
            {
                //act.Invoke();
                //Action<string> action = new Action<string>(shoudao);
                //mq.CreateConsumer(exchangeName, routingKey, queueName, action);
            }


            while (true)
            {
                //RabbitMQHelper mq1 = new RabbitMQHelper("amqp://alimquser:sfayxgtxkmuh@192.168.0.144:5672");
                //List<string> mqTests = new List<string>();
                //mqTests.Add("测试1");
                //mqTests.Add("测试2");
                //mqTests.Add("测试3");
                //mq.SendData(queueName, "测试数据");
            }


            //mq.Send(ExchangeType.Direct, exchangeName, routingKey, queueName, "我是测试aab");


        }

        public void shoudao(List<string> msg)
        {
            //Thread.Sleep(3000);
        }

    }

}
