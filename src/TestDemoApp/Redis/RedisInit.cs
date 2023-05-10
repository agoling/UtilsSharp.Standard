using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp.Redis;

namespace TestDemoApp.Redis
{
    /// <summary>
    /// 初始化  
    /// </summary>
    public class RedisInit
    {
        //同个项目连接不同的Redis库 redis库1
        public abstract class Redis1Helper : RedisHelper<Redis1Helper> { }
        public abstract class RedisCache1Helper : RedisCacheHelper<RedisCache1Helper> { }


        //同个项目连接不同的Redis库 redis库2
        public abstract class Redis2Helper : RedisHelper<Redis2Helper> { }
        public abstract class RedisCache2Helper : RedisCacheHelper<RedisCache2Helper> { }

        public static void Init()
        {
            //redis库1
            var csRedis1 = new CSRedisClient("连接串1");
            RedisManager.Register<Redis1Helper, RedisCache1Helper>(csRedis1);

            //redis库2
            var csRedis2 = new CSRedisClient("连接串2");
            RedisManager.Register<Redis2Helper, RedisCache2Helper>(csRedis2);


            //库1
            Redis1Helper.Set("aa", "你好");

            //库2
            Redis2Helper.Set("bb", "你好");
        }


    }
}
