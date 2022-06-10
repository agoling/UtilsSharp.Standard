using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using OptionConfig;

namespace Kafka
{
    /// <summary>
    /// KafkaClient
    /// </summary>
    public class KafkaClient
    {
        private readonly KafkaSetting _kafkaSetting;
        private readonly IAdminClient _adminClient;

        public KafkaClient(KafkaSetting kafkaSetting)
        {
            if (kafkaSetting == null)
            {
                throw new Exception("kafkaSetting cannot be null");
            }
            if (kafkaSetting.BootstrapServers == null || kafkaSetting.BootstrapServers.Length == 0)
            {
                throw new Exception("kafka BootstrapServers cannot be null or empty");
            }

            _kafkaSetting = kafkaSetting;
            _adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = string.Join(",", kafkaSetting.BootstrapServers) }).Build();
        }

        /// <summary>
        /// 生产者
        /// </summary>
        /// <returns></returns>
        public KafkaProducer GetProducer()
        {
            var producer = new KafkaProducer(_kafkaSetting.BootstrapServers);
            return producer;
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public KafkaConsumer GetConsumer(string groupId)
        {
            var consumer = new KafkaConsumer(groupId, _kafkaSetting.BootstrapServers);
            return consumer;
        }

        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topicName">主题名称</param>
        /// <param name="numPartitions">Partitions数量</param>
        /// <param name="replicationFactor">replicationFactor</param>
        /// <returns>ErrorReason</returns>
        public async Task<string> CreateTopicsAsync(string topicName, int numPartitions = 1, short replicationFactor = 1)
        {
            try
            {
                await _adminClient.CreateTopicsAsync(new[] { new TopicSpecification { Name = topicName, NumPartitions = numPartitions, ReplicationFactor = replicationFactor } });
                return "";
            }
            catch (CreateTopicsException ex)
            {
                Console.WriteLine($"An error occured creating topic {ex.Results[0].Topic}: {ex.Results[0].Error.Reason}");
                return $"{ex.Results[0].Topic}: {ex.Results[0].Error.Reason}";
            }
        }


        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="topicNames">主题名称集合</param>
        /// <param name="options">参数</param>
        /// <returns></returns>
        public async Task<string> DeleteTopicsAsync(IEnumerable<string> topicNames, DeleteTopicsOptions options=null)
        {
            try
            {
                await _adminClient.DeleteTopicsAsync(topicNames, options);
                return "";
            }
            catch (DeleteTopicsException ex)
            {
                Console.WriteLine($"An error occured creating topic {ex.Results[0].Topic}: {ex.Results[0].Error.Reason}");
                return $"{ex.Results[0].Topic}: {ex.Results[0].Error.Reason}";
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="topicPartitionOffset">topicPartitionOffset</param>
        /// <param name="options"></param>
        /// <returns>ErrorReason</returns>
        public async Task<string> DeleteRecordsAsync(IEnumerable<TopicPartitionOffset> topicPartitionOffset, DeleteRecordsOptions options = null)
        {
            try
            {
                await _adminClient.DeleteRecordsAsync(topicPartitionOffset, options);
                return "";
            }
            catch (DeleteRecordsException ex)
            {
                Console.WriteLine($"An error occured creating topic {ex.Results[0].Topic}: {ex.Results[0].Error.Reason}");
                return $"{ex.Results[0].Topic}: {ex.Results[0].Error.Reason}";
            }
        }

        /// <summary>
        /// 主题是否存在
        /// </summary>
        /// <param name="topicName">主题名称</param>
        /// <param name="timeout">超时时间(默认10秒)</param>
        /// <returns></returns>
        public bool IsTopicExist(string topicName, TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(10);
            return _adminClient.GetMetadata(timeout.Value).Topics.Any(t => t.Topic == topicName);
        }

        /// <summary>
        /// 获取主题
        /// </summary>
        /// <param name="topicName">主题名称</param>
        /// <param name="timeout">超时时间(默认10秒)</param>
        /// <returns></returns>
        public TopicMetadata GetTopic(string topicName,TimeSpan? timeout=null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(10);
            var topic = _adminClient.GetMetadata(timeout.Value).Topics.FirstOrDefault(t => t.Topic == topicName);
            return topic;
        }

        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <param name="timeout">超时时间(默认10秒)</param>
        /// <returns></returns>
        public List<TopicMetadata> GetTopics(TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(10);
            var topics = _adminClient.GetMetadata(timeout.Value).Topics;
            return topics;
        }
    }
}
