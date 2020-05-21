using System;
using System.Collections.Generic;
using System.Configuration;
using CSRedis;

namespace RedisCacheManager
{
    /// <summary>
    /// Redis注册管理
    /// </summary>
    public class RedisManager
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public static readonly string RedisConnection = ConfigurationManager.AppSettings["RedisConnection"];

        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            //初始化 RedisHelper
            CSRedisClient csredis = new CSRedisClient(RedisConnection);
            RedisHelper.Initialization(csredis);
        }
    }
}
