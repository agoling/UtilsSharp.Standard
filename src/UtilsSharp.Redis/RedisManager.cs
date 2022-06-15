using System;
using CSRedis;
using UtilsSharp.OptionConfig;

namespace UtilsSharp.Redis
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
            var csRedis = new CSRedisClient(RedisConfig.RedisConnection);
            //初始化 RedisHelper
            RedisHelper.Initialization(csRedis);
            //初始化 RedisCacheHelper
            RedisCacheHelper.Initialization(csRedis);
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="T1">T1</typeparam>
        /// <param name="csRedisClient">csRedisClient</param>
        public static void Register<T,T1>(CSRedisClient csRedisClient) where T : RedisHelper<T> where T1: RedisCacheHelper<T1>
        {
            //初始化 RedisHelper
            RedisHelper<T>.Initialization(csRedisClient);
            //初始化 RedisCacheHelper
            RedisCacheHelper<T1>.Initialization(csRedisClient);
        }


        /// <summary>
        /// 获取多数据库CsRedisClient客户端
        /// </summary>
        /// <param name="redisConnection">redis连接串，无需包含：defaultDatabase</param>
        /// <param name="dbCount">数据库数量</param>
        /// <returns></returns>
        public static CSRedisClient[] GetMultipleDbCsRedisClient(string redisConnection, int dbCount=1)
        {
            if (string.IsNullOrEmpty(redisConnection))
            {
                throw new Exception("连接串不能为空");
            }
            if (redisConnection.ToLower().Contains("defaultdatabase"))
            {
                throw new Exception("连接串中不能含有defaultDatabase");
            }
            if (dbCount == 0)
            {
                throw new Exception("数据库总数不能为0");
            }
            redisConnection = redisConnection.TrimEnd(',');
            var csRedisArray = new CSRedisClient[dbCount]; //Singleton
            for (var db = 0; db < csRedisArray.Length; db++)
                csRedisArray[db] = new CSRedisClient(redisConnection + ",defaultDatabase=" + db);
            return csRedisArray;
        }
    }
}
