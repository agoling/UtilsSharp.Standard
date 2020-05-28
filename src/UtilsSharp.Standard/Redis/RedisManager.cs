using System;
using System.Collections.Generic;
using CSRedis;
using OptionConfig;

namespace Redis
{
    /// <summary>
    /// Redis注册管理
    /// </summary>
    public class RedisManager
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            //初始化 RedisHelper
            CSRedisClient csredis = new CSRedisClient(RedisConfig.RedisConnection);
            RedisHelper.Initialization(csredis);
        }
    }
}
