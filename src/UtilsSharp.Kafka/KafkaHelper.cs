using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace UtilsSharp.Kafka
{
    /// <summary>
    /// KafkaHelper
    /// </summary>
    public abstract class KafkaHelper : KafkaHelper<KafkaHelper> { }

    /// <summary>
    /// Kafka帮助类
    /// </summary>
    public abstract class KafkaHelper<TMark>
    {
        private static KafkaClient _instance;

        /// <summary>
        /// KafkaClient 静态实例，使用前请初始化
        /// KafkaHelper.Initialization(new KafkaClient())
        /// </summary>
        public static KafkaClient Instance
        {
            get
            {
                if (_instance == null) throw new Exception("使用前请初始化 KafkaHelper.Initialization(new KafkaClient());");
                return _instance;
            }
        }

        /// <summary>
        /// 初始化KafkaClient静态访问类
        /// KafkaHelper.Initialization(new KafkaClient())
        /// </summary>
        /// <param name="kafkaClient"></param>
        public static void Initialization(KafkaClient kafkaClient)
        {
            _instance = kafkaClient;
        }

        /// <summary>
        /// 生产者
        /// </summary>
        /// <returns></returns>
        public static KafkaProducer GetProducer()
        {
            return Instance.GetProducer();
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static KafkaConsumer GetConsumer(string groupId)
        {
            return Instance.GetConsumer(groupId);
        }

        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topicName">主题名称</param>
        /// <param name="numPartitions">Partitions数量</param>
        /// <param name="replicationFactor">replicationFactor</param>
        /// <returns>ErrorReason</returns>
        public static async Task<string> CreateTopicsAsync(string topicName, int numPartitions = 1, short replicationFactor = 1)
        {
            return await Instance.CreateTopicsAsync(topicName, numPartitions, replicationFactor);
        }


        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="topicNames">主题名称集合</param>
        /// <param name="options">参数</param>
        /// <returns></returns>
        public static async Task<string> DeleteTopicsAsync(IEnumerable<string> topicNames, DeleteTopicsOptions options = null)
        {
            return await Instance.DeleteTopicsAsync(topicNames, options);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="topicPartitionOffset">topicPartitionOffset</param>
        /// <param name="options"></param>
        /// <returns>ErrorReason</returns>
        public static async Task<string> DeleteRecordsAsync(IEnumerable<TopicPartitionOffset> topicPartitionOffset, DeleteRecordsOptions options = null)
        {
            return await Instance.DeleteRecordsAsync(topicPartitionOffset, options);
        }

        /// <summary>
        /// 主题是否存在
        /// </summary>
        /// <param name="topicName">主题名称</param>
        /// <param name="timeout">超时时间(默认10秒)</param>
        /// <returns></returns>
        public static bool IsTopicExist(string topicName, TimeSpan? timeout = null)
        {
            return Instance.IsTopicExist(topicName, timeout);
        }

        /// <summary>
        /// 获取主题
        /// </summary>
        /// <param name="topicName">主题名称</param>
        /// <param name="timeout">超时时间(默认10秒)</param>
        /// <returns></returns>
        public static TopicMetadata GetTopic(string topicName, TimeSpan? timeout = null)
        {
            return Instance.GetTopic(topicName, timeout);
        }

        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <param name="timeout">超时时间(默认10秒)</param>
        /// <returns></returns>
        public static List<TopicMetadata> GetTopics(TimeSpan? timeout = null)
        {
            return Instance.GetTopics(timeout);
        }

    }
}
