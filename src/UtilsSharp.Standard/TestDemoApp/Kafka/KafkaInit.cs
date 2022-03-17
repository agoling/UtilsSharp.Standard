using System;
using System.Collections.Generic;
using System.Text;
using Kafka;
using OptionConfig;

namespace TestDemoApp.Kafka
{
    public abstract class Kafka2Helper : KafkaHelper<Kafka2Helper>{}

    public class KafkaInit
    {

        public KafkaInit()
        {
            //注册
            Register();
            //创建主题
            CreateTopic();
            //消费者
            Consumer();
            //生产者
            Producer();
        }

        //主题
        string topic = "myTopic";
        
        /// <summary>
        /// 注册
        /// </summary>
        public void Register()
        {
            #region 链接Kafka
            //kafka配置
            KafkaSetting kafkaSetting = new KafkaSetting
            {
                //集群的话以逗号隔开
                BootstrapServers = new[] { "192.168.0.56:9092" }
            };
            //注册kafka
            KafkaManager.Register<KafkaHelper>(new KafkaClient(kafkaSetting));
            #endregion
         }

        /// <summary>
        /// 注册1
        /// </summary>

        public void Register1()
        {
            #region 如果同一台机器连接另外一个kafka链接

            //kafka配置
            var kafka2Setting = new KafkaSetting
            {
                BootstrapServers = new[] { "192.168.0.57:9092" }
            };
            //注册kafka
            KafkaManager.Register<Kafka2Helper>(new KafkaClient(kafka2Setting));

            #endregion
        }

        /// <summary>
        /// 创建主题
        /// </summary>
        public void CreateTopic()
        {
            //创建主题
            if (!KafkaHelper.IsTopicExist(topic))
            {
                //ctResult为空则创建成功,否则创建失败
                var ctResult = KafkaHelper.CreateTopicsAsync(topic, 3).Result;
            }
            else
            {
                //获取已创建的主题信息
                var gtResult = KafkaHelper.GetTopic(topic);
            }

        }

        /// <summary>
        /// 消费者
        /// </summary>
        public void Consumer()
        {
            #region 消费者
            var group1 = "group.1";
            var group2 = "group.2";
            var group3 = "group.3";
            {
                var consumer = KafkaHelper.GetConsumer(group1);
                consumer.EnableAutoCommit = false;
                consumer.ListenAsync(new[] { new KafkaSubscriber() { Topic = topic, Partition = 0 } }, result =>
                {
                    Console.WriteLine($"{group1} recieve message:{result.Message}");
                    result.Commit();//手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
                }).Wait();
            }

            {
                var consumer = KafkaHelper.GetConsumer(group2);
                consumer.EnableAutoCommit = false;
                consumer.ListenAsync(new[] { new KafkaSubscriber() { Topic = topic, Partition = 1 } }, result =>
                {
                    Console.WriteLine($"{group2} recieve message:{result.Message}");
                    result.Commit();//手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
                }).Wait();
            }

            {
                var consumer = KafkaHelper.GetConsumer(group3);
                consumer.EnableAutoCommit = false;
                consumer.ListenAsync(new[] { new KafkaSubscriber() { Topic = topic, Partition = 2 } }, result =>
                {
                    Console.WriteLine($"{group3} recieve message:{result.Message}");
                    result.Commit();//手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
                }).Wait();
            }
            #endregion

        }

        /// <summary>
        /// 生产者
        /// </summary>
        public void Producer()
        {
            #region 生产者

            var producer = KafkaHelper.GetProducer();
            var index = 0;
            while (true)
            {
                Console.Write("请输入消息:");
                var line = Console.ReadLine();

                var partition = index % 3;
                producer.Send(topic, partition, "Test", line);
                index++;
            }

            #endregion

        }
        
    }

}
